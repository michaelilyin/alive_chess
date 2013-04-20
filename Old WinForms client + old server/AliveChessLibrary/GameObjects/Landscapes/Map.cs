using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Objects;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    [Table(Name = "dbo.map")]
    public class Map : ILocalizable
    {
        #region Variables

        private int _mapId;
      
        private int _mapSizeX;
        private int _mapSizeY;
      
        private MapPoint[,] objects;
      
        private EntitySet<King> _kings;
        private EntitySet<Mine> _mines;
        private EntitySet<Castle> _castles;
        private EntitySet<Resource> _resources;
        private EntitySet<SingleObject> _singleObjects;
        private EntitySet<MultyObject> _multyObjects;
        private EntitySet<BasePoint> _basePoints;
        private EntitySet<Border> _borders;

        private Dictionary<int, IObserver> _observers;

        private object kingsSync = new object();
        private object minesSync = new object();
        private object resourcesSync = new object();
        private object castlesSync = new object();
        private object observerSync = new object();
        private object singleSync = new object();
        private object multySync = new object();
        private object landscapePoint = new object();
        private object borderSync = new object();
      
        #endregion

        #region Constructors

        public Map()
        {
            this._kings = new EntitySet<King>(AttachKing, DetachKing);
            this._mines = new EntitySet<Mine>(AttachMine, DetachMine);
            this._castles = new EntitySet<Castle>(AttachCastle, DetachCastle);
            this._resources = new EntitySet<Resource>(AttachResource, DetachResource);
            this._singleObjects = new EntitySet<SingleObject>(AttachSingleObject, DetachSingleObject);
            this._multyObjects = new EntitySet<MultyObject>(AttachMultyObject, DetachMultyObject);
            this._basePoints = new EntitySet<BasePoint>(AttachLandscapePoint, DetachLandscapePoint);
            this._borders = new EntitySet<Border>(AttachBorder, DetachBorder);

            this._observers = new Dictionary<int, IObserver>();
        }

        public Map(int sizeX, int sizeY)
            :this()
        {
            //this.Id = id;
            Initialize(Guid.Empty, sizeX, sizeY);
        }

        #endregion

        #region Methods

        public void Initialize()
        {
            this.objects = new MapPoint[this.SizeX, this.SizeY];
        }

        public void Initialize(Guid id, int sizeX, int sizeY)
        {
            //this._mapId = id;
            this._mapSizeX = sizeX;
            this._mapSizeY = sizeY;
            this.objects = new MapPoint[this.SizeX, this.SizeY];
        }

        public void TestLocate(int x, int y)
        {
            if (!Locate(x, y)) throw new IndexOutOfRangeException();
        }

        public static MapPoint CreatePoint(Position position, ImageInfo image,
            PointTypes type, MapPoint under, float wayCost)
        {
            MapPoint point = new MapPoint();
            point.X = position.X;
            point.Y = position.Y;
            point.ImageId = image.ImageId;
            point.MapPointType = type;
            point.ObjectUnderThis = under;
            point.WayCost = wayCost;
            point.MapSector = null;
            point.Detected = false;
            return point;
        }

        public static MapPoint CreatePoint(int id, Position position, ImageInfo image,
           PointTypes type, MapPoint under, float wayCost)
        {
            MapPoint point = new MapPoint();
            point.Id = id;
            point.X = position.X;
            point.Y = position.Y;
            point.ImageId = image.ImageId;
            point.MapPointType = type;
            point.ObjectUnderThis = under;
            point.WayCost = wayCost;
            point.MapSector = null;
            point.Detected = false;
            return point;
        }

        public static MapPoint CreatePoint(int x, int y, ImageInfo image,
            PointTypes type, MapPoint under, float wayCost)
        {
            MapPoint point = new MapPoint();
            point.X = x;
            point.Y = y;
            point.ImageId = image.ImageId;
            point.MapPointType = type;
            point.ObjectUnderThis = under;
            point.WayCost = wayCost;
            point.MapSector = null;
            point.Detected = false;
            return point;
        }

        public static MapPoint CreatePoint(int id, int x, int y, ImageInfo image,
           PointTypes type, MapPoint under, float wayCost)
        {
            MapPoint point = new MapPoint();
            point.Id = id;
            point.X = x;
            point.Y = y;
            point.ImageId = image.ImageId;
            point.MapPointType = type;
            point.ObjectUnderThis = under;
            point.WayCost = wayCost;
            point.MapSector = null;
            point.Detected = false;
            return point;
        }

        public static MapPoint CreatePoint(int x, int y, int? imageId,
           PointTypes type, MapPoint under, float wayCost)
        {
            MapPoint point = new MapPoint();
            point.X = x;
            point.Y = y;
            point.ImageId = imageId;
            point.MapPointType = type;
            point.ObjectUnderThis = under;
            point.WayCost = wayCost;
            point.MapSector = null;
            point.Detected = false;
            return point;
        }

        public static MapPoint CreatePoint(int id, int x, int y, int? imageId,
          PointTypes type, MapPoint under, float wayCost)
        {
            MapPoint point = new MapPoint();
            point.Id = id;
            point.X = x;
            point.Y = y;
            point.ImageId = imageId;
            point.MapPointType = type;
            point.ObjectUnderThis = under;
            point.WayCost = wayCost;
            point.MapSector = null;
            point.Detected = false;
            return point;
        }

        public static MapPoint CreatePoint(Position position, MapSector sector,
            PointTypes type, MapPoint under, float wayCost)
        {
            MapPoint point = new MapPoint();
            point.X = position.X;
            point.Y = position.Y;
            point.MapPointType = type;
            point.ObjectUnderThis = under;
            point.WayCost = wayCost;
            point.MapSector = sector;
            point.Detected = false;
            point.ImageId = null;
            return point;
        }

        public static MapPoint CreatePoint(int id, Position position, MapSector sector,
           PointTypes type, MapPoint under, float wayCost)
        {
            MapPoint point = new MapPoint();
            point.Id = id;
            point.X = position.X;
            point.Y = position.Y;
            point.MapPointType = type;
            point.ObjectUnderThis = under;
            point.WayCost = wayCost;
            point.MapSector = sector;
            point.Detected = false;
            point.ImageId = null;
            return point;
        }

        public static MapPoint CreatePoint(int x, int y, MapSector sector,
           PointTypes type, MapPoint under, float wayCost)
        {
            MapPoint point = new MapPoint();
            point.X = x;
            point.Y = y;
            point.MapPointType = type;
            point.ObjectUnderThis = under;
            point.WayCost = wayCost;
            point.MapSector = sector;
            point.Detected = false;
            point.ImageId = null;
            return point;
        }

        public static MapPoint CreatePoint(int id, int x, int y, MapSector sector,
          PointTypes type, MapPoint under, float wayCost)
        {
            MapPoint point = new MapPoint();
            point.Id = id;
            point.X = x;
            point.Y = y;
            point.MapPointType = type;
            point.ObjectUnderThis = under;
            point.WayCost = wayCost;
            point.MapSector = sector;
            point.Detected = false;
            point.ImageId = null;
            return point;
        }

        public static MapSector CreateSector(Position leftTopCorner, ImageInfo image,
            PointTypes type, float wayCost)
        {
            MapSector sector = new MapSector();
            sector.X = leftTopCorner.X;
            sector.Y = leftTopCorner.Y;
            sector.Width = image.Width.Value;
            sector.Height = image.Height.Value;
            sector.ImageId = image.ImageId.Value;
            sector.MapPointType = type;
            sector.WayCost = wayCost;
            return sector;
        }

        public static MapSector CreateSector(int id, Position leftTopCorner, ImageInfo image,
           PointTypes type, float wayCost)
        {
            MapSector sector = new MapSector();
            sector.Id = id;
            sector.X = leftTopCorner.X;
            sector.Y = leftTopCorner.Y;
            sector.Width = image.Width.Value;
            sector.Height = image.Height.Value;
            sector.ImageId = image.ImageId.Value;
            sector.MapPointType = type;
            sector.WayCost = wayCost;
            return sector;
        }

        public void InitializeSector(MapSector sector)
        {
            for (int i = sector.X; i < sector.X + sector.Width; i++)
            {
                for (int j = sector.Y; j < sector.Y + sector.Height; j++)
                {
                    MapPoint point = CreatePoint(sector.Id, i, j, sector, sector.MapPointType,
                                                null, sector.WayCost);
                    sector.AddPoint(point);
                    this.SetObject(point);
                }
            }
        }
      
        #region Searchers

        public King SearchKingById(int id)
        {
            lock (kingsSync)
                return _kings.FirstOrDefault(x => x.Id == id);
        }

        public Mine SearchMineById(int id)
        {
            lock (minesSync)
                return _mines.FirstOrDefault(x => x.Id == id);
        }

        public Castle SearchCastleById(int id)
        {
            lock (castlesSync)
                return _castles.FirstOrDefault(x => x.Id == id);
        }

        public Resource SearchResourceById(int id)
        {
            lock (resourcesSync)
                return _resources.FirstOrDefault(x => x.Id == id);
        }

        public King SearchKingByPointId(int id)
        {
            lock (kingsSync)
                return _kings.FirstOrDefault(x => x.MapPointId == id);
        }

        public Mine SearchMineByPointId(int id)
        {
            lock (minesSync)
                return _mines.FirstOrDefault(x => x.MapSectorId == id);
        }

        public Castle SearchCastleByPointId(int id)
        {
            lock (castlesSync)
                return _castles.FirstOrDefault(x => x.MapSectorId == id);
        }

        public Resource SearchResourceByPointId(int id)
        {
            lock (resourcesSync)
                return _resources.FindElement(x => x.MapPointId == id);
        }

        public IObserver SearchObserverById(int observerId, PointTypes observerType)
        {
            Debug.Assert(observerType != PointTypes.None);
            if (observerType == PointTypes.King)
                return _kings.FirstOrDefault(x => x.Id == observerId);
            else
            {
                if (observerType == PointTypes.Castle)
                    return _castles.FirstOrDefault(x => x.Id == observerId);
                else
                {
                    if (observerType == PointTypes.Mine)
                        return _mines.FirstOrDefault(x => x.Id == observerId);
                    else throw new AliveChessException("Observer not found");
                }
            }
        }

        #endregion

        #region Add And Remove

        public void AddMine(Mine mine)
        {
            lock (minesSync)
                _mines.Add(mine);

            lock (minesSync)
                _observers.Add(mine.Id, mine);
        }

        public void AddCastle(Castle castle)
        {
            lock (castlesSync)
                _castles.Add(castle);

            lock (observerSync)
                _observers.Add(castle.Id, castle);
        }

        public void AddKing(King king)
        {
            lock (kingsSync)
                _kings.Add(king);

            lock (observerSync)
                _observers.Add(king.Id, king);
        }

        public void AddResource(Resource resource)
        {
            lock (resourcesSync)
                _resources.Add(resource);
        }

        public void AddSingleObject(SingleObject singleObject)
        {
            lock (singleSync)
                _singleObjects.Add(singleObject);
        }

        public void AddMultyObject(MultyObject multyObject)
        {
            lock (multySync)
                _multyObjects.Add(multyObject);
        }

        public void AddLandscapePoint(BasePoint point)
        {
            lock (landscapePoint)
                _basePoints.Add(point);
        }

        public void AddBorder(Border point)
        {
            lock (borderSync)
                _borders.Add(point);
        }

        public void RemoveKing(King king)
        {
            lock (kingsSync)
                _kings.Remove(king);

            lock (observerSync)
                _observers.Remove(king.Id);

            Debug.Assert(king.ViewOnMap != null);

            if (king.ViewOnMap.ObjectUnderThis != null)
                SetObject(king.ViewOnMap.ObjectUnderThis);
        }

        public void RemoveCastle(Castle castle)
        {
            lock (castlesSync)
                _castles.Remove(castle);

            lock (observerSync)
                _observers.Remove(castle.Id);

            Debug.Assert(castle.ViewOnMap != null);

            foreach (MapPoint mp in castle.ViewOnMap.NextPoint())
                if (mp.ObjectUnderThis != null)
                    SetObject(mp.ObjectUnderThis);
        }

        public void RemoveMine(Mine mine)
        {
            lock (minesSync)
                _mines.Remove(mine);

            lock (observerSync)
                _observers.Remove(mine.Id);

            Debug.Assert(mine.ViewOnMap != null);

            foreach (MapPoint mp in mine.ViewOnMap.NextPoint())
                if (mp.ObjectUnderThis != null)
                    SetObject(mp.ObjectUnderThis);
        }

        public void RemoveResource(Resource resource)
        {
            lock (resourcesSync)
                _resources.Remove(resource);

            Debug.Assert(resource.ViewOnMap != null);

            //foreach (MapPoint mp in resource.ViewOnMap.MapPoints)
            if (resource.ViewOnMap.ObjectUnderThis != null)
                SetObject(resource.ViewOnMap.ObjectUnderThis);
        }

        public void RemoveSingleObject(SingleObject single)
        {
            lock (singleSync)
                _singleObjects.Remove(single);

            Debug.Assert(single.ViewOnMap != null);

            if (single.ViewOnMap.ObjectUnderThis != null)
                SetObject(single.ViewOnMap.ObjectUnderThis);
        }

        public void RemoveMultyObject(MultyObject multy)
        {
            lock (multySync)
                _multyObjects.Remove(multy);

            Debug.Assert(multy.ViewOnMap != null);

            foreach (MapPoint mp in multy.ViewOnMap.NextPoint())
                if (mp.ObjectUnderThis != null)
                    SetObject(mp.ObjectUnderThis);
        }

        public void RemoveLandscapePoint(BasePoint point)
        {
            lock (landscapePoint)
                _basePoints.Remove(point);
        }

        public void RemoveBorder(Border point)
        {
            lock (borderSync)
                _borders.Remove(point);
        }

        #endregion

        #region Check Existance

        public bool ContainsMine(long id)
        {
            lock (minesSync)
            {
                foreach (Mine m in _mines)
                    if (m.Equals(id)) return true;
            }
            return false;
        }

        public bool ContainsKing(long id)
        {
            lock (kingsSync)
            {
                foreach (King k in _kings)
                    if (k.Equals(id)) return true;
            }
            return false;
        }

        public bool ContainsCastle(long id)
        {
            lock (castlesSync)
            {
                foreach (Castle c in _castles)
                    if (c.Equals(id)) return true;
            }
            return false;
        }

        public bool ContainsResource(long id)
        {
            lock (resourcesSync)
            {
                foreach (Resource r in _resources)
                    if (r.Equals(id)) return true;
            }
            return false;
        }

        #endregion

        #region Enumerators

        public IEnumerable NextKing()
        {
            lock (kingsSync)
            {
                for (int i = 0; i < _kings.Count; i++)
                    yield return _kings[i];
            }
        }

        public IEnumerable NextCastle()
        {
            lock (castlesSync)
            {
                for (int i = 0; i < _castles.Count; i++)
                    yield return _castles[i];
            }
        }

        public IEnumerable NextMine()
        {
            lock (minesSync)
            {
                for (int i = 0; i < _mines.Count; i++)
                    yield return _mines[i];
            }
        }

        public IEnumerable NextBorder()
        {
            lock (borderSync)
            {
                for (int i = 0; i < _borders.Count; i++)
                    yield return _borders[i];
            }
        }

        #endregion

        /// <summary>
        /// поиск пустого замка
        /// </summary>
        /// <returns></returns>
        public Castle SearchFreeCastle()
        {
            return _castles.First(delegate(Castle c) { return c.IsFree; });
        }

        /// <summary>
        /// добавляем объект на карту
        /// </summary>
        /// <param name="x">координата x</param>
        /// <param name="y">координата y</param>
        public void SetObject(MapPoint obj)
        {
            objects[obj.X, obj.Y] = obj;
        }

        /// <summary>
        /// получаем стоимость прохождения через ячейку
        /// </summary>
        /// <param name="x">координата x</param>
        /// <param name="y">координата y</param>
        /// <returns></returns>
        public float GetWayCost(int x, int y)
        {
            return objects[x, y].WayCost;
        }

        /// <summary>
        /// проверка принадлежности ячейки карте
        /// </summary>
        /// <param name="x">координата x</param>
        /// <param name="y">координата y</param>
        /// <returns></returns>
        public bool Locate(int x, int y)
        {
            return x >= 1 && x < this._mapSizeX - 1 && y >= 1 && y < this._mapSizeY - 1;
        }

        public bool CheckPoint(int x, int y, int costLimit)
        {
            return Locate(x, y) && objects[x, y].WayCost <= costLimit;
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

        public MapPoint this[int x, int y]
        {
            get
            {
                return objects[x, y];
            }
            set
            {
                objects[x, y] = value;
            }
        }

        public MapPoint this[Position position]
        {
            get
            {
                return objects[position.X, position.Y];
            }
            set
            {
                objects[position.X, position.Y] = value;
            }
        }

        [Column(Name = "map_id", Storage = "_mapId", CanBeNull = false, DbType = Constants.DB_INT,
            IsPrimaryKey = true, IsDbGenerated = true)]
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

        [Column(Name = "map_sizeX", Storage = "_mapSizeX", CanBeNull = false, DbType = Constants.DB_INT)]
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

        [Column(Name = "map_sizeY", Storage = "_mapSizeY", CanBeNull = false, DbType = Constants.DB_INT)]
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

        [Association(Name = "fk_castle_map", Storage = "_castles", OtherKey = "MapId")]
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

        [Association(Name = "fk_king_map", Storage = "_kings", OtherKey = "MapId")]
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

        [Association(Name = "fk_mine_map", Storage = "_mines", OtherKey = "MapId")]
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

        [Association(Name = "fk_resource_map", Storage = "_resources", OtherKey = "MapId")]
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

        [Association(Name = "fk_single_object_map", Storage = "_singleObjects", OtherKey = "MapId")]
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

        [Association(Name = "fk_multy_object_map", Storage = "_multyObjects", OtherKey = "MapId")]
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

        [Association(Name = "fk_landscape_point_map", Storage = "_basePoints", OtherKey = "MapId")]
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

        [Association(Name = "fk_border_point_map", Storage = "_borders", OtherKey = "MapId")]
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

        #endregion
    }
}