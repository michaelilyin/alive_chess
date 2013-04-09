using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    [ProtoContract]
    public class Map : ILocalizable
    {
        #region Variables

        private ILevel _level;
        private MapPoint[,] _objects;
        private readonly Dictionary<int, IObserver> _observers;

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
            Kings = new List<King>();
            Mines = new List<Mine>();
            Castles = new List<Castle>();
            Resources = new List<Resource>();
            SingleObjects = new List<SingleObject>();
            MultyObjects = new List<MultyObject>();
            BasePoints = new List<BasePoint>();
            Borders = new List<Border>();
            _observers = new Dictionary<int, IObserver>();
        }

        public Map(int sizeX, int sizeY)
            : this()
        {
            Initialize(sizeX, sizeY);
        }

        #endregion

        #region Methods

        public void Initialize()
        {
            _objects = new MapPoint[SizeX,SizeY];
        }

        public void Initialize(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            _objects = new MapPoint[SizeX,SizeY];
        }

        public void Fill()
        {
            var algorithm = new FloodAlgorithm(this);
            foreach (BasePoint basePoint in BasePoints)
                algorithm.Run(basePoint);
        }

        public void TestLocate(int x, int y)
        {
            if (!Locate(x, y)) throw new IndexOutOfRangeException();
        }

        public static MapPoint CreatePoint(Position position, PointTypes type)
        {
            var point = new MapPoint {X = position.X, Y = position.Y, PointType = type, MapSector = null};
            return point;
        }

        public static MapPoint CreatePoint(int x, int y, PointTypes type)
        {
            var point = new MapPoint {X = x, Y = y, PointType = type, MapSector = null};
            return point;
        }

        public static MapPoint CreatePoint(Position position, MapSector sector, PointTypes type)
        {
            var point = new MapPoint {X = position.X, Y = position.Y, PointType = type, MapSector = sector};
            return point;
        }

        public static MapPoint CreatePoint(int x, int y, MapSector sector, PointTypes type)
        {
            var point = new MapPoint {X = x, Y = y, PointType = type, MapSector = sector};
            return point;
        }

        public static MapSector CreateSector(Position leftTopCorner, Position size, PointTypes type)
        {
            var sector = new MapSector
                             {
                                 X = leftTopCorner.X,
                                 Y = leftTopCorner.Y,
                                 Width = size.X,
                                 Height = size.Y,
                                 MapPointType = type
                             };
            return sector;
        }

        public static MapSector CreateSector(int leftX, int topY, int width, int height, PointTypes type)
        {
            var sector = new MapSector {X = leftX, Y = topY, Width = width, Height = height, MapPointType = type};
            return sector;
        }

        private void InitializeSector(MapSector sector)
        {
            for (var i = sector.X; i < sector.X + sector.Width; i++)
            {
                for (var j = sector.Y; j < sector.Y + sector.Height; j++)
                {
                    var point = CreatePoint(i, j, sector, sector.MapPointType);
                    SetObject(point);
                }
            }
        }

        public void Update(DateTime time)
        {
            lock (_kingsSync)
            {
                foreach (var t in Kings)
                    t.Update();
            }
            lock (_minesSync)
            {
                foreach (var t in Mines)
                    t.DoWork(time);
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

        public void AddMine(Mine mine)
        {
            lock (_minesSync)
                Mines.Add(mine);

            var mapSector = CreateSector(
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

        public void AddCastle(Castle castle)
        {
            lock (_castlesSync)
                Castles.Add(castle);

            var mapSector = CreateSector(
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

        public void AddKing(King king)
        {
            lock (_kingsSync)
                Kings.Add(king);

            var mapPoint = CreatePoint(
                king.X, king.Y,
                PointTypes.King);

            SetObject(mapPoint);

            king.AddView(mapPoint);
            king.Map = this;

            mapPoint.WayCost = king.WayCost;

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

            var mapPoint = CreatePoint(
                resource.X, resource.Y,
                PointTypes.Resource);

            SetObject(mapPoint);

            resource.AddView(mapPoint);

            mapPoint.WayCost = resource.WayCost;
        }

        public void AddSingleObject(SingleObject singleObject)
        {
            lock (_singleSync)
                SingleObjects.Add(singleObject);

            var mapPoint = CreatePoint(
                singleObject.X, singleObject.Y,
                PointTypes.SingleObject);

            SetObject(mapPoint);

            singleObject.AddView(mapPoint);

            mapPoint.WayCost = singleObject.WayCost;
        }

        public void AddMultyObject(MultyObject multyObject)
        {
            lock (_multySync)
                MultyObjects.Add(multyObject);

            var mapSector = CreateSector(
                multyObject.X, multyObject.Y,
                multyObject.Width, multyObject.Height,
                PointTypes.MultyObject);

            InitializeSector(mapSector);

            multyObject.AddView(mapSector);
            mapSector.WayCost = multyObject.WayCost;
        }

        public void AddBasePoint(BasePoint basePoint)
        {
            lock (_landscapePoint)
                BasePoints.Add(basePoint);

            var mapPoint = CreatePoint(
                basePoint.X, basePoint.Y,
                PointTypes.Landscape);

            SetObject(mapPoint);

            basePoint.AddView(mapPoint);

            mapPoint.WayCost = basePoint.WayCost;
        }

        public void AddBorder(Border border)
        {
            lock (_borderSync)
                Borders.Add(border);

            var mapPoint = CreatePoint(
                border.X, border.Y,
                PointTypes.Landscape);

            SetObject(mapPoint);

            border.AddView(mapPoint);

            mapPoint.WayCost = border.WayCost;
        }

        public void RemoveKing(King king)
        {
            lock (_kingsSync)
                Kings.Remove(king);

            lock (_observerSync)
                _observers.Remove(king.Id);

            king.RemoveView();
        }

        public void RemoveCastle(Castle castle)
        {
            lock (_castlesSync)
                Castles.Remove(castle);

            lock (_observerSync)
                _observers.Remove(castle.Id);

            castle.RemoveView();
        }

        public void RemoveMine(Mine mine)
        {
            lock (_minesSync)
                Mines.Remove(mine);

            lock (_observerSync)
                _observers.Remove(mine.Id);

            mine.RemoveView();
        }

        public void RemoveResource(Resource resource)
        {
            lock (_resourcesSync)
                Resources.Remove(resource);

            resource.RemoveView();
        }

        public void RemoveSingleObject(SingleObject single)
        {
            lock (_singleSync)
                SingleObjects.Remove(single);

            single.RemoveView();
        }

        public void RemoveMultyObject(MultyObject multy)
        {
            lock (_multySync)
                MultyObjects.Remove(multy);

            multy.RemoveView();
        }

        public void RemoveBasePoint(BasePoint point)
        {
            lock (_landscapePoint)
                BasePoints.Remove(point);

            point.RemoveView();
        }

        public void RemoveBorder(Border point)
        {
            lock (_borderSync)
                Borders.Remove(point);

            point.RemoveView();
        }

        #endregion

        #region Check Existance

        public bool ContainsMine(long id)
        {
            lock (_minesSync)
            {
                if (Mines.Any(m => m.Equals(id)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKing(long id)
        {
            lock (_kingsSync)
            {
                if (Kings.Any(t => t.Equals(id)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsCastle(long id)
        {
            lock (_castlesSync)
            {
                if (Castles.Any(c => c.Equals(id)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsResource(long id)
        {
            lock (_resourcesSync)
            {
                if (Resources.Any(r => r.Equals(id)))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Enumerators

        public IEnumerable NextKing()
        {
            lock (_kingsSync)
            {
                foreach (var t in Kings)
                    yield return t;
            }
        }

        public IEnumerable NextCastle()
        {
            lock (_castlesSync)
            {
                foreach (var t in Castles)
                    yield return t;
            }
        }

        public IEnumerable NextMine()
        {
            lock (_minesSync)
            {
                foreach (var t in Mines)
                    yield return t;
            }
        }

        public IEnumerable NextBorder()
        {
            lock (_borderSync)
            {
                foreach (var t in Borders)
                    yield return t;
            }
        }

        #endregion

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
            if (Locate(x, y))
                return _objects[x, y];
            throw new IndexOutOfRangeException();
        }

        public MapPoint GetObject(float x, float y)
        {
            return GetObject((int) x, (int) y);
        }

        public float GetWayCost(int x, int y)
        {
            return _objects[x, y].WayCost;
        }

        public bool Locate(int x, int y)
        {
            return x > -1 && x < SizeX - 1 && y > -1 && y < SizeY - 1;
        }

        public bool CheckPoint(int x, int y, int costLimit)
        {
            return Locate(x, y) && _objects[x, y].WayCost <= costLimit;
        }

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
            get { return _objects[x, y]; }
            set { _objects[x, y] = value; }
        }

        public MapPoint this[Position position]
        {
            get { return _objects[position.X, position.Y]; }
            set { _objects[position.X, position.Y] = value; }
        }

        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public int SizeX { get; set; }

        [ProtoMember(3)]
        public int SizeY { get; set; }

        [ProtoMember(4)]
        public string Name { get; set; }

        public List<King> Kings { get; set; }

        public List<Mine> Mines { get; set; }

        public List<Castle> Castles { get; set; }

        public List<Resource> Resources { get; set; }

        public List<SingleObject> SingleObjects { get; set; }

        public List<MultyObject> MultyObjects { get; set; }

        public List<BasePoint> BasePoints { get; set; }

        public List<Border> Borders { get; set; }

        #endregion
    }
}