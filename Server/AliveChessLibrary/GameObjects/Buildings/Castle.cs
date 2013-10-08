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
using System.Data.Linq;

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
        [ProtoMember(8)]
        private bool _kingInside;

        private int _imageId;
        private VisibleSpace _visibleSpace;
        private bool _isAttached = false;

        private MapSector _viewOnMap; // сектор на карте

        private int? _mapId;
        private int _figureStoreId;
        private IProductionManager<InnerBuildingType> _buildingManager;
        private IProductionManager<UnitType> _recruitingManager;

        private Army _army;

        private object _innerBuildingsLock = new object();

        private EntityRef<Map> _map; // ссылка на карту
        private EntityRef<King> _king; // ссылка на короля
        private EntityRef<Vicegerent> _vicegerent; // наместник
        private EntitySet<InnerBuilding> _innerBuildings; // список внутренних строений
        private int _distance = 5;

        #endregion

        #region Constructors

        public Castle()
        {
            _army = new Army();
            this._map = default(EntityRef<Map>);
            this._king = default(EntityRef<King>);
            this._vicegerent = default(EntityRef<Vicegerent>);
            this._innerBuildings = new EntitySet<InnerBuilding>();
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
            Army = new Army();
            _map.Entity = map;
            _mapId = map.Id;
            InnerBuilding quarters = new InnerBuilding();
            quarters.InnerBuildingType = InnerBuildingType.Quarters;
            AddBuilding(quarters);
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

        private InnerBuilding _getBuilding(InnerBuildingType type)
        {
            return _innerBuildings.FirstOrDefault(innerBuilding => innerBuilding.InnerBuildingType == type);
        }

        public void AddBuilding(InnerBuilding building)
        {
            if (!HasBuilding(building.InnerBuildingType))
            {
                lock (_innerBuildingsLock)
                {
                    building.Castle = this;
                    _innerBuildings.Add(building);
                }
            }

        }

        public void DestroyBuilding(InnerBuildingType type)
        {
            lock (_innerBuildingsLock)
            {
                for (int i = 0; i < _innerBuildings.Count; i++)
                {
                    if (_innerBuildings[i].InnerBuildingType == type)
                    {
                        _innerBuildings.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        public bool HasBuilding(InnerBuildingType type)
        {
            lock (_innerBuildingsLock)
            {
                return _getBuilding(type) != null;

            }
        }

        /// <summary>
        /// список внутренних зданий
        /// </summary>
        public List<InnerBuilding> GetInnerBuildingListCopy()
        {
            var result = new List<InnerBuilding>();
            lock (_innerBuildingsLock)
            {
                result.AddRange(_innerBuildings);
            }
            return result;
        }

        public void SetBuildings(List<InnerBuilding> buildings)
        {
            lock (_innerBuildingsLock)
            {
                _innerBuildings.Clear();
                if(buildings == null)
                    return;
                foreach (var innerBuilding in buildings)
                {
                    _innerBuildings.Add(innerBuilding);
                }
            }
        }

        /// <summary>
        /// Создание начальной армии 
        /// </summary>
        public void CreateInitialArmy()
        {
            Army.AddUnit(UnitType.Pawn, 8);
        }

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

        public IProductionManager<InnerBuildingType> BuildingManager
        {
            get { return _buildingManager; }
            set
            {
                _buildingManager = value;
                if (value != null)
                    value.Castle = this;
            }
        }

        public IProductionManager<UnitType> RecruitingManager
        {
            get { return _recruitingManager; }
            set
            {
                _recruitingManager = value;
                if (value != null)
                    value.Castle = this;
            }
        }

        public virtual Army Army
        {
            get { return _army; }
            set { _army = value; }
        }
        #endregion

        public static event LoadingHandler<Castle> OnLoad;
        public event DeferredLoadingHandler<Castle> OnDeferredLoadingFigureStore;
        public event DeferredLoadingHandler<Castle> OnDeferredLoadingResourceStore;
    }
}
