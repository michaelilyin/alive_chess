using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Objects;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;
#if !UNITY_EDITOR
using System.Data.Linq;
#endif

namespace AliveChessLibrary.GameObjects.Landscapes
{
    /// <summary>
    /// карта
    /// </summary>
    [ProtoContract, DataContract]
    public class Map : ILocalizable
    {
        #region Variables

        [ProtoMember(1), DataMember]
        private int _mapId;
        [ProtoMember(2), DataMember]
        private int _mapSizeX;
        [ProtoMember(3), DataMember]
        private int _mapSizeY;
        [ProtoMember(4), DataMember]
        private string _name;

        private ILevel _level;
        private MapPoint[,] _objects;
      
#if !UNITY_EDITOR
        private EntitySet<King> _kings;
        private EntitySet<Mine> _mines;
        private EntitySet<Castle> _castles;
        private EntitySet<Resource> _resources;
        private EntitySet<SingleObject> _singleObjects;
        private EntitySet<MultyObject> _multyObjects;
        private EntitySet<BasePoint> _basePoints;
        private EntitySet<Border> _borders;
#else
        private List<King> _kings;
        private List<Mine> _mines;
        private List<Castle> _castles;
        private List<Resource> _resources;
        private List<SingleObject> _singleObjects;
        private List<MultyObject> _multyObjects;
        private List<BasePoint> _basePoints;
        private List<Border> _borders;
#endif
        private Dictionary<int, IObserver> _observers;

        private readonly object _kingsSync = new object();
        private readonly object _minesSync = new object();
        private readonly object _resourcesSync = new object();
        private readonly object _castlesSync = new object();
        private readonly object _observerSync = new object();
        private readonly object _singleSync = new object();
        private readonly object _multySync = new object();
        private readonly object _landscapePoint = new object();
        private readonly object _borderSync = new object();
      
        #endregion

        #region Constructors

        public Map()
        {
#if !UNITY_EDITOR
            this._kings = new EntitySet<King>(AttachKing, DetachKing);
            this._mines = new EntitySet<Mine>(AttachMine, DetachMine);
            this._castles = new EntitySet<Castle>(AttachCastle, DetachCastle);
            this._resources = new EntitySet<Resource>(AttachResource, DetachResource);
            this._singleObjects = new EntitySet<SingleObject>(AttachSingleObject, DetachSingleObject);
            this._multyObjects = new EntitySet<MultyObject>(AttachMultyObject, DetachMultyObject);
            this._basePoints = new EntitySet<BasePoint>(AttachLandscapePoint, DetachLandscapePoint);
            this._borders = new EntitySet<Border>(AttachBorder, DetachBorder);
#else
            this.Kings = new List<King>();
            this.Mines = new List<Mine>();
            this.Castles = new List<Castle>();
            this.Resources = new List<Resource>();
            this.SingleObjects = new List<SingleObject>();
            this.MultyObjects = new List<MultyObject>();
            this.BasePoints = new List<BasePoint>();
            this.Borders = new List<Border>();
#endif
            this._observers = new Dictionary<int, IObserver>();
        }

        public Map(int sizeX, int sizeY)
            :this()
        {
            Initialize(sizeX, sizeY);
        }

        #endregion

        #region Methods

        /// <summary>
        /// инициализация
        /// </summary>
        public void Initialize()
        {
            _objects = new MapPoint[SizeX, SizeY];
        }

        /// <summary>
        /// инициализация
        /// </summary>
        public void Initialize(int sizeX, int sizeY)
        {
            _mapSizeX = sizeX;
            _mapSizeY = sizeY;
            _objects = new MapPoint[SizeX, SizeY];
        }

        /// <summary>
        /// заполнение карты объектами
        /// </summary>
        public void Fill()
        {
            FloodAlgorithm algorithm = new FloodAlgorithm(this);
            foreach (var basePoint in _basePoints)
                algorithm.Run(basePoint);
        }

        /// <summary>
        /// проверка ячейки на принадлежность
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void TestLocate(int x, int y)
        {
            if (!Locate(x, y)) throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// создать ячейку
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MapPoint CreatePoint(Position position, PointTypes type)
        {
            MapPoint point = new MapPoint();
            point.X = position.X;
            point.Y = position.Y;
            point.PointType = type;
            point.MapSector = null;
            return point;
        }

        /// <summary>
        /// создание ячейки
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MapPoint CreatePoint(int x, int y, PointTypes type)
        {
            MapPoint point = new MapPoint();
            point.X = x;
            point.Y = y;
            point.PointType = type;
            point.MapSector = null;
            return point;
        }

        /// <summary>
        /// создать ячейку
        /// </summary>
        /// <param name="position"></param>
        /// <param name="sector"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MapPoint CreatePoint(Position position, MapSector sector, PointTypes type)
        {
            MapPoint point = new MapPoint();
            point.X = position.X;
            point.Y = position.Y;
            point.PointType = type;
            point.MapSector = sector;
            return point;
        }

        /// <summary>
        /// создать ячейку
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="sector"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MapPoint CreatePoint(int x, int y, MapSector sector, PointTypes type)
        {
            MapPoint point = new MapPoint();
            point.X = x;
            point.Y = y;
            point.PointType = type;
            point.MapSector = sector;
            return point;
        }

        /// <summary>
        /// создание сектора
        /// </summary>
        /// <param name="leftTopCorner"></param>
        /// <param name="size"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MapSector CreateSector(Position leftTopCorner, Position size, PointTypes type)
        {
            MapSector sector = new MapSector();
            sector.X = leftTopCorner.X;
            sector.Y = leftTopCorner.Y;
            sector.Width = size.X;
            sector.Height = size.Y;
            sector.MapPointType = type;
            return sector;
        }

        /// <summary>
        /// создать сектор
        /// </summary>
        /// <param name="leftX"></param>
        /// <param name="topY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MapSector CreateSector(int leftX, int topY, int width, int height, PointTypes type)
        {
            MapSector sector = new MapSector();
            sector.X = leftX;
            sector.Y = topY;
            sector.Width = width;
            sector.Height = height;
            sector.MapPointType = type;
            return sector;
        }

        /// <summary>
        /// инициализировать ячейки сектора
        /// </summary>
        /// <param name="sector"></param>
        private void InitializeSector(MapSector sector)
        {
            for (int i = sector.X; i < sector.X + sector.Width; i++)
            {
                for (int j = sector.Y; j < sector.Y + sector.Height; j++)
                {
                    MapPoint point = CreatePoint(i, j, sector, sector.MapPointType);
                    SetObject(point);
                }
            }
        }

        /// <summary>
        /// обновить состояние
        /// </summary>
        /// <param name="time"></param>
        public void Update(DateTime time)
        {
            lock (_kingsSync)
            {
                for (int i = 0; i < _kings.Count; i++)
                    _kings[i].Update();
            }
            lock (_minesSync)
            {
                for (int i = 0; i < _mines.Count; i++)
                    _mines[i].DoWork(time);
            }
        }

        #region Searchers

        /// <summary>
        /// искать короля по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public King SearchKingById(int id)
        {
            lock (_kingsSync)
                return Kings.Search(x => x.Id == id);
        }

        /// <summary>
        /// искать шахту по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Mine SearchMineById(int id)
        {
            lock (_minesSync)
                return Mines.Search(x => x.Id == id);
        }

        /// <summary>
        /// искать замок по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Castle SearchCastleById(int id)
        {
            lock (_castlesSync)
                return Castles.Search(x => x.Id == id);
        }

        /// <summary>
        /// искать ресурс по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Resource SearchResourceById(int id)
        {
            lock (_resourcesSync)
                return Resources.Search(x => x.Id == id);
        }

        /// <summary>
        /// поиск объекта, обладающего областью видимости
        /// </summary>
        /// <param name="observerId"></param>
        /// <param name="observerType"></param>
        /// <returns></returns>
        public IObserver SearchObserverById(int observerId, PointTypes observerType)
        {
            Debug.Assert(observerType != PointTypes.None);
            switch (observerType)
            {
                case PointTypes.King:
                    return Kings.Search(x => x.Id == observerId);
                case PointTypes.Mine:
                    return Mines.Search(x => x.Id == observerId);
                case PointTypes.Castle:
                    return Castles.Search(x => x.Id == observerId);
                default:
                    throw new AliveChessException("Observer not found");
            }
        }

        #endregion

        #region Add And Remove

        /// <summary>
        /// добавить шахту
        /// </summary>
        /// <param name="mine"></param>
        public void AddMine(Mine mine)
        {
            lock (_minesSync)
                Mines.Add(mine);

            MapSector mapSector = CreateSector(
                mine.X, mine.Y,
                mine.Width, mine.Height,
                PointTypes.Mine);

            InitializeSector(mapSector);

            mine.AddView(mapSector);
            mapSector.WayCost = mine.WayCost;

            lock (_minesSync)
            {
                if (!_observers.ContainsKey(mine.Id))
                {
                    _observers.Add(mine.Id, mine);
                }
            }
        }

        /// <summary>
        /// добавить замок
        /// </summary>
        /// <param name="castle"></param>
        public void AddCastle(Castle castle)
        {
            lock (_castlesSync)
                Castles.Add(castle);

            MapSector mapSector = CreateSector(
                castle.X, castle.Y,
                castle.Width, castle.Height,
                PointTypes.Castle);

            InitializeSector(mapSector);

            castle.AddView(mapSector);
            mapSector.WayCost = castle.WayCost;

            lock (_observerSync)
            {
                if (!_observers.ContainsKey(castle.Id))
                {
                    _observers.Add(castle.Id, castle);
                }
            }
        }

        /// <summary>
        /// добавить короля
        /// </summary>
        /// <param name="king"></param>
        public void AddKing(King king)
        {
            lock (_kingsSync)
                Kings.Add(king);

            MapPoint mapPoint = CreatePoint(
                king.X, king.Y,
                PointTypes.King);

            SetObject(mapPoint);

            king.AddView(mapPoint);

            mapPoint.WayCost = king.WayCost;

            lock (_observerSync)
            {
                if (!_observers.ContainsKey(king.Id))
                {
                    _observers.Add(king.Id, king);
                }
            }
        }

        /// <summary>
        /// добавить ресурс
        /// </summary>
        /// <param name="resource"></param>
        public void AddResource(Resource resource)
        {
            lock (_resourcesSync)
                Resources.Add(resource);

            MapPoint mapPoint = CreatePoint(
                resource.X, resource.Y,
                PointTypes.Resource);

            SetObject(mapPoint);

            resource.AddView(mapPoint);

            mapPoint.WayCost = resource.WayCost;
        }

        /// <summary>
        /// добавить одиночный объект
        /// </summary>
        /// <param name="singleObject"></param>
        public void AddSingleObject(SingleObject singleObject)
        {
            lock (_singleSync)
                SingleObjects.Add(singleObject);

            MapPoint mapPoint = CreatePoint(
                singleObject.X, singleObject.Y,
                PointTypes.SingleObject);

            SetObject(mapPoint);

            singleObject.AddView(mapPoint);

            mapPoint.WayCost = singleObject.WayCost;
        }

        /// <summary>
        /// добавить крупный объект
        /// </summary>
        /// <param name="multyObject"></param>
        public void AddMultyObject(MultyObject multyObject)
        {
            lock (_multySync)
                MultyObjects.Add(multyObject);

            MapSector mapSector = CreateSector(
                multyObject.X, multyObject.Y,
                multyObject.Width, multyObject.Height,
                PointTypes.MultyObject);

            InitializeSector(mapSector);

            multyObject.AddView(mapSector);
            mapSector.WayCost = multyObject.WayCost;
        }

        /// <summary>
        /// добавить индикатор местности
        /// </summary>
        /// <param name="basePoint"></param>
        public void AddBasePoint(BasePoint basePoint)
        {
            lock (_landscapePoint)
                BasePoints.Add(basePoint);

            MapPoint mapPoint = CreatePoint(
                basePoint.X, basePoint.Y,
                PointTypes.Landscape);

            SetObject(mapPoint);

            basePoint.AddView(mapPoint);

            mapPoint.WayCost = basePoint.WayCost;
        }

        /// <summary>
        /// добавить границу типа местности
        /// </summary>
        /// <param name="border"></param>
        public void AddBorder(Border border)
        {
            lock (_borderSync)
                Borders.Add(border);

            MapPoint mapPoint = CreatePoint(
                border.X, border.Y,
                PointTypes.Landscape);

            SetObject(mapPoint);

            border.AddView(mapPoint);

            mapPoint.WayCost = border.WayCost;
        }

        /// <summary>
        /// удалить короля
        /// </summary>
        /// <param name="king"></param>
        public void RemoveKing(King king)
        {
            lock (_kingsSync)
                Kings.Remove(king);

            lock (_observerSync)
                _observers.Remove(king.Id);

            king.RemoveView();
        }

        /// <summary>
        /// удалить замок
        /// </summary>
        /// <param name="castle"></param>
        public void RemoveCastle(Castle castle)
        {
            lock (_castlesSync)
                Castles.Remove(castle);

            lock (_observerSync)
                _observers.Remove(castle.Id);

            castle.RemoveView();
        }

        /// <summary>
        /// удалить шахту
        /// </summary>
        /// <param name="mine"></param>
        public void RemoveMine(Mine mine)
        {
            lock (_minesSync)
                Mines.Remove(mine);

            lock (_observerSync)
                _observers.Remove(mine.Id);

            mine.RemoveView();
        }

        /// <summary>
        /// удалить ресурс
        /// </summary>
        /// <param name="resource"></param>
        public void RemoveResource(Resource resource)
        {
            lock (_resourcesSync)
                Resources.Remove(resource);

            resource.RemoveView();
        }

        /// <summary>
        /// удалить одиночный объект
        /// </summary>
        /// <param name="single"></param>
        public void RemoveSingleObject(SingleObject single)
        {
            lock (_singleSync)
                SingleObjects.Remove(single);

            single.RemoveView();
        }

        /// <summary>
        /// удалить крупный объект
        /// </summary>
        /// <param name="multy"></param>
        public void RemoveMultyObject(MultyObject multy)
        {
            lock (_multySync)
                MultyObjects.Remove(multy);

            multy.RemoveView();
        }

        /// <summary>
        /// удалить индикатор местности
        /// </summary>
        /// <param name="point"></param>
        public void RemoveBasePoint(BasePoint point)
        {
            lock (_landscapePoint)
                BasePoints.Remove(point);

            point.RemoveView();
        }

        /// <summary>
        /// удалить границу местности
        /// </summary>
        /// <param name="point"></param>
        public void RemoveBorder(Border point)
        {
            lock (_borderSync)
                Borders.Remove(point);

            point.RemoveView();
        }

        #endregion

        #region Check Existance

        /// <summary>
        /// проверить существование шахты
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsMine(long id)
        {
            lock (_minesSync)
            {
                foreach (Mine m in Mines)
                    if (m.Equals(id)) return true;
            }
            return false;
        }

        /// <summary>
        /// проверить существование короля
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsKing(long id)
        {
            lock (_kingsSync)
            {
                for (int i = 0; i < Kings.Count; i++)
                    if (Kings[i].Equals(id)) return true;
            }
            return false;
        }

        /// <summary>
        /// проверить существование замка
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsCastle(long id)
        {
            lock (_castlesSync)
            {
                foreach (Castle c in Castles)
                    if (c.Equals(id)) return true;
            }
            return false;
        }

        /// <summary>
        /// проверить существование ресурса
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsResource(long id)
        {
            lock (_resourcesSync)
            {
                foreach (Resource r in Resources)
                    if (r.Equals(id)) return true;
            }
            return false;
        }

        #endregion

        #region Enumerators

        public IEnumerable NextKing()
        {
            lock (_kingsSync)
            {
                for (int i = 0; i < Kings.Count; i++)
                    yield return Kings[i];
            }
        }

        public IEnumerable NextCastle()
        {
            lock (_castlesSync)
            {
                for (int i = 0; i < Castles.Count; i++)
                    yield return Castles[i];
            }
        }

        public IEnumerable NextMine()
        {
            lock (_minesSync)
            {
                for (int i = 0; i < Mines.Count; i++)
                    yield return Mines[i];
            }
        }

        public IEnumerable NextBorder()
        {
            lock (_borderSync)
            {
                for (int i = 0; i < Borders.Count; i++)
                    yield return Borders[i];
            }
        }

        #endregion

        /// <summary>
        /// поиск пустого замка
        /// </summary>
        /// <returns></returns>
        public Castle SearchFreeCastle()
        {
            return Castles.Search(x => x.IsFree && !x.IsAttached);
        }

        /// <summary>
        /// установить тип ячейки
        /// </summary>
        /// <param name="object"></param>
        public void SetObject(MapPoint @object)
        {
            _objects[@object.X, @object.Y] = @object;
        }

        /// <summary>
        /// получить тип ячейки
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public MapPoint GetObject(int x, int y)
        {
            if (this.Locate(x, y))
                return _objects[x, y];
            else throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// получить тип ячейки
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public MapPoint GetObject(float x, float y)
        {
            return GetObject((int) x, (int) y);
        }

        /// <summary>
        /// получаем стоимость прохождения через ячейку
        /// </summary>
        /// <param name="x">координата x</param>
        /// <param name="y">координата y</param>
        /// <returns></returns>
        public float GetWayCost(int x, int y)
        {
            return _objects[x, y].WayCost;
        }

        /// <summary>
        /// проверка принадлежности ячейки карте
        /// </summary>
        /// <param name="x">координата x</param>
        /// <param name="y">координата y</param>
        /// <returns></returns>
        public bool Locate(int x, int y)
        {
            return x > -1 && x < this._mapSizeX - 1 && y > -1 && y < this._mapSizeY - 1;
        }

        /// <summary>
        /// проверить проходимость ячейки
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="costLimit"></param>
        /// <returns></returns>
        public bool CheckPoint(int x, int y, int costLimit)
        {
            return Locate(x, y) && _objects[x, y].WayCost <= costLimit;
        }

        #region Attach handlers

        private void AttachMine(Mine entity)
        {
            entity.Map = this;
        }

        private void DetachMine(Mine entity)
        {
            entity.Map = null;
        }

        private void AttachCastle(Castle entity)
        {
            entity.Map = this;
        }

        private void DetachCastle(Castle entity)
        {
            entity.Map = null;
        }

        private void AttachKing(King entity)
        {
            entity.Map = this;
        }

        private void DetachKing(King entity)
        {
            entity.Map = null;
        }

        private void AttachResource(Resource entity)
        {
            entity.Map = this;
        }

        private void DetachResource(Resource entity)
        {
            entity.Map = null;
        }

        private void AttachSingleObject(SingleObject entity)
        {
            entity.Map = this;
        }

        private void DetachSingleObject(SingleObject entity)
        {
            entity.Map = null;
        }

        private void AttachMultyObject(MultyObject entity)
        {
            entity.Map = this;
        }

        private void DetachMultyObject(MultyObject entity)
        {
            entity.Map = null;
        }

        private void AttachLandscapePoint(BasePoint entity)
        {
            entity.Map = this;
        }

        private void DetachLandscapePoint(BasePoint entity)
        {
            entity.Map = null;
        }

        private void AttachBorder(Border entity)
        {
            entity.Map = this;
        }

        private void DetachBorder(Border entity)
        {
            entity.Map = null;
        }

        #endregion

        #endregion

        #region Properties

        public ILevel Level
        {
            get { return _level; }
            set
            {
                if (_level != value)
                {
                    _level = value;
                    _level.Map = this;
                }
            }
        }

        public MapPoint this[int x, int y]
        {
            get
            {
                return _objects[x, y];
            }
            set
            {
                _objects[x, y] = value;
            }
        }

        public MapPoint this[Position position]
        {
            get
            {
                return _objects[position.X, position.Y];
            }
            set
            {
                _objects[position.X, position.Y] = value;
            }
        }

        public int Id
        {
            get
            {
                return this._mapId;
            }
            set
            {
                if (this._mapId != value)
                {
                    this._mapId = value;
                }
            }
        }

        public int SizeX
        {
            get
            {
                return this._mapSizeX;
            }
            set
            {
                if (this._mapSizeX != value)
                {
                    this._mapSizeX = value;
                }
            }
        }

        public int SizeY
        {
            get
            {
                return this._mapSizeY;
            }
            set
            {
                if (this._mapSizeY != value)
                {
                    this._mapSizeY = value;
                }
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                }
            }
        }

#if !UNITY_EDITOR
        
        public EntitySet<Castle> Castles
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

        public EntitySet<King> Kings
        {
            get
            {
                return this._kings;
            }
            set
            {
                this._kings.Assign(value);
            }
        }

        public EntitySet<Mine> Mines
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

        public EntitySet<Resource> Resources
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

        public EntitySet<SingleObject> SingleObjects
        {
            get
            {
                return this._singleObjects;
            }
            set
            {
                this._singleObjects.Assign(value);
            }
        }

        public EntitySet<MultyObject> MultyObjects
        {
            get
            {
                return this._multyObjects;
            }
            set
            {
                this._multyObjects.Assign(value);
            }
        }

        public EntitySet<BasePoint> BasePoints
        {
            get
            {
                return this._basePoints;
            }
            set
            {
                this._basePoints.Assign(value);
            }
        }

        public void set_BasePoints(List<BasePoint> list)
        {
            EntitySet<BasePoint> es = new EntitySet<BasePoint>();
            foreach (BasePoint bp in list)
            {
                es.Add(bp);
            }
            this._basePoints.Assign(es);
        }

        //public List<BasePoint> BasePoints
        //{
        //    get
        //    {
        //        List<BasePoint> res = new List<BasePoint>();
        //        foreach (BasePoint bp in this._basePoints)
        //        {
        //            res.Add(bp);
        //        }
        //        return res;
        //    }
        //}

        public EntitySet<Border> Borders
        {
            get
            {
                return this._borders;
            }
            set
            {
                this._borders.Assign(value);
            }
        }
#else
        public List<King> Kings
        {
            get { return _kings; }
            set { _kings = value; }
        }

        public List<Mine> Mines
        {
            get { return _mines; }
            set { _mines = value; }
        }

        public List<Castle> Castles
        {
            get { return _castles; }
            set { _castles = value; }
        }

        public List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        public List<SingleObject> SingleObjects
        {
            get { return _singleObjects; }
            set { _singleObjects = value; }
        }

        public List<MultyObject> MultyObjects
        {
            get { return _multyObjects; }
            set { _multyObjects = value; }
        }

        public List<BasePoint> BasePoints
        {
            get { return _basePoints; }
            set { _basePoints = value; }
        }

        public List<Border> Borders
        {
            get { return _borders; }
            set { _borders = value; }
        }
#endif
        #endregion
    }
}