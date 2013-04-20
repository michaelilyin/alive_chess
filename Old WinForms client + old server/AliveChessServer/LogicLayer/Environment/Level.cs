using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Utility;
using AliveChessServer.DBLayer;
using AliveChessServer.LogicLayer.AI;
using AliveChessServer.LogicLayer.AI.DecisionLayer;
using AliveChessServer.LogicLayer.Environment.Aliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.Environment
{
    [Table(Name = "dbo.chess_level")]
    public class Level : ILevel, IRoutine
    {
        #region Variables

        private int _levelId;
        private int _mapId;

        private EntityRef<Map> _map;
        private EntityRef<Animat> _animat;
        private EntitySet<Player> _players;

        private EntitySet<Union> _unions;
        private EntitySet<Empire> _emperies;

        private IDictionary<int, Battle> _battles;
        private IDictionary<int, IDispute> _disputes;
       
        private GameData _context;
        private LevelTypes _levelType;

        private AlianceRoutine _aRoutine;
        private BigMapRoutine _mRoutine;
        private DisputeRoutine _dRoutine;
        private EconomyRoutine _eRoutine;

        private PlayerManager _playerManager;

        private object _unionSync = new object();
        private object _empireSync = new object();
        private object _disputeSync = new object();
        private object _battleSync = new object();
        private object _playerSync = new object();
     
        #endregion

        #region Constructors

        public Level()
        {
            this._context = new GameData();
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
            //this.Id = map.DbId;
            this._levelType = type;
            this.EmpireManager = new AlianceRoutine();
            this.BigMapRoutine = new BigMapRoutine(this, _context);
            this.DisputeRoutine = new DisputeRoutine(this, _context);
            //this.EconomyRoutine = new EconomyRoutine(this, context);
        }

        #endregion

        #region Methods

        public void DoLogic(GameTime time)
        {
            BigMapRoutine.DoLogic(time);
            DisputeRoutine.DoLogic(time);
            EmpireManager.DoLogic(time);
        }

        public Union CreateUnion(King organizator, King respondent)
        {
            if (_playerManager != null)
            {
                Union union = new Union();

                GuidIDPair guid = GuidGenerator.Instance.GeneratePair();

                //union.Id = guid.Id;
                //union.Id = guid.Guid;

                union.Kings.Add(organizator);
                union.Kings.Add(respondent);

                lock (_unionSync)
                    _unions.Add(union);
                _aRoutine.Add(union);

                union.Level = this;
                //RemoveUnion(union);
                union.PlayerManager = _playerManager;

                return union;
            }
            else throw new AliveChessException("PlayerManager is not initialized");
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

        public GameData Context
        {
            get { return _context; }
            set { _context = value; }
        }

        public BigMapRoutine BigMapRoutine
        {
            get { return _mRoutine; }
            set { _mRoutine = value; }
        }

        public DisputeRoutine DisputeRoutine
        {
            get { return _dRoutine; }
            set { _dRoutine = value; }
        }

        public EconomyRoutine EconomyRoutine
        {
            get { return _eRoutine; }
            set { _eRoutine = value; }
        }

        public AlianceRoutine EmpireManager
        {
            get { return _aRoutine; }
            set { _aRoutine = value; }
        }

        public PlayerManager PlayerManager
        {
            get { return _playerManager; }
            set
            {
                this._playerManager = value;
                if (this._aRoutine != null)
                    this._aRoutine.PlayerManager = value;
            }
        }

        public EntitySet<Player> Players
        {
            get { return this._players; }
            set { this._players.Assign(value); }
        }

        [Column(Name = "level_id", Storage = "_levelId", CanBeNull = false, DbType = Constants.DB_INT,
           IsPrimaryKey = true, IsDbGenerated = true)]
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

        [Column(Name = "map_id", Storage = "_mapId", CanBeNull = false, DbType = Constants.DB_INT)]
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

        [Column(Name = "level_type", Storage = "_levelType", CanBeNull = false, DbType = Constants.DB_INT)]
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

        [Association(Name = "fk_union_level", Storage = "_unions", OtherKey = "LevelId")]
        public EntitySet<Union> Unions
        {
            get { return this._unions; }
            set { this._unions.Assign(value); }
        }

        [Association(Name = "fk_empire_level", Storage = "_emperies", OtherKey = "LevelId")]
        public EntitySet<Empire> Emperies
        {
            get { return this._emperies; }
            set { this._emperies.Assign(value); }
        }

        #endregion
    }
}
