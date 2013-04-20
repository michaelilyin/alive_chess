using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Buildings
{
    /// <summary>
    /// замок
    /// </summary>
    [ProtoContract]
    [Table(Name = "dbo.castle")]
    public class Castle : IBuilding, IEquatable<int>, IMultyPoint
    {
        #region Variables

        [ProtoMember(1)]
        private int _castleId;

        private bool _kingInside;
        private VisibleSpace _visibleSector;

        [ProtoMember(2)]
        private MapSector _viewOnMap; // сектор на карте

        private int _mapId;
        private int? _kingId;
        private int _mapSectorId;
        private int _resourceVaultId;
        private int _figureVaultId;

        private EntityRef<Map> _map; // ссылка на карту
        private EntityRef<King> _king; // ссылка на короля
        private EntityRef<Vicegerent> _vicegerent; // наместник
        private EntitySet<InnerBuilding> _innerBuildings; // список внутренних строений
        private EntityRef<ResourceStore> _resourceStore;
        private EntityRef<FigureStore> _figureStore;

        private int _distance = 5;
        private GameData _gameData;
        
        ///Slisarenko
        //фабрика юнитов
        private UnitFacrory _factory;
        //для призыва пешек
        private InnerBuilding _recruitmentOffice;
        //фабрика постройки зданий
        private InnerBuildingFactory Fabric = new InnerBuildingFactory();
        //Армия в замке
        //private List<Unit> _army = new List<Unit>();

        //список ресурсов для постройки  
        //private List<IInnerBuilding> list = new List<IInnerBuilding>();

        ///Slisarenko

        #endregion

        private UpdateCheck updateCheck;
        public UpdateCheck UpdateCheck
        {
            get { return updateCheck; }
            set { updateCheck = value; }
        }

        #region Constructors

        public Castle()
        {
            this._map = default(EntityRef<Map>);
            this._king = default(EntityRef<King>);
            this._vicegerent = default(EntityRef<Vicegerent>);
            this._figureStore = default(EntityRef<FigureStore>);
            this._resourceStore = default(EntityRef<ResourceStore>);
            this._innerBuildings = new EntitySet<InnerBuilding>(AttachInnerBuilding, DetachInnerBuilding);
        }

        public Castle(Guid id, Map map, GameData context)
            : this()
        {
            Initialize(id, map, context);
        }

        #endregion

        #region Initialization
       
        public void Initialize(Map map, GameData data)
        {
            this.Map = map;
            this._gameData = data;
            this._factory = new UnitFacrory(data);
        }

        public void Initialize(Guid id, Map map, GameData context)
        {
            this.Map = map;
            //this._castleId = id;
            this._gameData = context;
            this._factory = new UnitFacrory(context);
        }

        public void Initialize(Guid id, Map map, GameData context, UniqueIdHandler generator)
        {
            Initialize(id, map, context);

            _recruitmentOffice = new InnerBuilding();
            GuidIDPair pair = generator.Invoke();
            //_recruitmentOffice.Id = pair.Id;
           // _recruitmentOffice.Id = pair.Guid;
            _recruitmentOffice.ProducedUnitType = UnitType.Pawn;
            _recruitmentOffice.InnerBuildingType = InnerBuildingType.Voencomat;
            _recruitmentOffice.Name = "Recruitment office";
            _innerBuildings.Add(_recruitmentOffice);
        }

        /// <summary>
        /// добавление представления на карту
        /// </summary>
        /// <param name="sector"></param>
        public void AddView(MapSector sector)
        {
            this.ViewOnMap = sector;
            foreach (MapPoint mp in sector.MapPoints)
                _map.Entity.SetObject(mp);
        }

        [ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.MapSectorId = this.ViewOnMap.Id;
        }

        #endregion

        #region Methods

        private void InitTest()
        {
            if (_viewOnMap == null)
                throw new AliveChessException("Object is not initialized");
        }

        public bool IsBelongTo(King king)
        {
            return this.King == king;
        }

        ///Slisarenko
        //Выдать список

        public int SizeListbuilldingsInCastle()
        {
            return _innerBuildings.Count;
        }

       // Создание юнита и отправка в армию
        public void CreateUnitAndAddInArmy(UniqueIdHandler generator, int count, UnitType type)
        {
            for (int i = 0; i < _innerBuildings.Count; i++)
            {
                if (_innerBuildings[i].ProducedUnitType == type)
                {
                    AddInArmy(_innerBuildings[i].CreateUnit(generator, count, type));
                }
            }
        }

        //Добавление в армию замка
        private void AddInArmy(Unit un)
        {
            bool t = false;
            for (int i = 0; i < _figureStore.Entity.Units.Count; i++)
            {
                if (_figureStore.Entity.Units[i].UnitType == un.UnitType)
                {
                    _figureStore.Entity.Units[i].UnitCount += un.UnitCount;
                    t = true;
                    break;
                }
            }
            if (!t) _figureStore.Entity.Units.Add(un);
        }

        public bool test_res(int[] f)
        {
            if (f[0] > _vicegerent.Entity.Castle.ResourceStore.GetResourceCountInRepository(Resources.ResourceTypes.Coal))
                return false;
            if (f[1] > _vicegerent.Entity.Castle.ResourceStore.GetResourceCountInRepository(Resources.ResourceTypes.Gold))
                return false;
            if (f[2] > _vicegerent.Entity.Castle.ResourceStore.GetResourceCountInRepository(Resources.ResourceTypes.Iron))
                return false;
            if (f[3] > _vicegerent.Entity.Castle.ResourceStore.GetResourceCountInRepository(Resources.ResourceTypes.Stone))
                return false;
            if (f[4] > _vicegerent.Entity.Castle.ResourceStore.GetResourceCountInRepository(Resources.ResourceTypes.Wood))
                return false;
            return true;
        }

        //передать армию замка королю
        public void GetArmyToKing()
        {
            bool t = false;
            for (int j = 0; j < _figureStore.Entity.Units.Count; j++)
            {
                for (int i = 0; i < _king.Entity.Units.Count; i++)
                {
                    if (_figureStore.Entity.Units[j].UnitType == _king.Entity.Units[i].UnitType)
                    {
                        _king.Entity.Units[i].UnitCount += _figureStore.Entity.Units[j].UnitCount;
                        t = true;
                        break;

                    }
                }
                if (!t)
                {
                    _king.Entity.Units.Add(_figureStore.Entity.Units[j]);
                    t = false;
                }
            }
            _figureStore.Entity.Units.Clear();
        }
        ///Slisarenko

        ///Slisarenko
      
        public InnerBuilding GetBuildings(int i)
        {
            return _innerBuildings[i];
        }

        public void AddBuildings(UniqueIdHandler generator, InnerBuildingType type)
        {
            GuidIDPair pair = generator.Invoke();
            //_innerBuildings.Add(Fabric.Build(pair.Guid, pair.Id, type, type.ToString(), _gameData));
        }

        public List<Unit> ArmyInsideCastle
        {
            get { return _figureStore.Entity.Units.ToList(); }
        }

        public InnerBuilding RecruitmentOffice
        {
            get { return _recruitmentOffice; }
        }

        /// <summary>
        /// Создание начальной армии 
        /// </summary>
        public void CreatStartArmy(UniqueIdHandler generator)
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

        /// <summary>
        ///добавление юнита 
        /// </summary>
        /// <param name="un"></param>
        /// <param name="arm"></param>
        public void AddUnit(Unit un, EntitySet<Unit> arm)
        {
            bool ok = true;
            for (int i = 0; i < _figureStore.Entity.Units.Count; i++)
            {
                if (un.Id == arm[i].Id)
                {
                    arm[i].UnitCount++;
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
        /// обновляем область видимости
        /// </summary>
        /// <param name="sector"></param>
        public void UpdateVisibleSpace(VisibleSpace sector)
        {
            this._visibleSector = sector;
        }

        #region Attach handlers

        /// <summary>
        /// добавляем к замку внутреннее строение
        /// </summary>
        /// <param name="entity"></param>
        private void AttachInnerBuilding(InnerBuilding entity)
        {
            entity.Castle = this;
        }

        /// <summary>
        /// удаляем внутреннее строение
        /// </summary>
        /// <param name="entity"></param>
        private void DetachInnerBuilding(InnerBuilding entity)
        {
            entity.Castle = null;
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// центр замка по X
        /// </summary>
        public int X
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

        /// <summary>
        /// центр замка по Y
        /// </summary>
        public int Y
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

        /// <summary>
        /// проверка замка на свободность
        /// </summary>
        public bool IsFree
        {
            get { return _kingId == null; }
        }

        /// <summary>
        /// получаем игрока владеющего замком либо null
        /// </summary>
        public IPlayer Player
        {
            get 
            {
                return _king.Entity != null ? _king.Entity.Player : null;                   
            }
        }

        public bool KingInside
        {
            get { return _kingInside; }
            set { _kingInside = value; }
        }

        public int Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        public GameData GameData
        {
            get { return _gameData; }
            set { _gameData = value; }
        }

        public VisibleSpace VisibleSpace
        {
            get { return _visibleSector; }
            set { _visibleSector = value; }
        }

        public BuildingTypes BuildingType
        {
            get { return BuildingTypes.Castle; }
        }

        /// <summary>
        /// сектор на карте
        /// </summary>
        public MapSector ViewOnMap
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
                        _mapSectorId = _viewOnMap.Id;
                    }
                }
            }
        }

        public FigureStore FigureStore
        {
            get
            {
                return this._figureStore.Entity;
            }
            set
            {
                if (_figureStore.Entity != value)
                {
                    _figureStore.Entity = value;
                    _figureVaultId = _figureStore.Entity.Id;
                }
            }
        }

        public ResourceStore ResourceStore
        {
            get
            {
                return this._resourceStore.Entity;
            }
            set
            {
                if (_resourceStore.Entity != value)
                {
                    _resourceStore.Entity = value;
                    _resourceVaultId = _resourceStore.Entity.Id;
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

        /// <summary>
        /// идентификатор замка (первичный ключ)
        /// </summary>
        [Column(Name = "castle_id", Storage = "_castleId", CanBeNull = false, DbType = Constants.DB_INT,
            IsPrimaryKey = true, IsDbGenerated = true)]
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

        [Column(Name = "map_sector_id", Storage = "_mapSectorId", CanBeNull = false, DbType = Constants.DB_INT)]
        public int MapSectorId
        {
            get
            {
                return this._mapSectorId;
            }
            set
            {
                if (this._mapSectorId != value)
                {
                    this._mapSectorId = value;
                }
            }
        }

        [Column(Name = "resource_vault_id", Storage = "_resourceVaultId", CanBeNull = false, DbType = Constants.DB_INT)]
        public int ResourceVaultId
        {
            get
            {
                return this._resourceVaultId;
            }
            set
            {
                if (this._resourceVaultId != value)
                {
                    if (this._resourceStore.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._resourceVaultId = value;
                }
            }
        }

        [Column(Name = "figure_vault_id", Storage = "_figureVaultId", CanBeNull = false, DbType = Constants.DB_INT)]
        public int FigureVaultId
        {
            get
            {
                return this._figureVaultId;
            }
            set
            {
                if (this._figureVaultId != value)
                {
                    if (this._figureStore.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._figureVaultId = value;
                }
            }
        }

        /// <summary>
        /// идентификатор карты (внешний ключ)
        /// </summary>
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

        /// <summary>
        /// идентификатор короля (внешний ключ)
        /// </summary>
        [Column(Name = "king_id", Storage = "_kingId", CanBeNull = true, DbType = Constants.DB_INT)]
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
        /// ссылка на короля
        /// </summary>
        [Association(Name = "fk_castle_king", Storage = "_king", ThisKey = "KingId", IsForeignKey = true)]
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
        /// ссылка на карту
        /// </summary>
        [Association(Name = "fk_castle_map", Storage = "_map", ThisKey = "MapId", IsForeignKey = true)]
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
                        _visibleSector = new VisibleSpace(value);
                    }
                    else
                    {
                        _mapId = -1;
                    }
                }
            }
        }

        /// <summary>
        /// список внутренних зданий
        /// </summary>
        [Association(Name = "fk_inner_building_castle", Storage = "_innerBuildings", OtherKey = "CastleId")]
        public EntitySet<InnerBuilding> InnerBuildings
        {
            get
            {
                return this._innerBuildings;
            }
            set
            {
                this._innerBuildings.Assign(value);
            }
        }

        #endregion
    }
}
