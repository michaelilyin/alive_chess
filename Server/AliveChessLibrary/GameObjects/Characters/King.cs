using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Mathematic.GeometryUtils;
using AliveChessLibrary.Statistics;
using AliveChessLibrary.Utility;
using AliveChessLibrary.Interfaces;
using ProtoBuf;
#if !UNITY_EDITOR
using System.Data.Linq;
#endif

namespace AliveChessLibrary.GameObjects.Characters
{
    /// <summary>
    /// король
    /// </summary>
    [Serializable, ProtoContract]
    public class King : MovingEntity, IKing, IEquatable<int>, IEquatable<MapPoint>, IDynamic<King>, ISinglePoint
    {
        #region Variables

        [ProtoMember(1)]
        private int _kingId;
        [ProtoMember(2)]
        private int _x;
        [ProtoMember(3)]
        private int _y;
        [ProtoMember(4)]
        private string _kingName;
        [ProtoMember(5)]
        private int _kingExperience;
        [ProtoMember(6)]
        private int _kingMilitaryRank;

        private int _imageId;
        private float _wayCost;

        private int _prevX;
        private int _prevY;

        private int? _mapId;
        private int? _playerId;
        private int? _animatId;
      
        private int? _unionId;
        private int? _empireId;

        private MapPoint _viewOnMap;
        private readonly FPosition _position;

        private Castle _startCastle;
        private Castle _currentCastle;

        private IPlayer _player;
        private IEvaluator _evaluator;
      
#if !UNITY_EDITOR
        private EntityRef<Map> _map;

        private readonly EntitySet<Mine> _mines;
        private readonly EntitySet<Unit> _units;
        private readonly EntitySet<Castle> _castles;
        private readonly EntitySet<Resource> _resources;
#else
        private Map _map;
        private List<Mine> _mines;
        private List<Unit> _units;
        private List<Castle> _castles;
        private List<Resource> _resources;
#endif
        // король вне игры
        private bool _sleep;
        // король в движении
        private bool _isMove;
        // король переместился
        private bool _updated;

        private KingState _state;

        protected TimeSpan Time;
        private Queue<FPosition> _steps;

        private int _distance = 3;

        private VisibleSpace _sector;
      
        private IInteraction _interaction;

        private readonly object _minesSync = new object();
        private readonly object _castlesSync = new object();
        private readonly object _stepsSync = new object();

        #endregion

        #region Constructors

        public King()
        {
            this.Time = TimeSpan.Zero;
            this._steps = new Queue<FPosition>();
            this._isMove = false;
            this._state = KingState.BigMap;
            this._sector = new VisibleSpace(this);
            this._position = new FPosition();

#if !UNITY_EDITOR
            this._map = default(EntityRef<Map>);
            this._mines = new EntitySet<Mine>();
            this._units = new EntitySet<Unit>();
            this._castles = new EntitySet<Castle>();
            this._resources = new EntitySet<Resource>();
#else
            this.Map = null;
            this.Mines = new List<Mine>();
            this.Units = new List<Unit>();
            this.Castles = new List<Castle>();
            this.Resources = new List<Resource>();
#endif
        }

        public King(string name)
            : this()
        {
            this._kingName = name;
        }

        #endregion

        #region Initialization

        /// <summary>
        /// добавление представления на карту
        /// </summary>
        /// <param name="point"></param>
        public virtual void AddView(MapPoint point)
        {
            this._viewOnMap = point;
            this._viewOnMap.SetOwner(this);
        }

        /// <summary>
        /// удаление с карты ячейки
        /// </summary>
        public void RemoveView()
        {
            ViewOnMap.SetOwner(null);
        }

        /// <summary>
        /// инициализация
        /// </summary>
        /// <param name="map">карта</param>
        /// <param name="point">позиция</param>
        public virtual void Initialize(Map map, MapPoint point)
        {
            _map.Entity = map;
            _mapId = map.Id;
            if (point != null)
                AddView(point);
        }

        public void CreateArmy()
        {
            //this._castle.CreatStartArmy(generator);
            //foreach (Unit u in _startCastle.ArmyInsideCastle)
            //    Units.Add(u);
        }

        #endregion

        #region Methods

        /// <summary>
        /// сравнить по идентификатору
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(int other)
        {
            return Id.CompareTo(other) == 0 ? true : false;
        }

        /// <summary>
        /// сравнить по ячейке
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(MapPoint other)
        {
            return Id.CompareTo(other.Owner.Id) == 0 ? true : false;
        }

        /// <summary>
        /// проверить нахождение в замке
        /// </summary>
        /// <param name="castle"></param>
        /// <returns></returns>
        public virtual bool InsideCastle(Castle castle)
        {
            return _currentCastle == castle;
        }

        /// <summary>
        /// назначить начальный замок
        /// </summary>
        /// <param name="castle"></param>
        public virtual void AttachStartCastle(Castle castle)
        {
            Castles.Add(castle);
            castle.King = this;
            this._startCastle = castle;
            this._currentCastle = null;
        }

        /// <summary>
        /// перемещение короля
        /// </summary>
        /// <param name="step"></param>
        public virtual void MoveBy(Position step)
        {
            MoveBy(step.X, step.Y);
        }

        /// <summary>
        /// перемещение короля
        /// </summary>
        /// <param name="step"></param>
        public virtual void MoveBy(float x, float y)
        {
            int iX = (int) x;
            int iY = (int) y;
            if ((this.PrevX != iX || this.PrevY != iY) && Map.Locate(iX, iY))
            {
                this.Position.X = x;
                this.Position.Y = y;

                this._position.X = x;
                this._position.Y = y;

                this.PrevX = this.X;
                this.PrevY = this.Y;

                (ViewOnMap = Map.GetObject(X, Y)).SetOwner(null);

                Debug.Assert(Map.GetObject(X, Y).Owner == Map.GetObject(X, Y).Previous);

                this.X = iX;
                this.Y = iY;

                (ViewOnMap = Map.GetObject(X, Y)).SetOwner(this);

                if (ChangeMapStateEvent != null)
                    ChangeMapStateEvent(this, new UpdateWorldEventArgs(Map, _position, UpdateType.KingMove));

                if (Map.GetObject(X, Y).Owner != this || Map.GetObject(X, Y).Previous == this)
                    Debugger.Break();
            }
        }

        /// <summary>
        /// поиск ближайшего замка
        /// </summary>
        /// <returns></returns>
        public virtual Castle SearchCastle()
        {
            if (this.Castles.Count != 0)
            {
                Castle castle = Castles[0];
                double path = Math.Sqrt(Math.Pow(this.X - castle.X, 2) + Math.Pow(this.Y - castle.Y, 2));
                foreach (Castle item in Castles)
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
            this._state = KingState.Castle;

            (ViewOnMap = Map.GetObject(X, Y)).SetOwner(null);

            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, new UpdateWorldEventArgs(Map, _position, UpdateType.KingDisappear));

            this._isMove = false;
            this._currentCastle = castle;
        }

        /// <summary>
        /// выход из замка
        /// </summary>
        public virtual void LeaveCastle()
        {
            (ViewOnMap = Map.GetObject(X, Y)).SetOwner(this);
           
            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, new UpdateWorldEventArgs(Map, _position, UpdateType.KingAppear));

            this._startCastle.KingInside = false;
       
            this._state = KingState.BigMap;
            this._currentCastle = null;
        }

        /// <summary>
        /// добавление замка
        /// </summary>
        /// <param name="mine"></param>
        public virtual void AddMine(Mine mine)
        {
            Monitor.Enter(_minesSync);
            Mines.Add(mine);
            Monitor.Exit(_minesSync);
            Player.AddVisibleSector(mine.VisibleSpace);
            mine.SetOwner(this);
        }

        /// <summary>
        /// деактивация и удаление всех шахт
        /// </summary>
        public virtual void RemoveAllMines()
        {
            Monitor.Enter(_minesSync);
            foreach (Mine mine in Mines)
            {
                if (mine.Active) mine.Deactivation();
                Player.RemoveVisibleSector(mine.VisibleSpace);
                mine.SetOwner(null);
            }
            Mines.Clear();
            Monitor.Exit(_minesSync);
        }

        /// <summary>
        /// деактивация и удаление шахты
        /// </summary>
        /// <param name="mineId"></param>
        public virtual void RemoveMine(Mine mine)
        {
            if (mine.Active)
                mine.Deactivation();

            Monitor.Enter(_minesSync);
            Mines.Remove(mine);
            Monitor.Exit(_minesSync);
            Player.AddVisibleSector(mine.VisibleSpace);
            mine.SetOwner(null);
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
            Monitor.Enter(_castlesSync);
            Castles.Add(castle);
            Monitor.Exit(_castlesSync);
            Player.AddVisibleSector(castle.VisibleSpace);
            castle.SetOwner(this);
        }

        /// <summary>
        /// удаление всех замков
        /// </summary>
        public virtual void RemoveAllCastles()
        {
            Monitor.Enter(_castlesSync);
            foreach (Castle castle in Castles)
            {
                Player.RemoveVisibleSector(castle.VisibleSpace);
                castle.SetOwner(null);
            }
            Castles.Clear();
            Monitor.Exit(_castlesSync);
        }

        /// <summary>
        /// удаление замка
        /// </summary>
        /// <param name="castleId"></param>
        public virtual void RemoveCastle(Castle castle)
        {
            Monitor.Enter(_castlesSync);
            Castles.Remove(castle);
            Monitor.Exit(_castlesSync);
            Player.RemoveVisibleSector(castle.VisibleSpace);
            castle.SetOwner(null);
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
            return Castles.Exists(x => x.Equals(point));
        }

        /// <summary>
        /// проверка принадлежности шахты
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool HasMine(MapPoint point)
        {
            return Mines.Exists(x => x.Equals(point));
        }

        /// <summary>
        /// метод реализующий активность короля на сервере
        /// </summary>
        public virtual void Update()
        {
            this._updated = false;
            // король на большой карте
            if (this._state == KingState.BigMap)
            {
                // если королю нужно идти
                if (_steps.Count != 0)
                {
                    this._updated = true;
                    // берем шаг из очереди и в методе DoStep() принимаем решение что делать
                    lock (_stepsSync)
                        DoStep(_steps.Dequeue());

                    if (_steps.Count == 0)
                    {
                        this._isMove = false;
                        // король достиг пункта назначения и 
                        // запрашивает область видимости (только на клиенте)
                        if (UpdateSectorEvent != null)
                            UpdateSectorEvent(this);
                    }
                }
            }
        }

        /// <summary>
        /// король изчезает с карты
        /// </summary>
        public virtual void OutOfGame()
        {
            (ViewOnMap = Map.GetObject(X, Y)).SetOwner(null);

            //RemoveAllMines();
            //RemoveAllCastles();

            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, new UpdateWorldEventArgs(Map, _position, UpdateType.KingDisappear));

            Map.RemoveKing(this);
        }

        /// <summary>
        /// добавить в очередь ячейки которые король должен пройти
        /// </summary>
        /// <param name="steps"></param>
        public virtual void AddSteps(Queue<FPosition> steps)
        {
            this._isMove = true;
            lock (_stepsSync)
                this._steps = steps;
        }

        /// <summary>
        /// очистка пути
        /// </summary>
        public virtual void ClearSteps()
        {
            this._isMove = false;
            lock (_stepsSync)
                _steps.Clear();
        }

        /// <summary>
        /// получение шахты
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Mine GetMineById(int id)
        {
            return Mines.Search(x => x.Id == id);
        }

        /// <summary>
        /// получение замка
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Castle GetCastleById(int id)
        {
            return Castles.Search(x => x.Id == id);
        }

        /// <summary>
        /// король делает шаг который зависит от
        /// того что находится в ячейки карты в которую
        /// он этот шаг делает
        /// </summary>
        /// <param name="s"></param>
        protected void DoStep(FPosition step)
        {
            DoStep(step.X, step.Y);
        }

        /// <summary>
        /// король делает шаг который зависит от
        /// того что находится в ячейки карты в которую
        /// он этот шаг делает
        /// </summary>
        /// <param name="s"></param>
        protected void DoStep(Vector2D step)
        {
            DoStep((int)step.X, (int)step.Y);
        }

        /// <summary>
        /// король делает шаг который зависит от
        /// того что находится в ячейки карты в которую
        /// он этот шаг делает
        /// </summary>
        /// <param name="s"></param>
        protected void DoStep(float x, float y)
        {
            MapPoint @object = Map.GetObject(x, y);
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

        /// <summary>
        /// король встретил соперника
        /// </summary>
        /// <param name="king"></param>
        protected void FindKing(MapPoint point)
        {
            if (!this.Equals(point))
            {
                if (ContactWithKingEvent != null)
                    ContactWithKingEvent(this, point);
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
        }

        #endregion

        #region Properties

        /// <summary>
        /// Координата
        /// </summary>
        public virtual int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Координата
        /// </summary>
        public virtual int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Предыдущая координата
        /// </summary>
        public virtual int PrevX
        {
            get { return _prevX; }
            set { _prevX = value; }
        }

        /// <summary>
        /// Предыдущая координата
        /// </summary>
        public virtual int PrevY
        {
            get { return _prevY; }
            set { _prevY = value; }
        }

        /// <summary>
        /// Идентификатор картинки
        /// </summary>
        public virtual int ImageId
        {
            get { return _imageId; }
            set { _imageId = value; }
        }

        /// <summary>
        /// Стоимость прохождения
        /// </summary>
        public virtual float WayCost
        {
            get { return _wayCost; }
            set { _wayCost = value; }
        }

        /// <summary>
        /// Тип ячейки
        /// </summary>
        public virtual PointTypes Type
        {
            get { return PointTypes.King; }
        }

        /// <summary>
        /// король обновил состояние
        /// </summary>
        public virtual bool Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }

        /// <summary>
        /// ссылка на ячейку
        /// </summary>
        public MapPoint ViewOnMap
        {
            get { return _viewOnMap; }
            set { _viewOnMap = value; }
        }

        /// <summary>
        /// ссылка на взаимодействие
        /// </summary>
        public virtual IInteraction Interaction
        {
            get { return _interaction; }
            set { _interaction = value; }
        }

        /// <summary>
        /// текущий замок
        /// </summary>
        public virtual Castle CurrentCastle
        {
            get { return _currentCastle; }
        }

        /// <summary>
        /// ссылка на начальный замок
        /// </summary>
        public virtual Castle StartCastle
        {
            get { return _startCastle; }
        }

        /// <summary>
        /// количество шагов в очереди
        /// </summary>
        public virtual int StepCount { get { return _steps.Count; } }

        /// <summary>
        /// дистанция обзора
        /// </summary>
        public virtual int Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        /// <summary>
        /// состояние
        /// </summary>
        public virtual KingState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// индикатор движения
        /// </summary>
        public virtual bool IsMove
        {
            get { return _isMove; }
            set { _isMove = value; }
        }

        /// <summary>
        /// король не активен
        /// </summary>
        public virtual bool Sleep
        {
            get { return _sleep; }
            set { _sleep = value; }
        }

        /// <summary>
        /// ссылка на область видимости
        /// </summary>
        public virtual VisibleSpace VisibleSpace
        {
            get { return _sector; }
            set { _sector = value; }
        }

        /// <summary>
        /// если король император то истина
        /// </summary>
        public virtual bool IsLeader { get { return false; } }

        /// <summary>
        /// ссылка на статистику
        /// </summary>
        public virtual Statistic Statistic { get { return _player.Statistics; } }

        /// <summary>
        /// ссылка на объект для оценивания коэффициентов
        /// </summary>
        public virtual IEvaluator Evaluator
        {
            get { return _evaluator; }
            set
            {
                _evaluator = value;
                _evaluator.AttachOwner(this);
            }
        }

        /// <summary>
        /// ссылка на память
        /// </summary>
        public virtual IMemory Memory { get { return _evaluator.Memory; } }

        /// <summary>
        /// ссылка на союз
        /// </summary>
        public virtual ICommunity Community { get { return _player.Community; } }

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
        /// идентификатор
        /// </summary>
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

        /// <summary>
        /// идентификатор игрока
        /// </summary>
        public virtual int? PlayerId
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

        /// <summary>
        /// идентификатор агента
        /// </summary>
        public virtual int? AnimatId
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

#if !UNITY_EDITOR

        /// <summary>
        /// идентификатор карты
        /// </summary>
        public virtual int? MapId
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
#endif

        /// <summary>
        /// идентификатор империи
        /// </summary>
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
        /// идентификатор союза
        /// </summary>
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
        /// имя
        /// </summary>
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
        /// опыт
        /// </summary>
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
        /// воинское зывние
        /// </summary>
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

#if !UNITY_EDITOR

        /// <summary>
        /// карта
        /// </summary>
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
                    }
                }
            }
        }

        /// <summary>
        /// список шахт
        /// </summary>
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
        /// список фигур
        /// </summary>
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
#else
        public virtual Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        public virtual List<Mine> Mines
        {
            get { return _mines; }
            set { _mines = value; }
        }

        public virtual List<Unit> Units
        {
            get { return _units; }
            set { _units = value; }
        }

        public virtual List<Castle> Castles
        {
            get { return _castles; }
            set { _castles = value; }
        }

        public virtual List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }
#endif
        #endregion

        #region Delegates

        public delegate void FindHandler(MapPoint point); // делегат взаимодействия с объектом карты

        public delegate void ComeInCastleHandler(King player, MapSector point); // событийный делегат. Вход в замок
        public delegate void CollectResourceHandler(King player, MapPoint point); // событийный делегат. Сбор ресурса

        public delegate void CaptureMineHandler(King player, MapSector point); // событийный делегат. Захват шахты
        public delegate void ContactWithCastleHandler(King player, MapSector point); // событийный делегат. Захват замка

        public delegate void ContactWithKingHandler(King player, MapPoint point); // событийный делегат. Встреча с другим королем

        public delegate void UpdateSectorHandler(King king); // событийный делегат. Передвижение короля

        //public delegate void ChangeMapStateHandler(King king, Map map, MapPoint point); // событийный делегат. Изменение ячейки на карте
       
        #endregion

        #region Events

        public event ContactWithKingHandler ContactWithKingEvent;
        public event ContactWithCastleHandler ContactWithCastleEvent;

        public event ComeInCastleHandler ComeInCastleEvent;
        public event CollectResourceHandler CollectResourceEvent;

        public event CaptureMineHandler CaptureMineEvent;

        public event UpdateSectorHandler UpdateSectorEvent;
        public event ChangeMapStateHandler<King> ChangeMapStateEvent;

        public event DeferredTargetedLoadingHandler<King> OnDeferredLoadingMapPoint;

        #endregion

        public int GetUnitCountFAKE()
        {
            return 1;
        }

        public double Power { get; set; }
        public double Wealth { get; set; }
        public double Success { get; set; }

        public double SentHelp { get; set; }
        public double ReceivedHelp { get; set; }
        public int WinNumber { get { return _player.Statistics.WinNumber; } set { _player.Statistics.WinNumber = value; } }
        public int LooseNumber { get { return _player.Statistics.LooseNumber; } set { _player.Statistics.LooseNumber = value; } }
        public int CommonBattlesNumber { get { return _player.Statistics.CommonBattlesNumber; } set { _player.Statistics.CommonBattlesNumber = value; } }
        public double TradingActivityNumber { get; set; }
    }
}
