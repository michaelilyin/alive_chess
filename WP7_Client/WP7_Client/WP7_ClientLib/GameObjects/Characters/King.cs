using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Statistics;
using AliveChessLibrary.Utility;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Characters
{
    [ProtoContract]
    public sealed class King : MovingEntity, IKing, IEquatable<int>, IEquatable<MapPoint>, IDynamic<King>, ISinglePoint
    {
        #region Variables

        private int? _playerId;
        private int? _animatId;

        private readonly FPosition _position;

        private IPlayer _player;
        private IEvaluator _evaluator;
        private Map _map;

        private Queue<FPosition> _steps;

        private readonly object _minesSync = new object();
        private readonly object _castlesSync = new object();
        private readonly object _stepsSync = new object();

        #endregion

        #region Constructors

        public King()
        {
            Distance = 3;
            _steps = new Queue<FPosition>();
            IsMove = false;
            State = KingState.BigMap;
            VisibleSpace = new VisibleSpace(this);
            _position = new FPosition();
            Map = null;
            Mines = new List<Mine>();
            Units = new List<Unit>();
            Castles = new List<Castle>();
            Resources = new List<Resource>();
        }

        public King(string name)
            : this()
        {
            Name = name;
        }

        #endregion

        #region Initialization

        public void AddView(MapPoint point)
        {
            ViewOnMap = point;
            ViewOnMap.SetOwner(this);
        }

        public void RemoveView()
        {
            ViewOnMap.SetOwner(null);
        }

        public void Initialize(Map map, MapPoint point)
        {
            _map = map;
            if (point != null)
                AddView(point);
        }

        public void CreateArmy()
        {

        }

        #endregion

        #region Methods

        public bool Equals(int other)
        {
            return Id.CompareTo(other) == 0;
        }

        public bool Equals(MapPoint other)
        {
            return Id.CompareTo(other.Owner.Id) == 0;
        }

        public bool InsideCastle(Castle castle)
        {
            return CurrentCastle == castle;
        }

        public void AttachStartCastle(Castle castle)
        {
            Castles.Add(castle);
            castle.King = this;
            StartCastle = castle;
            CurrentCastle = null;
        }

        public void MoveBy(Position step)
        {
            MoveBy(step.X, step.Y);
        }

        public void MoveBy(float x, float y)
        {
            var iX = (int) x;
            var iY = (int) y;
            if ((PrevX == iX && PrevY == iY) || !Map.Locate(iX, iY)) return;
            Position.X = x;
            Position.Y = y;

            _position.X = x;
            _position.Y = y;

            PrevX = X;
            PrevY = Y;

            (ViewOnMap = Map.GetObject(X, Y)).SetOwner(null);

            Debug.Assert(Map.GetObject(X, Y).Owner == Map.GetObject(X, Y).Previous);

            X = iX;
            Y = iY;

            (ViewOnMap = Map.GetObject(X, Y)).SetOwner(this);

            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, new UpdateWorldEventArgs(Map, _position, UpdateType.KingMove));

            if (Map.GetObject(X, Y).Owner != this || Map.GetObject(X, Y).Previous == this)
                Debugger.Break();
        }

        public Castle SearchCastle()
        {
            if (Castles.Count != 0)
            {
                var castle = Castles[0];
                var path = Math.Sqrt(Math.Pow(X - castle.X, 2) + Math.Pow(Y - castle.Y, 2));
                foreach (var item in Castles)
                {
                    var tmp = Math.Sqrt(Math.Pow(X - item.X, 2) + Math.Pow(Y - item.Y, 2));
                    if (tmp >= path) continue;
                    path = tmp;
                    castle = item;
                }

                return castle;
            }
            return null;
        }

        public void ComeInCastle(Castle castle)
        {
            State = KingState.Castle;

            (ViewOnMap = Map.GetObject(X, Y)).SetOwner(null);

            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, new UpdateWorldEventArgs(Map, _position, UpdateType.KingDisappear));

            IsMove = false;
            CurrentCastle = castle;
        }

        public void LeaveCastle()
        {
            (ViewOnMap = Map.GetObject(X, Y)).SetOwner(this);

            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, new UpdateWorldEventArgs(Map, _position, UpdateType.KingAppear));

            StartCastle.KingInside = false;

            State = KingState.BigMap;
            CurrentCastle = null;
        }


        public void AddMine(Mine mine)
        {
            Monitor.Enter(_minesSync);
            Mines.Add(mine);
            Monitor.Exit(_minesSync);
            Player.AddVisibleSector(mine.VisibleSpace);
            mine.SetOwner(this);
        }

        public void RemoveAllMines()
        {
            Monitor.Enter(_minesSync);
            foreach (var mine in Mines)
            {
                if (mine.Active) mine.Deactivation();
                Player.RemoveVisibleSector(mine.VisibleSpace);
                mine.SetOwner(null);
            }
            Mines.Clear();
            Monitor.Exit(_minesSync);
        }

        public void RemoveMine(Mine mine)
        {
            if (mine.Active)
                mine.Deactivation();

            Monitor.Enter(_minesSync);
            Mines.Remove(mine);
            Monitor.Exit(_minesSync);
            Player.AddVisibleSector(mine.VisibleSpace);
            mine.SetOwner(null);
        }

        public void RemoveMine(int id)
        {
            RemoveMine(GetMineById(id));
        }

        public void AddCastle(Castle castle)
        {
            Monitor.Enter(_castlesSync);
            Castles.Add(castle);
            Monitor.Exit(_castlesSync);
            Player.AddVisibleSector(castle.VisibleSpace);
            castle.SetOwner(this);
        }

        public void RemoveAllCastles()
        {
            Monitor.Enter(_castlesSync);
            foreach (var castle in Castles)
            {
                Player.RemoveVisibleSector(castle.VisibleSpace);
                castle.SetOwner(null);
            }
            Castles.Clear();
            Monitor.Exit(_castlesSync);
        }

        public void RemoveCastle(Castle castle)
        {
            Monitor.Enter(_castlesSync);
            Castles.Remove(castle);
            Monitor.Exit(_castlesSync);
            Player.RemoveVisibleSector(castle.VisibleSpace);
            castle.SetOwner(null);
        }

        public void RemoveCastle(int id)
        {
            RemoveCastle(GetCastleById(id));
        }

        public bool HasCastle(MapPoint point)
        {
            return Castles.Exists(x => x.Equals(point));
        }

        public bool HasMine(MapPoint point)
        {
            return Mines.Exists(x => x.Equals(point));
        }

        public void Update()
        {
            Updated = false;
            if (State != KingState.BigMap) return;
            if (_steps.Count == 0) return;
            Updated = true;
            lock (_stepsSync)
                DoStep(_steps.Dequeue());

            if (_steps.Count != 0) return;
            IsMove = false;
            if (UpdateSectorEvent != null)
                UpdateSectorEvent(this);
        }

        public void OutOfGame()
        {
            (ViewOnMap = Map.GetObject(X, Y)).SetOwner(null);

            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, new UpdateWorldEventArgs(Map, _position, UpdateType.KingDisappear));

            Map.RemoveKing(this);
        }

        public void AddSteps(Queue<FPosition> steps)
        {
            IsMove = true;
            lock (_stepsSync)
                _steps = steps;
        }

        public void ClearSteps()
        {
            IsMove = false;
            lock (_stepsSync)
                _steps.Clear();
        }

        public Mine GetMineById(int id)
        {
            return Mines.Search(x => x.Id == id);
        }

        public Castle GetCastleById(int id)
        {
            return Castles.Search(x => x.Id == id);
        }

        private void DoStep(IPosition<float> step)
        {
            DoStep(step.X, step.Y);
        }

        private void DoStep(float x, float y)
        {
            var @object = Map.GetObject(x, y);
            switch (@object.PointType)
            {
                case PointTypes.Landscape:
                    MoveBy(x, y);
                    break;
                case PointTypes.SingleObject:
                    MoveBy(x, y);
                    break;
                case PointTypes.MultyObject:
                    MoveBy(x, y);
                    break;
                case PointTypes.Castle:
                    FindCastle(@object);
                    break;
                case PointTypes.Mine:
                    FindMine(@object);
                    break;
                case PointTypes.King:
                    FindKing(@object);
                    break;
                case PointTypes.Resource:
                    FindResource(@object);
                    break;
            }
        }

        private void FindKing(MapPoint point)
        {
            if (Equals(point)) return;
            if (ContactWithKingEvent != null)
                ContactWithKingEvent(this, point);
        }

        private void FindMine(MapPoint mine)
        {
            if (CaptureMineEvent == null) return;
            if (!HasMine(mine))
                CaptureMineEvent(this, mine.MapSector);
        }

        private void FindCastle(MapPoint castle)
        {
            if (ComeInCastleEvent == null || ContactWithCastleEvent == null) return;
            if (HasCastle(castle))
            {
                if (ComeInCastleEvent != null)
                    ComeInCastleEvent(this, castle.MapSector);
            }
            else if (ContactWithCastleEvent != null)
                ContactWithCastleEvent(this, castle.MapSector);
        }

        private void FindResource(MapPoint resource)
        {
            if (CollectResourceEvent != null)
                CollectResourceEvent(this, resource);
        }

        #endregion

        #region Properties

        [ProtoMember(2)]
        public int X { get; set; }
        [ProtoMember(3)]
        public int Y { get; set; }

        public int PrevX { get; set; }

        public int PrevY { get; set; }
        public int ImageId { get; set; }
        public float WayCost { get; set; }

        public PointTypes Type
        {
            get { return PointTypes.King; }
        }

        public bool Updated { get; set; }

        public MapPoint ViewOnMap { get; set; }

        public IInteraction Interaction { get; set; }

        public Castle CurrentCastle { get; private set; }

        public Castle StartCastle { get; private set; }

        public int StepCount
        {
            get { return _steps.Count; }
        }

        public int Distance { get; set; }

        public KingState State { get; set; }

        public bool IsMove { get; set; }

        public bool Sleep { get; set; }

        public VisibleSpace VisibleSpace { get; set; }

        public bool IsLeader
        {
            get { return false; }
        }

        public Statistic Statistic
        {
            get { return _player.Statistics; }
        }

        public IEvaluator Evaluator
        {
            get { return _evaluator; }
            set
            {
                _evaluator = value;
                _evaluator.AttachOwner(this);
            }
        }

        public IMemory Memory
        {
            get { return _evaluator.Memory; }
        }

        public ICommunity Community
        {
            get { return _player.Community; }
        }

        public IPlayer Player
        {
            get { return _player; }
            set
            {
                if (_player != value)
                {
                    if (value != null)
                    {
                        _player = value;
                        value.AddKing(this);
                        if (!value.Bot)
                        {
                            _animatId = null;
                            _playerId = value.Id;
                        }
                        else
                        {
                            _playerId = null;
                            _animatId = value.Id;
                        }
                    }
                }
            }
        }

        [ProtoMember(1)]
        public int Id { get; set; }

        public int? PlayerId
        {
            get { return _playerId; }
            set { _playerId = value; }
        }

        public int? AnimatId
        {
            get { return _animatId; }
            set { _animatId = value; }
        }

        public int? EmpireId { get; set; }

        public int? UnionId { get; set; }

        [ProtoMember(4)]
        public string Name { get; set; }

        [ProtoMember(5)]
        public int Experience { get; set; }

        [ProtoMember(6)]
        public int MilitaryRank { get; set; }

        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        public List<Mine> Mines { get; set; }

        public List<Unit> Units { get; set; }

        public List<Castle> Castles { get; set; }

        public List<Resource> Resources { get; set; }

        #endregion

        #region Delegates

        public delegate void FindHandler(MapPoint point);
        public delegate void ComeInCastleHandler(King player, MapSector point);
        public delegate void CollectResourceHandler(King player, MapPoint point);
        public delegate void CaptureMineHandler(King player, MapSector point);
        public delegate void ContactWithCastleHandler(King player, MapSector point);
        public delegate void ContactWithKingHandler(King player, MapPoint point);
        public delegate void UpdateSectorHandler(King king);
        #endregion

        #region Events

        public event ContactWithKingHandler ContactWithKingEvent;
        public event ContactWithCastleHandler ContactWithCastleEvent;

        public event ComeInCastleHandler ComeInCastleEvent;
        public event CollectResourceHandler CollectResourceEvent;

        public event CaptureMineHandler CaptureMineEvent;

        public event UpdateSectorHandler UpdateSectorEvent;
        public event ChangeMapStateHandler<King> ChangeMapStateEvent;


        #endregion

        public int GetUnitCountFake()
        {
            return 1;
        }

        public double Power { get; set; }
        public double Wealth { get; set; }
        public double Success { get; set; }

        public double SentHelp { get; set; }
        public double ReceivedHelp { get; set; }

        public int WinNumber
        {
            get { return _player.Statistics.WinNumber; }
            set { _player.Statistics.WinNumber = value; }
        }

        public int LooseNumber
        {
            get { return _player.Statistics.LooseNumber; }
            set { _player.Statistics.LooseNumber = value; }
        }

        public int CommonBattlesNumber
        {
            get { return _player.Statistics.CommonBattlesNumber; }
            set { _player.Statistics.CommonBattlesNumber = value; }
        }

        public double TradingActivityNumber { get; set; }
    }
}