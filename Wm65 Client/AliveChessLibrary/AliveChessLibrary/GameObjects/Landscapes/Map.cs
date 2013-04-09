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

namespace AliveChessLibrary.GameObjects.Landscapes
{
    /// <summary>
    /// карта
    /// </summary>
    [ProtoContract]
    public class Map : ILocalizable
    {
        #region Variables

        [ProtoMember(1)]
        private int _mapId;
        [ProtoMember(2)]
        private int _mapSizeX;
        [ProtoMember(3)]
        private int _mapSizeY;
        [ProtoMember(4)]
        private string _name;

        private ILevel _level;
        private MapPoint[,] _objects;
      
        private List<King> _kings;
        private List<Mine> _mines;
        private List<Castle> _castles;
        private List<Resource> _resources;
        private List<SingleObject> _singleObjects;
        private List<MultyObject> _multyObjects;
        private List<BasePoint> _basePoints;
        private List<Border> _borders;

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
            this.Kings = new List<King>();
            this.Mines = new List<Mine>();
            this.Castles = new List<Castle>();
            this.Resources = new List<Resource>();
            this.SingleObjects = new List<SingleObject>();
            this.MultyObjects = new List<MultyObject>();
            this.BasePoints = new List<BasePoint>();
            this.Borders = new List<Border>();
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
            this._objects = new MapPoint[this.SizeX, this.SizeY];
        }

        /// <summary>
        /// инициализация
        /// </summary>
        public void Initialize(int sizeX, int sizeY)
        {
            this._mapSizeX = sizeX;
            this._mapSizeY = sizeY;
            this._objects = new MapPoint[this.SizeX, this.SizeY];
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

        public static MapPoint CreatePoint(Position position, PointTypes type)
        {
            MapPoint point = new MapPoint();
            point.X = position.X;
            point.Y = position.Y;
            point.MapPointType = type;
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
            point.MapPointType = type;
            point.MapSector = null;
            return point;
        }

        public static MapPoint CreatePoint(Position position, MapSector sector, PointTypes type)
        {
            MapPoint point = new MapPoint();
            point.X = position.X;
            point.Y = position.Y;
            point.MapPointType = type;
            point.MapSector = sector;
            return point;
        }

        public static MapPoint CreatePoint(int x, int y, MapSector sector, PointTypes type)
        {
            MapPoint point = new MapPoint();
            point.X = x;
            point.Y = y;
            point.MapPointType = type;
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

        public void InitializeSector(MapSector sector)
        {
            for (int i = sector.X; i < sector.X + sector.Width; i++)
            {
                for (int j = sector.Y; j < sector.Y + sector.Height; j++)
                {
                    MapPoint point = CreatePoint(i, j, sector, sector.MapPointType);
                    this.SetObject(point);
                }
            }
        }

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

        public King SearchKingById(int id)
        {
            lock (_kingsSync)
                return Kings.Search(x => x.Id == id);
        }

        public Mine SearchMineById(int id)
        {
            lock (_minesSync)
                return Mines.Search(x => x.Id == id);
        }

        public Castle SearchCastleById(int id)
        {
            lock (_castlesSync)
                return Castles.Search(x => x.Id == id);
        }

        public Resource SearchResourceById(int id)
        {
            lock (_resourcesSync)
                return Resources.Search(x => x.Id == id);
        }

        public Resource SearchResourceByPointId(int id)
        {
            lock (_resourcesSync)
                return Resources.Search(x => x.MapPointId == id);
        }

        public IObserver SearchObserverById(int observerId, PointTypes observerType)
        {
            Debug.Assert(observerType != PointTypes.None);
            if (observerType == PointTypes.King)
                return Kings.Search(x => x.Id == observerId);
            else
            {
                if (observerType == PointTypes.Castle)
                    return Castles.Search(x => x.Id == observerId);
                else
                {
                    if (observerType == PointTypes.Mine)
                        return Mines.Search(x => x.Id == observerId);
                    else throw new AliveChessException("Observer not found");
                }
            }
        }

        #endregion

        #region Add And Remove

        public void AddMine(Mine mine)
        {
            lock (_minesSync)
                Mines.Add(mine);

            lock (_minesSync)
            {
                if (!_observers.ContainsKey(mine.Id))
                {
                    _observers.Add(mine.Id, mine);
                }
            }
        }

        public void AddCastle(Castle castle)
        {
            lock (_castlesSync)
                Castles.Add(castle);

            lock (_observerSync)
            {
                if (!_observers.ContainsKey(castle.Id))
                {
                    _observers.Add(castle.Id, castle);
                }
            }
        }

        public void AddKing(King king)
        {
            lock (_kingsSync)
                Kings.Add(king);

            lock (_observerSync)
            {
                if (!_observers.ContainsKey(king.Id))
                {
                    _observers.Add(king.Id, king);
                }
            }
        }

        public void AddResource(Resource resource)
        {
            lock (_resourcesSync)
                Resources.Add(resource);
        }

        public void AddSingleObject(SingleObject singleObject)
        {
            lock (_singleSync)
                SingleObjects.Add(singleObject);
        }

        public void AddMultyObject(MultyObject multyObject)
        {
            lock (_multySync)
                MultyObjects.Add(multyObject);
        }

        public void AddLandscapePoint(BasePoint point)
        {
            lock (_landscapePoint)
                BasePoints.Add(point);
        }

        public void AddBorder(Border point)
        {
            lock (_borderSync)
                Borders.Add(point);
        }

        public void RemoveKing(King king)
        {
            lock (_kingsSync)
                Kings.Remove(king);

            lock (_observerSync)
                _observers.Remove(king.Id);

            //Debug.Assert(king.ViewOnMap != null);

            //if (king.ViewOnMap.ObjectUnderThis != null)
            //    SetObject(king.ViewOnMap.ObjectUnderThis);
        }

        public void RemoveCastle(Castle castle)
        {
            lock (_castlesSync)
                Castles.Remove(castle);

            lock (_observerSync)
                _observers.Remove(castle.Id);

            //Debug.Assert(castle.ViewOnMap != null);

            //foreach (MapPoint mp in castle.ViewOnMap.NextPoint())
            //    if (mp.ObjectUnderThis != null)
            //        SetObject(mp.ObjectUnderThis);
        }

        public void RemoveMine(Mine mine)
        {
            lock (_minesSync)
                Mines.Remove(mine);

            lock (_observerSync)
                _observers.Remove(mine.Id);

            //Debug.Assert(mine.ViewOnMap != null);

            //foreach (MapPoint mp in mine.ViewOnMap.NextPoint())
            //    if (mp.ObjectUnderThis != null)
            //        SetObject(mp.ObjectUnderThis);
        }

        public void RemoveResource(Resource resource)
        {
            lock (_resourcesSync)
                Resources.Remove(resource);

            //Debug.Assert(resource.ViewOnMap != null);

            //foreach (MapPoint mp in resource.ViewOnMap.MapPoints)
            //if (resource.ViewOnMap.ObjectUnderThis != null)
            //    SetObject(resource.ViewOnMap.ObjectUnderThis);
        }

        public void RemoveSingleObject(SingleObject single)
        {
            lock (_singleSync)
                SingleObjects.Remove(single);

            //Debug.Assert(single.ViewOnMap != null);

            //if (single.ViewOnMap.ObjectUnderThis != null)
            //    SetObject(single.ViewOnMap.ObjectUnderThis);
        }

        public void RemoveMultyObject(MultyObject multy)
        {
            lock (_multySync)
                MultyObjects.Remove(multy);

            //Debug.Assert(multy.ViewOnMap != null);

            //foreach (MapPoint mp in multy.ViewOnMap.NextPoint())
            //    if (mp.ObjectUnderThis != null)
            //        SetObject(mp.ObjectUnderThis);
        }

        public void RemoveLandscapePoint(BasePoint point)
        {
            lock (_landscapePoint)
                BasePoints.Remove(point);
        }

        public void RemoveBorder(Border point)
        {
            lock (_borderSync)
                Borders.Remove(point);
        }

        #endregion

        #region Check Existance

        public bool ContainsMine(long id)
        {
            lock (_minesSync)
            {
                foreach (Mine m in Mines)
                    if (m.Equals(id)) return true;
            }
            return false;
        }

        public bool ContainsKing(long id)
        {
            lock (_kingsSync)
            {
                for (int i = 0; i < Kings.Count; i++)
                    if (Kings[i].Equals(id)) return true;
            }
            return false;
        }

        public bool ContainsCastle(long id)
        {
            lock (_castlesSync)
            {
                foreach (Castle c in Castles)
                    if (c.Equals(id)) return true;
            }
            return false;
        }

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

        public void SetObject(MapPoint @object)
        {
            _objects[@object.X, @object.Y] = @object;
        }

        public MapPoint GetObject(int x, int y)
        {
            if (this.Locate(x, y))
                return _objects[x, y];
            else throw new IndexOutOfRangeException();
        }

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
        #endregion
    }
}