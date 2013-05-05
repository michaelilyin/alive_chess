using System;
using System.Collections.Generic;
using System.Linq;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;
#if !UNITY_EDITOR
using System.Data.Linq;
#endif

namespace AliveChessLibrary.GameObjects.Buildings
{
    /// <summary>
    /// Замок
    /// </summary>
    [ProtoContract]
    public class Castle : IBuilding, IEquatable<int>, IEquatable<MapPoint>, IMultyPoint
    {
        #region Variables

        [ProtoMember(1)]
        private int _castleId;
        [ProtoMember(2)]
        private int _leftX;
        [ProtoMember(3)]
        private int _topY;
        [ProtoMember(4)]
        private int _width;
        [ProtoMember(5)]
        private int _height;
        [ProtoMember(6)]
        private float _wayCost;
        [ProtoMember(7)]
        private int? _kingId;

        private int _imageId;
        private bool _kingInside;
        private VisibleSpace _visibleSpace;
        private bool _isAttached = false;

        private MapSector _viewOnMap; // сектор на карте

        private int? _mapId;
        private int _figureStoreId;
        private IBuildingFactory _buildingFactory;
        private IUnitFactory _unitFactory;

#if !UNITY_EDITOR
        private EntityRef<Map> _map; // ссылка на карту
        private EntityRef<King> _king; // ссылка на короля
        private EntityRef<Vicegerent> _vicegerent; // наместник
        private EntitySet<InnerBuilding> _innerBuildings; // список внутренних строений
        private EntityRef<FigureStore> _figureStore;
#else
        private Map _map; // ссылка на карту
        private King _king; // ссылка на короля
        private Vicegerent _vicegerent; // наместник
        private List<InnerBuilding> _innerBuildings; // список внутренних строений
        private FigureStore _figureStore;
#endif
        private int _distance = 5;

        #endregion

        #region Constructors

        public Castle()
        {
#if !UNITY_EDITOR
            this._map = default(EntityRef<Map>);
            this._king = default(EntityRef<King>);
            this._vicegerent = default(EntityRef<Vicegerent>);
            this._figureStore = default(EntityRef<FigureStore>);
            this._innerBuildings = new EntitySet<InnerBuilding>();
#else
            this.Map = null;
            this.King = null;
            this.Vicegerent = null;
            this.FigureStore = null;
            this.InnerBuildings = new List<InnerBuilding>();
#endif
            _visibleSpace = new VisibleSpace(this);

            if (OnLoad != null)
                OnLoad(this);
        }

        /*public Castle(int id, Map map)
            : this()
        {
            Initialize(id, map);
        }*/

        #endregion

        #region Initialization

        public void Initialize(Map map)
        {
            _map.Entity = map;
            _mapId = map.Id;
            InnerBuilding recruitmentOffice = new InnerBuilding();
            recruitmentOffice.InnerBuildingType = InnerBuildingType.Quarters;
            AddBuilding(recruitmentOffice);
            InnerBuilding building = new InnerBuilding();
            building.InnerBuildingType = InnerBuildingType.Forge;
            AddBuilding(building);
        }

        /*public void Initialize(int id, Map map)
        {
            Initialize(map);
            _castleId = id;
        }

        public void Initialize(Map map, MapSector sector)
        {
            Initialize(map);

            if (sector != null)
                AddView(sector);
        }*/

        /// <summary>
        /// добавление представления на карту
        /// </summary>
        /// <param name="sector"></param>
        public void AddView(MapSector sector)
        {
            ViewOnMap = sector;
            sector.SetOwner(this);
        }

        /// <summary>
        /// удаление с карты сектора
        /// </summary>
        public void RemoveView()
        {
            ViewOnMap.SetOwner(null);
        }

        #endregion

        #region Methods

        /// <summary>
        /// назначаем владельца замка
        /// </summary>
        /// <param name="king"></param>
        public void SetOwner(King king)
        {
            if (_king.Entity != null && king != null)
                throw new AliveChessException("Owner isn't null");
            else
            {
                _king.Entity = king;
                _kingId = king != null ? king.Id : -1;
            }
        }

        /// <summary>
        /// проверка принадлежности замка королю
        /// </summary>
        /// <param name="king"></param>
        /// <returns></returns>
        public bool BelongsTo(King king)
        {
            return this.King == king;
        }

#warning Создание юнита
        //TODO: Переделать по-нормальному
        // Создание юнита и отправка в армию
        public void CreateUnitAndAddInArmy(int count, UnitType type)
        {
            /*for (int i = 0; i < InnerBuildings.Count; i++)
            {
                if (InnerBuildings[i].ProducedUnitType == type)
                {
                    AddInArmy(InnerBuildings[i].CreateUnit(count, type));
                }
            }*/
        }

        //Добавление в армию замка
        //TODO: Переделать по-нормальному
        private void AddInArmy(Unit un)
        {
            bool t = false;
            for (int i = 0; i < FigureStore.Units.Count; i++)
            {
                if (FigureStore.Units[i].UnitType == un.UnitType)
                {
                    FigureStore.Units[i].Quantity += un.Quantity;
                    t = true;
                    break;
                }
            }
            if (!t) FigureStore.Units.Add(un);
        }

        //TODO: Избавиться от этого бреда
        //передать армию замка королю
        public void GetArmyToKing()
        {
            bool t = false;
            foreach (Unit t1 in FigureStore.Units)
            {
                foreach (Unit t2 in King.Units)
                {
                    if (t1.UnitType == t2.UnitType)
                    {
                        t2.Quantity += t1.Quantity;
                        t = true;
                        break;

                    }
                }
                if (!t)
                {
                    King.Units.Add(t1);
                    t = false;
                }
            }
            FigureStore.Units.Clear();
        }

        public InnerBuilding GetBuilding(InnerBuildingType type)
        {
            return InnerBuildings.FirstOrDefault(innerBuilding => innerBuilding.InnerBuildingType == type);
        }

        public void AddBuilding(InnerBuilding building)
        {
            if (!HasBuilding(building.InnerBuildingType))
            {
                building.Castle = this;
                _innerBuildings.Add(building);
            }

        }

        public bool HasBuilding(InnerBuildingType type)
        {
            return GetBuilding(type) != null;
        }

        //TODO: Реализовать
        /// <summary>
        /// Создание начальной армии 
        /// </summary>
        public void CreateInitialArmy()
        {
            //GuidIDPair pair = generator.Invoke();
            //int cost = Convert.ToInt32(CostUnit.One);
            //Unit p = _factory.Create(pair.Guid, pair.Id, 8, UnitType.Pawn);
            //AddUnit(p, _figureStore.Entity.Units);

            //pair = generator.Invoke();
            //cost = Convert.ToInt32(CostUnit.One);
            //p = _factory.Create(pair.Guid, pair.Id, 2, UnitType.Bishop);
            //AddUnit(p, _figureStore.Entity.Units);

            //pair = generator.Invoke();
            //cost = Convert.ToInt32(CostUnit.One);
            //p = _factory.Create(pair.Guid, pair.Id, 2, UnitType.Knight);
            //AddUnit(p, _figureStore.Entity.Units);

            //pair = generator.Invoke();
            //cost = Convert.ToInt32(CostUnit.One);
            //p = _factory.Create(pair.Guid, pair.Id, 1, UnitType.Queen);
            //AddUnit(p, _figureStore.Entity.Units);

            //pair = generator.Invoke();
            //cost = Convert.ToInt32(CostUnit.One);
            //p = _factory.Create(pair.Guid, pair.Id, 2, UnitType.Rook);
            //AddUnit(p, _figureStore.Entity.Units);
        }

        //TODO: Нигде не используется, переписать этот бред
        /// <summary>
        ///добавление юнита 
        /// </summary>
        /// <param name="un"></param>
        /// <param name="arm"></param>
        public void AddUnit(Unit un, IList<Unit> arm)
        {
            bool ok = true;
            for (int i = 0; i < FigureStore.Units.Count; i++)
            {
                if (un.Id == arm[i].Id)
                {
                    arm[i].Quantity++;
                    ok = false;
                    break;
                }

            }
            if (ok) arm.Add(un);
        }
        //Slisarenko

        /// <summary>
        /// сравнение по идентификатору
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(int other)
        {
            return Id.CompareTo(other) == 0 ? true : false;
        }

        /// <summary>
        /// сравнение ячеек
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MapPoint other)
        {
            return Id.CompareTo(other.Owner.Id) == 0 ? true : false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// координала левого верхнего угла по X
        /// </summary>
        public int X
        {
            get { return _leftX; }
            set { _leftX = value; }
        }

        /// <summary>
        /// координала левого верхнего угла по Y
        /// </summary>
        public int Y
        {
            get { return _topY; }
            set { _topY = value; }
        }

        /// <summary>
        /// размер по X
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// размер по Y
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// тип ячейки
        /// </summary>
        public PointTypes Type
        {
            get { return PointTypes.Castle; }
        }

        /// <summary>
        /// идентификатор картинки
        /// </summary>
        public int ImageId
        {
            get { return _imageId; }
            set { _imageId = value; }
        }

        /// <summary>
        /// стоимость прохождения
        /// </summary>
        public float WayCost
        {
            get { return _wayCost; }
            set { _wayCost = value; }
        }

        /// <summary>
        /// проверка замка на свободность
        /// </summary>
        public bool IsFree
        {
            get { return !_kingId.HasValue; }
        }

        /// <summary>
        /// получаем игрока владеющего замком либо null
        /// </summary>
        public IPlayer Player
        {
            get
            {
                return King != null ? King.Player : null;
            }
        }

        /// <summary>
        /// флаг нахождения короля внутри замка
        /// </summary>
        public bool KingInside
        {
            get { return _kingInside; }
            set { _kingInside = value; }
        }

        /// <summary>
        /// дистанция видимости
        /// </summary>
        public int Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        /// <summary>
        /// область видимости
        /// </summary>
        public VisibleSpace VisibleSpace
        {
            get { return _visibleSpace; }
            set { _visibleSpace = value; }
        }

        /// <summary>
        /// тип здания
        /// </summary>
        public BuildingTypes BuildingType
        {
            get { return BuildingTypes.Castle; }
        }

        /// <summary>
        /// сектор на карте
        /// </summary>
        public MapSector ViewOnMap
        {
            get { return _viewOnMap; }
            set { _viewOnMap = value; }
        }

        /// <summary>
        /// замок назначен королю
        /// </summary>
        public bool IsAttached
        {
            get { return _isAttached; }
            set { _isAttached = value; }
        }

#if !UNITY_EDITOR
        /// <summary>
        /// хранилище фигур
        /// </summary>
        public FigureStore FigureStore
        {
            get
            {
                if (_figureStore.Entity == null && OnDeferredLoadingFigureStore != null)
                    OnDeferredLoadingFigureStore(this);

                return this._figureStore.Entity;
            }
            set
            {
                if (_figureStore.Entity != value)
                {
                    _figureStore.Entity = value;
                    _figureStoreId = _figureStore.Entity.Id;
                }
            }
        }

        /// <summary>
        /// ссылка на наместника
        /// </summary>
        public Vicegerent Vicegerent
        {
            get
            {
                return this._vicegerent.Entity;
            }
            set
            {
                if (_vicegerent.Entity != value)
                {
                    if (value != null)
                    {
                        value.Castle = this;
                        _vicegerent.Entity = value;
                    }
                }
            }
        }
#else
        public Vicegerent Vicegerent
        {
            get { return _vicegerent; }
            set { _vicegerent = value; }
        }

        public ResourceStore ResourceStore
        {
            get { return _resourceStore; }
            set { _resourceStore = value; }
        }

        public FigureStore FigureStore
        {
            get { return _figureStore; }
            set { _figureStore = value; }
        }
#endif

        /// <summary>
        /// идентификатор замка
        /// </summary>
        public int Id
        {
            get
            {
                return this._castleId;
            }
            set
            {
                if (this._castleId != value)
                {
                    this._castleId = value;
                }
            }
        }

#if !UNITY_EDITOR

        /// <summary>
        /// идентификатор хранилища фигур
        /// </summary>
        public int FigureStoreId
        {
            get
            {
                return this._figureStoreId;
            }
            set
            {
                if (this._figureStoreId != value)
                {
                    if (this._figureStore.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._figureStoreId = value;
                }
            }
        }

        /// <summary>
        /// идентификатор карты
        /// </summary>
        public int? MapId
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
        /// идентификатор короля
        /// </summary>
        public int? KingId
        {
            get
            {
                return this._kingId;
            }
            set
            {
                if (this._kingId != value)
                {
                    if (this._king.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._kingId = value;
                }
            }
        }
#endif
#if !UNITY_EDITOR

        /// <summary>
        /// владелец замка
        /// </summary>
        public King King
        {
            get
            {
                return this._king.Entity;
            }
            set
            {
                if (_king.Entity != value)
                {
                    if (_king.Entity != null)
                    {
                        var previousKing = _king.Entity;
                        _king.Entity = null;
                        previousKing.Castles.Remove(this);
                    }
                    _king.Entity = value;
                    if (value != null)
                    {
                        _kingId = value.Id;
                        value.Castles.Add(this);
                    }
                    else
                    {
                        this._kingId = null;
                    }
                }
            }
        }

        /// <summary>
        /// карта
        /// </summary>
        public Map Map
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
                        previousMap.Castles.Remove(this);
                    }
                    _map.Entity = value;
                    if (value != null)
                    {
                        _mapId = value.Id;
                    }
                    else
                    {
                        _mapId = null;
                    }
                }
            }
        }

        /// <summary>
        /// список внутренних зданий
        /// </summary>
        public EntitySet<InnerBuilding> InnerBuildings
        {
            get
            {
                return this._innerBuildings;
            }
            set
            {
                this._innerBuildings = value;
            }
        }

        public IBuildingFactory BuildingFactory
        {
            get { return _buildingFactory; }
            set
            {
                _buildingFactory = value;
                if (value != null)
                    value.Castle = this;
            }
        }

        public IUnitFactory UnitFactory
        {
            get { return _unitFactory; }
            set
            {
                _unitFactory = value;
                if (value != null)
                    value.Castle = this;
            }
        }
#else
        public King King
        {
            get { return _king; }
            set { _king = value; }
        }

        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        public List<InnerBuilding> InnerBuildings
        {
            get { return _innerBuildings; }
            set { _innerBuildings = value; }
        }
#endif
        #endregion

        public static event LoadingHandler<Castle> OnLoad;
        public event DeferredLoadingHandler<Castle> OnDeferredLoadingFigureStore;
        public event DeferredLoadingHandler<Castle> OnDeferredLoadingResourceStore;
    }
}
