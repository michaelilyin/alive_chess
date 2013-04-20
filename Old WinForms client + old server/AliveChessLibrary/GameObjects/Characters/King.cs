using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Mathematic.GeometryUtils;
using AliveChessLibrary.Utility;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Characters
{
    [Serializable, ProtoContract]
    [Table(Name = "dbo.king")]
    public class King : MovingEntity, IKing, IEquatable<int>, ISinglePoint
    {
        #region Variables

        [ProtoMember(1)]
        private int _kingId;

        private int prevX;
        private int prevY;

        private int _mapId;
        private int? _playerId;
        private int? _animatId;
        private int _mapPointId;

        private int? _unionId;
        private int? _empireId;

        [ProtoMember(2)]
        private MapPoint _viewOnMap;
       
        [ProtoMember(3)]
        private string _kingName;
        [ProtoMember(4)]
        private int _kingExperience;
        [ProtoMember(5)]
        private int _kingMilitaryRank;

        private Castle _startCastle;
        private Castle _currentCastle;

        private IPlayer _player;

        private EntityRef<Map> _map;

        private EntitySet<Mine> _mines;
        private EntitySet<Unit> _units;
        private EntitySet<Castle> _castles;
        private EntitySet<Resource> _resources;

        // король вне игры
        private bool _sleep;
        // король в движении
        private bool _isMove;
        // король переместился
        private bool _updated;

        // армия короля Slisarenko
        //private IList<Unit> units = new List<Unit>();
        //фабрика юнитов
        //private FabrikUnit fab;
        //Slisarenko
        
        private KingState state;

        protected TimeSpan time;
        private Queue<Position> steps;

        private int _distance = 3;

        private Dictionary<string, FindHandler> handlers;

        private VisibleSpace sector;
        private GameData _gameData;

        private IInteraction _interaction;

        private object minesSync = new object();
        private object castlesSync = new object();
        private object stepsSync = new object();

        #endregion

        #region Constructors

        public King()
            : base()
        {
            this.time = TimeSpan.Zero;
            this.steps = new Queue<Position>();
            this._isMove = false;
            this.state = KingState.BigMap;

            this.Position = new Vector2D();

            this.handlers = new Dictionary<string, FindHandler>();
            this.handlers.Add(String.Concat("Find", PointTypes.Castle.ToString()), new FindHandler(FindCastle));
            this.handlers.Add(String.Concat("Find", PointTypes.Mine.ToString()), new FindHandler(FindMine));
            this.handlers.Add(String.Concat("Find", PointTypes.King.ToString()), new FindHandler(FindKing));
            this.handlers.Add(String.Concat("Find", PointTypes.Resource.ToString()), new FindHandler(FindResource));

            this._map = default(EntityRef<Map>);
            //this._player = default(EntityRef<Player>);
            //this._resourceStore = default(EntityRef<ResourceStore>);
            //this._figureStore = default(EntityRef<FigureStore>);
            this._mines = new EntitySet<Mine>(AttachMine, DetachMine);
            this._units = new EntitySet<Unit>(AttachUnit, DetachUnit);
            this._castles = new EntitySet<Castle>(AttachCastle, DetachCastle);
            this._resources = new EntitySet<Resource>(AttachResource, DetachResource);
            ////создать армию Slisarenko
            //this._castle.CreatStartArmy();
            ////Slisarenko
        }

        public King(MapPoint view)
            : this()
        {
            this.ViewOnMap = view;
        }

        #endregion

        #region Initialization

        public virtual void AddView(MapPoint point)
        {
            this.ViewOnMap = point;
            this._viewOnMap.ObjectUnderThis = _map.Entity[point.X, point.Y];
            this._map.Entity.SetObject(_viewOnMap);
        }

        /// <summary>
        /// динамическое создание представления
        /// </summary>
        public static MapPoint CreateView(int x, int y, ImageInfo info, GameData data)
        {
            MapPoint view = new MapPoint();
           
            view.X = x;
            view.Y = y;
           // view.DbId = guid;
            view.ImageId = info.ImageId;
            view.MapPointType = PointTypes.King;
            view.WayCost = data.GetWayCost(view.MapPointType);
            view.Detected = true;
  
            return view;
        }

        public void CreateArmy(UniqueIdHandler generator)
        {
            //this._castle.CreatStartArmy(generator);
            foreach (Unit u in _startCastle.ArmyInsideCastle)
                _units.Add(u);
        }

        #endregion

        #region Methods

        private void InitTest()
        {
            if(_viewOnMap == null)
                throw new AliveChessException("Object is not initialized");
        }

        /// <summary>
        /// сравнение королей
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(int other)
        {
            return Id.CompareTo(other) == 0 ? true : false;
        }

        /// <summary>
        /// обновление области видимости
        /// </summary>
        /// <param name="sector"></param>
        public virtual void UpdateVisibleSpace(VisibleSpace sector)
        {
            this.sector = sector;
        }

        public bool InsideCastle(Castle castle)
        {
            return this._currentCastle == castle;
        }

        public virtual void AttachStartCastle(Castle castle)
        {
            this._castles.Add(castle);
            this._startCastle = castle;
            this.StartCastle.King = this;
            this._currentCastle = null;
        }

        /// <summary>
        /// перемещение короля
        /// </summary>
        /// <param name="step"></param>
        public virtual void MoveBy(Position step)
        {
            this.MoveBy(step.X, step.Y);
        }

        /// <summary>
        /// перемещение короля
        /// </summary>
        /// <param name="step"></param>
        public virtual void MoveBy(int x, int y)
        {
            // пункт назначения на карте
            if ((this.PrevX != x || this.PrevY != y) && Map.Locate(x, y))
            {
                if (this.Position.X != x) this.Position.X = x;
                if (this.Position.Y != y) this.Position.Y = y;

                // изменяем предыдущую координату короля
                // на текущую
                this.PrevX = this.X;
                this.PrevY = this.Y;
                // убираем короля из ячейки
                Map.SetObject(_viewOnMap.ObjectUnderThis);

                // генерирум событие обновления состояния ячейки
                if (ChangeMapStateEvent != null)
                    ChangeMapStateEvent(this, Map, _viewOnMap.ObjectUnderThis);

                // присваиваем королю новую координату
                this.X = x;
                this.Y = y;

                // запоминаем ссылку на ячейку в которой находится король
                _viewOnMap.ObjectUnderThis = _map.Entity[x, y];

                Map.SetObject(_viewOnMap);
                // генерирум событие обновления состояния ячейки
                if (ChangeMapStateEvent != null)
                    ChangeMapStateEvent(this, Map, _viewOnMap);

                // ячейка посещалась. Нужно для отрисовки на клиенте
                Map[x, y].Detected = true;
            }
        }

        /// <summary>
        /// поиск ближайшего замка
        /// </summary>
        /// <returns></returns>
        public virtual Castle SearchCastle()
        {
            if (this._castles.Count != 0)
            {
                EntitySet<Castle> castles = this._castles;
                Castle castle = castles[0];
                double path = Math.Sqrt(Math.Pow(this.X - castle.X, 2) + Math.Pow(this.Y - castle.Y, 2));
                foreach (Castle item in castles)
                {
                    double tmp = Math.Sqrt(Math.Pow(this.X - item.X, 2) + Math.Pow(this.Y - item.Y, 2));
                    if (tmp < path)
                    {
                        path = tmp;
                        castle = item;
                    }
                }

                return castle;
            }
            else return null;
        }

        /// <summary>
        /// вход в замок
        /// </summary>
        /// <param name="castle"></param>
        public virtual void ComeInCastle(Castle castle)
        {
            // король в замке
            this.state = KingState.Castle;

            // убираем короля из его текущей позиции
            Map.SetObject(_viewOnMap.ObjectUnderThis);
            // генерирум событие обновления состояния ячейки
            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, Map, _viewOnMap.ObjectUnderThis);

            // присваиваем королю координату замка
            this.X = castle.X;
            this.Y = castle.Y;

            this._isMove = false;
            this._currentCastle = castle;
        }

        /// <summary>
        /// выход из замка
        /// </summary>
        public virtual void LeaveCastle()
        {
            // помещаем короля рядом с замком
            this.X = _viewOnMap.ObjectUnderThis.X;
            this.Y = _viewOnMap.ObjectUnderThis.Y;

            // обновляем ячейку куда помещен король
            this._viewOnMap.X = this.X;
            this._viewOnMap.Y = this.Y;

            Map.SetObject(_viewOnMap);

            // ячейка посещалась. Нужно для отрисовки на клиенте
            Map[this.X, this.Y].Detected = true;

            // генерирум событие обновления состояния ячейки
            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, Map, _viewOnMap);

            // отмечаем, что король покинул замок
            this._startCastle.KingInside = false;
       
            // король вне замка
            this.state = KingState.BigMap;
            this._currentCastle = null;
        }

        /// <summary>
        /// добавление замка
        /// </summary>
        /// <param name="mine"></param>
        public virtual void AddMine(Mine mine)
        {
            Monitor.Enter(minesSync);
            _mines.Add(mine);
            Monitor.Exit(minesSync);
        }

        /// <summary>
        /// деактивация и удаление всех шахт
        /// </summary>
        public virtual void RemoveAllMines()
        {
            Monitor.Enter(minesSync);
            foreach (Mine m in _mines)
                if (m.Active) m.Deactivation();
            _mines.Clear();
            Monitor.Exit(minesSync);
        }

        /// <summary>
        /// деактивация и удаление шахты
        /// </summary>
        /// <param name="mineId"></param>
        public virtual void RemoveMine(Mine mine)
        {
            if (mine.Active)
                mine.Deactivation();

            Monitor.Enter(minesSync);
            _mines.Remove(mine);
            Monitor.Exit(minesSync);
        }

        /// <summary>
        /// деактивация и удаление шахты
        /// </summary>
        /// <param name="mineId"></param>
        public virtual void RemoveMine(int id)
        {
            RemoveMine(GetMineById(id));
        }

        /// <summary>
        /// добавление шахты
        /// </summary>
        /// <param name="castle"></param>
        public virtual void AddCastle(Castle castle)
        {
            Monitor.Enter(castlesSync);
            _castles.Add(castle);
            Monitor.Exit(castlesSync);
        }

        /// <summary>
        /// удаление всех замков
        /// </summary>
        public virtual void RemoveAllCastles()
        {
            Monitor.Enter(castlesSync);
            _castles.Clear();
            Monitor.Exit(castlesSync);
        }

        /// <summary>
        /// удаление замка
        /// </summary>
        /// <param name="castleId"></param>
        public virtual void RemoveCastle(Castle castle)
        {
            Monitor.Enter(castlesSync);
            _castles.Remove(castle);
            Monitor.Exit(castlesSync);
        }

        /// <summary>
        /// удаление замка
        /// </summary>
        /// <param name="castleId"></param>
        public virtual void RemoveCastle(int id)
        {
            RemoveCastle(GetCastleById(id));
        }

        /// <summary>
        /// проверка принадлежности замка
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool HasCastle(MapPoint point)
        {
            return _castles.Exists(x => x.MapSectorId == point.Id);
        }

        /// <summary>
        /// проверка принадлежности шахты
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool HasMine(MapPoint point)
        {
            return _mines.Exists(x => x.MapSectorId == point.Id);
        }

        /// <summary>
        /// метод реализующий активность короля на сервере
        /// </summary>
        public virtual void Update()
        {
            _updated = false;
            // король на большой карте
            if (this.state == KingState.BigMap)
            {
                // каждые 50 миллисекунд
                if (time > TimeSpan.FromMilliseconds(50))
                {
                    // если королю нужно идти
                    if (steps.Count != 0)
                    {
                        _updated = true;

                        // берем шаг из очереди и в методе DoStep() принимаем решение что делать
                        lock (stepsSync)
                            DoStep(steps.Dequeue());

                        if (steps.Count == 0)
                        {
                            this._isMove = false;

                            // король достиг пункта назначения и 
                            // запрашивает область видимости (только на клиенте)
                            if (UpdateSectorEvent != null)
                                UpdateSectorEvent(this);
                        }
                    }

                    time = TimeSpan.Zero;
                }
                else time += TimeSpan.FromMilliseconds(10);
            }
        }

        /// <summary>
        /// король изчезает с карты
        /// </summary>
        public virtual void OutOfGame()
        {
            _map.Entity.SetObject(_viewOnMap.ObjectUnderThis);

            RemoveAllMines();
            RemoveAllCastles();

            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, Map, _viewOnMap.ObjectUnderThis);

            Map.RemoveKing(this);
        }

        /// <summary>
        /// получение резерва указанного замка
        /// </summary>
        /// <param name="castle"></param>
        /// <returns></returns>
        //public virtual FigureStore GetFigureStoreFrom(Castle castle)
        //{
        //    Castle c = _castles.First(x => castle.Id == castle.Id);
        //    return c != null ? c.FigureStore : null;
        //}

        /// <summary>
        /// добавить в очередь ячейки которые король должен пройти
        /// </summary>
        /// <param name="steps"></param>
        public virtual void AddSteps(Queue<Position> steps)
        {
            this._isMove = true;
            lock (stepsSync)
                this.steps = steps;
        }

        /// <summary>
        /// очистка пути
        /// </summary>
        public virtual void ClearSteps()
        {
            this._isMove = false;
            lock (stepsSync)
                steps.Clear();
        }

        /// <summary>
        /// получение шахты
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Mine GetMineById(int id)
        {
            return _mines.FindElement(x => x.Id == id);
        }

        /// <summary>
        /// получение замка
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Castle GetCastleById(int id)
        {
            return _castles.FindElement(x => x.Id == id);
        }

        /// <summary>
        /// король делает шаг который зависит от
        /// того что находится в ячейки карты в которую
        /// он этот шаг делает
        /// </summary>
        /// <param name="s"></param>
        protected void DoStep(Position step)
        {
            DoStep(step.X, step.Y);
        }

        protected void DoStep(Vector2D step)
        {
            DoStep((int)step.X, (int)step.Y);
        }

        protected void DoStep(int x, int y)
        {
            MapPoint mObject = Map[x, y];
            string objectType = mObject.MapPointType.ToString();
            if (objectType.Equals("Landscape") || objectType.Equals("SingleObject")
                || objectType.Equals("MultyObject")) MoveBy(x, y);
            else handlers[String.Concat("Find", objectType)].Invoke(mObject);
        }

        /// <summary>
        /// король встретил соперника
        /// </summary>
        /// <param name="king"></param>
        protected void FindKing(MapPoint king)
        {
            if (!this.ViewOnMap.Equals(king))
            {
                ClearSteps();
                if (ContactWithKingEvent != null)
                    ContactWithKingEvent(this, king);
            }
        }

        /// <summary>
        /// король нашел шахту
        /// </summary>
        /// <param name="mine"></param>
        protected void FindMine(MapPoint mine)
        {
            if (CaptureMineEvent != null)
            {
                if (!this.HasMine(mine))
                    CaptureMineEvent(this, mine.MapSector);
            }
        }

        /// <summary>
        /// король нашел замок
        /// </summary>
        /// <param name="castle"></param>
        protected void FindCastle(MapPoint castle)
        {
            if (ComeInCastleEvent != null && ContactWithCastleEvent != null)
            {
                if (this.HasCastle(castle))
                {
                    if (ComeInCastleEvent != null)
                        ComeInCastleEvent(this, castle.MapSector);
                }
                else if (ContactWithCastleEvent != null)
                    ContactWithCastleEvent(this, castle.MapSector);
            }
        }

        /// <summary>
        /// король нашел ресурс
        /// </summary>
        /// <param name="resource"></param>
        protected void FindResource(MapPoint resource)
        {
            if (CollectResourceEvent != null)
                CollectResourceEvent(this, resource);
            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, _map.Entity, resource.ObjectUnderThis);
        }

        #region Attach handlers

        private void AttachCastle(Castle entity)
        {
            entity.King = this;
        }

        private void DetachCastle(Castle entity)
        {
            entity.King = null;
        }

        private void AttachMine(Mine entity)
        {
            entity.King = this;
        }

        private void DetachMine(Mine entity)
        {
            entity.King = null;
        }

        private void AttachUnit(Unit entity)
        {
            entity.King = this;
        }

        private void DetachUnit(Unit entity)
        {
            entity.King = null;
        }

        private void AttachResource(Resource entity)
        {
            entity.King = this;
        }

        private void DetachResource(Resource entity)
        {
            entity.King = null;
        }

        #endregion

        #endregion

        #region Properties

        public virtual int X
        {
            get
            {
                InitTest();
                return _viewOnMap.X;
            }
            set
            {
                InitTest();
                _viewOnMap.X = value;
            }
        }

        public virtual int Y
        {
            get
            {
                InitTest();
                return _viewOnMap.Y;
            }
            set
            {
                InitTest();
                _viewOnMap.Y = value;
            }
        }

        public virtual int PrevX
        {
            get { return prevX; }
            set { prevX = value; }
        }

        public virtual int PrevY
        {
            get { return prevY; }
            set { prevY = value; }
        }

        public bool Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }

        public IInteraction Interaction
        {
            get { return _interaction; }
            set { _interaction = value; }
        }

        public virtual Castle CurrentCastle
        {
            get { return _currentCastle; }
        }

        public virtual Castle StartCastle
        {
            get { return _startCastle; }
        }

        public int StepCount { get { return steps.Count; } }

        public virtual int Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        public virtual GameData GameData
        {
            get { return _gameData; }
            set { _gameData = value; }
        }

        public virtual KingState State
        {
            get { return state; }
            set { state = value; }
        }

        public virtual bool IsMove
        {
            get { return _isMove; }
            set { _isMove = value; }
        }

        public virtual bool Sleep
        {
            get { return _sleep; }
            set { _sleep = value; }
        }

        public virtual VisibleSpace VisibleSpace
        {
            get { return sector; }
            set { sector = value; }
        }

        public virtual bool IsLeader 
        { 
            get { return false; } 
        }

        /// <summary>
        /// ссылка на ячейку на карте
        /// </summary>
        public virtual MapPoint ViewOnMap
        {
            get
            {
                return this._viewOnMap;
            }
            set
            {
                if (_viewOnMap != value)
                {
                    if (value != null)
                    {
                        _viewOnMap = value;
                        _mapPointId = _viewOnMap.Id;
                    }
                }
            }
        }
        /// <summary>
        /// ссылка на игрока
        /// </summary>
        public virtual IPlayer Player
        {
            get
            {
                return this._player;
            }
            set
            {
                if (_player != value)
                {
                    if (value != null)
                    {
                        this._player = value;
                        value.AddKing(this);
                        if (!value.Bot)
                        {
                            this._animatId = null;
                            this._playerId = value.Id;
                        }
                        else
                        {
                            this._playerId = null;
                            this._animatId = value.Id;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// идентификатор короля (первичный ключ)
        /// </summary>
        [Column(Name = "king_id", Storage = "_kingId", CanBeNull = false, DbType = Constants.DB_INT,
            IsPrimaryKey = true, IsDbGenerated = true)]
        public virtual int Id
        {
            get
            {
                return this._kingId;
            }
            set
            {
                if (this._kingId != value)
                {
                    this._kingId = value;
                }
            }
        }

        [Column(Name = "player_id", Storage = "_playerId", CanBeNull = true, DbType = Constants.DB_INT,
            UpdateCheck = UpdateCheck.Never)]
        public int? PlayerId
        {
            get
            {
                return this._playerId;
            }
            set
            {
                if (this._playerId != value)
                {
                    this._playerId = value;
                }
            }
        }

        [Column(Name = "animat_id", Storage = "_animatId", CanBeNull = true, DbType = Constants.DB_INT,
            UpdateCheck = UpdateCheck.Never)]
        public int? AnimatId
        {
            get
            {
                return this._animatId;
            }
            set
            {
                if (this._animatId != value)
                {
                    this._animatId = value;
                }
            }
        }

        [Column(Name = "map_point_id", Storage = "_mapPointId", CanBeNull = false, DbType = Constants.DB_INT,
            UpdateCheck = UpdateCheck.Never)]
        public int MapPointId
        {
            get
            {
                return this._mapPointId;
            }
            set
            {
                if (this._mapPointId != value)
                {
                    this._mapPointId = value;
                }
            }
        }

        /// <summary>
        /// идентификатор карты (внешний ключ)
        /// </summary>
        [Column(Name = "map_id", Storage = "_mapId", CanBeNull = false, DbType = Constants.DB_INT,
            UpdateCheck = UpdateCheck.Never)]
        public virtual int MapId
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

        /// <summary>
        /// идентификатор империи (внешний ключ)
        /// </summary>
        [Column(Name = "empire_id", Storage = "_empireId", CanBeNull = true, DbType = Constants.DB_INT,
            UpdateCheck = UpdateCheck.Never)]
        public virtual int? EmpireId
        {
            get
            {
                return this._empireId;
            }
            set
            {
                if (this._empireId != value)
                {
                    this._empireId = value;
                }
            }
        }

        /// <summary>
        /// идентификатор союза (внешний ключ)
        /// </summary>
        [Column(Name = "union_id", Storage = "_unionId", CanBeNull = true, DbType = Constants.DB_INT,
            UpdateCheck = UpdateCheck.Never)]
        public virtual int? UnionId
        {
            get
            {
                return this._unionId;
            }
            set
            {
                if (this._unionId != value)
                {
                    this._unionId = value;
                }
            }
        }

        /// <summary>
        /// имя короля
        /// </summary>
        [Column(Name = "king_name", Storage = "_kingName", CanBeNull = false, DbType = "varchar(20)")]
        public virtual string Name
        {
            get
            {
                return this._kingName;
            }
            set
            {
                if (this._kingName != value)
                {
                    this._kingName = value;
                }
            }
        }

        /// <summary>
        /// опыт короля
        /// </summary>
        [Column(Name = "king_experience", Storage = "_kingExperience", CanBeNull = false,
            DbType = Constants.DB_INT, UpdateCheck = UpdateCheck.Never)]
        public virtual int Experience
        {
            get
            {
                return this._kingExperience;
            }
            set
            {
                if (this._kingExperience != value)
                {
                    this._kingExperience = value;
                }
            }
        }

        /// <summary>
        /// воинское звание короля
        /// </summary>
        [Column(Name = "king_military_rank", Storage = "_kingMilitaryRank", CanBeNull = false,
            DbType = Constants.DB_INT, UpdateCheck = UpdateCheck.Never)]
        public virtual int MilitaryRank
        {
            get
            {
                return this._kingMilitaryRank;
            }
            set
            {
                if (this._kingMilitaryRank != value)
                {
                    this._kingMilitaryRank = value;
                }
            }
        }

        /// <summary>
        /// ссылка на карту
        /// </summary>
        [Association(Name = "fk_king_map", Storage = "_map", ThisKey = "MapId", IsForeignKey = true)]
        public virtual Map Map
        {
            get
            {
                return this._map.Entity;
            }
            set
            {
                if (_map.Entity != value)
                {
                    if (_map.Entity != null)
                    {
                        var previousMap = _map.Entity;
                        _map.Entity = null;
                        previousMap.Kings.Remove(this);
                    }

                    if (value != null)
                    {
                        _mapId = value.Id;
                        _map.Entity = value;
                        sector = new VisibleSpace(value);
                    }
                }
            }
        }

        /// <summary>
        /// список шахт
        /// </summary>
        [Association(Name = "fk_mine_king", Storage = "_mines", OtherKey = "KingId")]
        public virtual EntitySet<Mine> Mines
        {
            get
            {
                return this._mines;
            }
            set
            {
                this._mines.Assign(value);
            }
        }

        /// <summary>
        /// список замков
        /// </summary>
        [Association(Name = "fk_castle_king", Storage = "_castles", OtherKey = "KingId")]
        public virtual EntitySet<Castle> Castles
        {
            get
            {
                return this._castles;
            }
            set
            {
                this._castles.Assign(value);
            }
        }

        /// <summary>
        /// список юнитов
        /// </summary>
        [Association(Name = "fk_unit_king", Storage = "_units", OtherKey = "KingId")]
        public virtual EntitySet<Unit> Units
        {
            get
            {
                return this._units;
            }
            set
            {
                this._units.Assign(value);
            }
        }

        /// <summary>
        /// список ресурсов
        /// </summary>
        [Association(Name = "fk_resource_king", Storage = "_resources", OtherKey = "KingId")]
        public virtual EntitySet<Resource> Resources
        {
            get
            {
                return this._resources;
            }
            set
            {
                this._resources.Assign(value);
            }
        }

        #endregion

        #region Delegates

        public delegate void FindHandler(MapPoint point); // делегат взаимодействия с объектом карты

        public delegate void ComeInCastleHandler(King player, MapSector point); // событийный делегат. Вход в замок
        public delegate void CollectResourceHandler(King player, MapPoint point); // событийный делегат. Сбор ресурса

        public delegate void CaptureMineHandler(King player, MapSector point); // событийный делегат. Захват шахты
        public delegate void ContactWithCastleHandler(King player, MapSector point); // событийный делегат. Захват замка

        public delegate void ContactWithKingHandler(King player, MapPoint point); // событийный делегат. Встреча с другим королем

        public delegate void UpdateSectorHandler(King king); // событийный делегат. Передвижение короля
        public delegate void ChangeMapStateHandler(King king, Map map, MapPoint point); // событийный делегат. Изменение ячейки на карте

        #endregion

        #region Events

        public event ContactWithKingHandler ContactWithKingEvent;
        public event ContactWithCastleHandler ContactWithCastleEvent;

        public event ComeInCastleHandler ComeInCastleEvent;
        public event CollectResourceHandler CollectResourceEvent;

        public event CaptureMineHandler CaptureMineEvent;

        public event UpdateSectorHandler UpdateSectorEvent;
        public event ChangeMapStateHandler ChangeMapStateEvent;

        #endregion

        public int GetUnitCountFAKE()
        {
            return 1;
        }
    }
}
