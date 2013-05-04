using System.Collections.Generic;
using System.Data.Linq;
using AliveChessLibrary;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;
using AliveChessServer.DBLayer.Loaders;
using AliveChessServer.LogicLayer.AI;
using AliveChessServer.LogicLayer.EconomyEngine;
using AliveChessServer.LogicLayer.Environment.Alliances;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.LogicLayer.Environment
{
    public class Level : ILevel, IRoutine
    {
        #region Variables

        private int _levelId;
        private int _mapId;

        private GameTime _bigMapTime;
        private GameTime _dialogTime;
        private GameTime _allianceTime;
        private GameTime _economyTime;

        private EntityRef<Map> _map;
        private EntityRef<Animat> _animat;
        private EntitySet<Player> _players;

        private EntitySet<Union> _unions;
        private EntitySet<Empire> _emperies;

        private IDictionary<int, Battle> _battles;
        private IDictionary<int, IDispute> _disputes;
       
        private LevelTypes _levelType;
        private TimeManager _timeManager;
       
        private AllianceRoutine _alianceRoutine;
        private BigMapRoutine _bigMapRoutine;
        private DisputeRoutine _disputeRoutine;
        private EconomyRoutine _economyRoutine;
        private BattleRoutine _battleRoutine;
      
        private PlayerManager _playerManager;

        private readonly object _unionSync = new object();
        private readonly object _empireSync = new object();
        private readonly object _disputeSync = new object();
        private readonly object _battleSync = new object();
        private readonly object _playerSync = new object();
     
        #endregion

        #region Constructors

        public Level()
        {
            this._players = new EntitySet<Player>(AttachPlayer, DetachPlayer);
            this._emperies = new EntitySet<Empire>(AttachEmpire, DetachEmpire);
            this._unions = new EntitySet<Union>(AttachUnion, DetachUnion);

            this._battles = new Dictionary<int, Battle>();
            this._disputes = new Dictionary<int, IDispute>();
        }

        public Level(Map map, LevelTypes type)
            : this()
        {
            this.Map = map;
            this._levelType = type;
        }

        #endregion

        #region Methods

        public void Initialize(TimeManager timeManager, PlayerManager playerManager, AliveChessLogger logger, IEconomyLoader economyLoader)
        {
            this._timeManager = timeManager;
            this._playerManager = playerManager;
            this._alianceRoutine = new AllianceRoutine(timeManager);
            this._bigMapRoutine = new BigMapRoutine(this, timeManager, logger);
            this._disputeRoutine = new DisputeRoutine(this, timeManager);
            this._battleRoutine = new BattleRoutine(timeManager);
            this._economyRoutine = new EconomyRoutine(this, timeManager);

            this._alianceRoutine.PlayerManager = playerManager;
            this._bigMapRoutine.PlayerManager = playerManager;
            this._disputeRoutine.PlayerManager = playerManager;
            this._economyRoutine.PlayerManager = playerManager;

            this._bigMapTime   = new GameTime();
            this._dialogTime = new GameTime();
            this._allianceTime = new GameTime();
            this._economyTime = new GameTime();

            this._timeManager.AddTime(_bigMapTime);
            this._timeManager.AddTime(_dialogTime);
            this._timeManager.AddTime(_allianceTime);
            this._timeManager.AddTime(_economyTime);

            Economy economy = economyLoader.LoadEconomy(this.LevelType);
            _economyRoutine.Initialize(economy);
            ActivateMines();
        }

        public void Update()
        {
            BigMapRoutine.Update(_bigMapTime);
            DisputeRoutine.Update(_dialogTime);
            EmpireManager.Update(_allianceTime);
            EconomyRoutine.Update(_economyTime);
        }

        public Union CreateUnion(King organizator, King respondent)
        {
            if (_playerManager == null)
                throw new AliveChessException("PlayerManager is not initialized");

            Union union = new Union();

            union.Kings.Add(organizator);
            union.Kings.Add(respondent);

            lock (_unionSync)
                _unions.Add(union);
            _alianceRoutine.Add(union);

            union.Level = this;
            union.PlayerManager = _playerManager;

            return union;
        }

        private void ActivateMines()
        {
            foreach (var mine in Map.Mines)
            {
                mine.GetResourceEvent += _economyRoutine.SendResource;
                mine.Activation();
            }
        }

        #region Add and Remove

        public void AddDispute(IDispute d)
        {
            lock (_disputeSync)
                _disputes.Add(d.Id, d);
        }

        public void RemoveDispute(IDispute d)
        {
            lock (_disputeSync)
                _disputes.Remove(d.Id);
        }

        public void AddBattle(Battle b)
        {
            lock (_battleSync)
                _battles.Add(b.Id, b);
        }

        public void RemoveBattle(Battle b)
        {
            lock (_battleSync)
                _battles.Remove(b.Id);
        }

        public void AddPlayer(Player player)
        {
            lock (_playerSync)
                _players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            lock (_playerSync)
                _players.Remove(player);
        }

        public void AddUnion(Union union)
        {
            lock (_unionSync)
                _unions.Add(union);
        }

        public void RemoveUnion(Union union)
        {
            lock (_unionSync)
                _unions.Remove(union);
        }

        public void AddEmpire(Empire empire)
        {
            lock (_empireSync)
                _emperies.Add(empire);
        }

        public void RemoveEmpire(Empire empire)
        {
            lock (_empireSync)
                _emperies.Remove(empire);
        }

        #endregion

        #region Attach and Detach

        private void AttachPlayer(Player entity)
        {
            entity.LevelId = this.Id;
        }

        private void DetachPlayer(Player entity)
        {
            entity.LevelId = -1;
        }

        private void AttachEmpire(Empire entity)
        {
            entity.Level = this;
        }

        private void DetachEmpire(Empire entity)
        {
            entity.Level = null;
        }

        private void AttachUnion(Union entity)
        {
            entity.Level = this;
        }

        private void DetachUnion(Union entity)
        {
            entity.Level = null;
        }

        #endregion

        #endregion

        #region Properties

        public Map Map
        {
            get { return _map.Entity; }
            set
            {
                if (_map.Entity != value)
                {
                    if (value != null)
                    {
                        _map.Entity = value;
                        _mapId = value.Id;
                        _map.Entity.Level = this;
                    }
                }
            }
        }

        public Animat Animat
        {
            get { return this._animat.Entity; }
            set
            {
                if (_animat.Entity != value)
                {
                    if (value != null)
                    {
                        _animat.Entity = value;
                    }
                }
            }
        }

        public BigMapRoutine BigMapRoutine
        {
            get { return _bigMapRoutine; }
            set { _bigMapRoutine = value; }
        }

        public DisputeRoutine DisputeRoutine
        {
            get { return _disputeRoutine; }
            set { _disputeRoutine = value; }
        }

        public EconomyRoutine EconomyRoutine
        {
            get { return _economyRoutine; }
            set { _economyRoutine = value; }
        }

        public AllianceRoutine EmpireManager
        {
            get { return _alianceRoutine; }
            set { _alianceRoutine = value; }
        }

        public BattleRoutine BattleRoutine
        {
            get { return _battleRoutine; }
            set { _battleRoutine = value; }
        }

        //public PlayerManager PlayerManager
        //{
        //    get { return _playerManager; }
        //    set
        //    {
        //        this._playerManager = value;
        //        if (this._alianceRoutine != null)
        //            this._alianceRoutine.PlayerManager = value;
        //    }
        //}

        public EntitySet<Player> Players
        {
            get { return this._players; }
            set { this._players.Assign(value); }
        }

        public int Id
        {
            get
            {
                return this._levelId;
            }
            set
            {
                if (this._levelId != value)
                {
                    this._levelId = value;
                }
            }
        }

        public int MapId
        {
            get
            {
                return this._mapId;
            }
            set
            {
                if (this._mapId != value)
                {
                    if (this._map.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._mapId = value;
                }
            }
        }

        public LevelTypes LevelType
        {
            get
            {
                return this._levelType;
            }
            set
            {
                if (this._levelType != value)
                {
                    this._levelType = value;
                }
            }
        }

        public EntitySet<Union> Unions
        {
            get { return this._unions; }
            set { this._unions.Assign(value); }
        }

        public EntitySet<Empire> Emperies
        {
            get { return this._emperies; }
            set { this._emperies.Assign(value); }
        }

        #endregion
    }
}
