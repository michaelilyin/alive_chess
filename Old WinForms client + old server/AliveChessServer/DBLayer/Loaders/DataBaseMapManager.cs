using AliveChessLibrary.GameObjects;
using AliveChessServer.LogicLayer.Environment;

namespace AliveChessServer.DBLayer.Loaders
{
    public class DataBaseMapManager
    {
        private GameData _gameData;
        private GameWorld _environment;
        private AliveChessDataContext _context;

        public DataBaseMapManager(GameWorld environment)
        {
            this._environment = environment;
            this._gameData = _environment.Context;
            this._context = new AliveChessDataContext(@"Data Source=ИГОРЬ-ПК\SQLEXPRESS;Initial Catalog=alive_chess;Integrated Security=True");
        }

        //public void Load()
        //{
        //    _environment.LevelManager.Load(from m in _context.Levels select m);
        //    foreach (Level level in _environment.LevelManager.NextLevel())
        //    {
        //        // TEST
        //        // LoadAliances(level);

        //        Map map = _context.Maps.Single(x => x.DbId == level.DbId);

        //        map.Initialize();
        //        level.Initialize(map, LevelTypes.Easy, _environment.BigMapRoutine,
        //            _environment.DisputeRoutine, _gameData);
        //        _environment.BigMapRoutine.AddMap(level.Map);

        //        LoadMines(map);
        //        LoadCastles(map);
        //        LoadResources(map);
        //        LoadSingleObjects(map);
        //        LoadMultyObjects(map);
        //        LoadBasePoints(map);

        //        CreateLandscape(map);
        //    }
        //}

        //// TEST
        //public void Save()
        //{
        //    //SingleObject mp = new SingleObject();
        //    //mp.DbId = Guid.NewGuid();
        //    //mp.MapId = Guid.NewGuid();
        //    //mp.ViewOnMap = new MapPoint();
        //    //mp.ViewOnMap.X = 12;
        //    //mp.ViewOnMap.Y = 23;
        //    //mp.ViewOnMap.ImageId = 12;
        //    //mp.SingleObjectType = SingleObjectType.Obstacle;
        //    //_procManager.Insert(mp);
        //}

        //private void Insert()
        //{
        //}

        //private void Update()
        //{
        //}

        //private void Delete()
        //{
        //}

        //public Player LoadPlayer(string username, string password)
        //{
        //    Player player = _context.Players.Single(x => x.Login == username && x.Password == password);
        //    player.King = _context.Kings.Single(x => x.DbId == player.DbId);
        //    player.King.Map = _context.Maps.Single(x => x.DbId == player.King.MapId);
        //    player.King.StartCastle.ResourceStore = _context.ResourceStores.Single(x => x.DbId == player.King.DbId);
        //    player.King.ViewOnMap = _context.MapPoints.Single(x => x.DbId == player.King.DbId);

        //    return player;
        //}

        //public Level LoadLevel(Player player)
        //{
        //    return _context.Levels.Single(x => x.DbId == player.LevelId);
        //}

        //public bool CheckPlayer(string login, string password)
        //{
        //    return _context.Players.Any(x => x.Login == login && x.Password == password);
        //}

        //private void CreateLandscape(Map map)
        //{
        //    for (int i = 0; i < map.BasePoints.Count; i++)
        //    {
        //        BasePoint lp = map.BasePoints[i];

        //        MapPoint mp = _context.MapPoints.Single(x => x.DbId == lp.DbId);

        //        mp.Id = GuidGenerator.Instance.GetUint(); // uint generating
        //        mp.MapPointType = PointTypes.Landscape;

        //        lp.AddView(mp);

        //        FloodFillAlgorithm floodFill = new FloodFillAlgorithm(map, lp, lp.LandscapeType,
        //            _environment.LevelManager.GameData);

        //        floodFill.Run(lp.X, lp.Y);

        //        ImageInfo image = new ImageInfo();
        //        image.ImageId = 0;

        //        for (int k = 0; k < map.SizeX; k++)
        //        {
        //            for (int j = 0; j < map.SizeY; j++)
        //            {
        //                if (map[k, j] == null)
        //                {
        //                    map.SetObject(Map.CreatePoint(Guid.Empty, 0, k, j, image,
        //                                                  PointTypes.Landscape, null, 0));
        //                }
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// закрузка замков
        ///// </summary>
        ///// <param name="map"></param>
        //private void LoadCastles(Map map)
        //{
        //    IEnumerable<Castle> castles = _context.Castles.Where(x => x.MapId == map.DbId);
        //    foreach (Castle item in castles)
        //    {
        //        item.Map = map;
        //        item.GameData = _environment.Context;

        //        MapSector ms = _context.MapSectors.Single(x => x.DbId == item.DbId);
        //        item.Vicegerent = _context.Vicegerenrs.Single(x => x.DbId == item.DbId);
        //        if (item.KingId != null) _context.Kings.Single(x => x.DbId == item.KingId);

        //        ms.MapPointType = PointTypes.Castle;
        //        map.InitializeSector(ms);
        //        //ms.Initialize(PointTypes.Castle, null);
        //        ms.Id = GuidGenerator.Instance.GetUint(); // uint generating

        //        item.AddView(ms);
        //        map.AddCastle(item);

        //        for (int j = 0; j < ms.MapPoints.Count; j++)
        //            ms.MapPoints[j].MapSector = ms;
        //    }
        //}

        ///// <summary>
        ///// загрузка шахт
        ///// </summary>
        ///// <param name="map"></param>
        //private void LoadMines(Map map)
        //{
        //    IEnumerable<Mine> mines = _context.Mines.Where(x => x.MapId == map.DbId);
        //    // IEnumerable<Mine> _m = map.Mines.SelectElements<Mine>(x => x.MapId == map.DbId);
        //    foreach (Mine item in mines)
        //    {
        //        ResourceTypes rt = item.MineType;
        //        item.Initialize(_environment.Context, map, rt, 10);

        //        MapSector ms = _context.MapSectors.Single(x => x.DbId == item.DbId);
        //        if (item.KingId != null) _context.Kings.Single(x => x.DbId == item.KingId);

        //        ms.MapPointType = PointTypes.Mine;
        //        map.InitializeSector(ms);
        //       // ms.Initialize(PointTypes.Mine, null);
        //        ms.Id = GuidGenerator.Instance.GetUint(); // uint generating

        //        item.AddView(ms);
        //        map.AddMine(item);

        //        for (int j = 0; j < ms.MapPoints.Count; j++)
        //            ms.MapPoints[j].MapSector = ms;

        //        item.GetResourceEvent += new Mine.GetResourceHandler(_environment.EconomyRoutine.SendResource);
        //    }
        //}

        ///// <summary>
        ///// закрузка одиночных объектов
        ///// </summary>
        ///// <param name="map"></param>
        //private void LoadSingleObjects(Map map)
        //{
        //    IEnumerable<SingleObject> singleObjects = _context.SingleObjects
        //        .Where(x => x.MapId == map.DbId);

        //    foreach (SingleObject item in singleObjects)
        //    {
        //        item.Map = map;
        //        MapPoint mp = _context.MapPoints.Single(x => x.DbId == item.DbId);
        //        mp.MapPointType = PointTypes.SingleObject;
        //        item.AddView(mp);
        //    }
        //}

        ///// <summary>
        ///// загрузка крупных объектов
        ///// </summary>
        ///// <param name="map"></param>
        //private void LoadMultyObjects(Map map)
        //{
        //    IEnumerable<MultyObject> multyObjects = _context.MultyObjects
        //        .Where(x => x.MapId == map.DbId);

        //    foreach (MultyObject item in multyObjects)
        //    {
        //        item.Map = map;
        //        MapSector ms = _context.MapSectors.Single(x => x.DbId == item.DbId);

        //        ms.MapPointType = PointTypes.MultyObject;
        //        map.InitializeSector(ms);
        //        //ms.Initialize(PointTypes.MultyObject, null);
        //        item.AddView(ms);

        //        for (int j = 0; j < ms.MapPoints.Count; j++)
        //            ms.MapPoints[j].MapSector = ms;
        //    }
        //}

        ///// <summary>
        ///// загрузка ресурсов
        ///// </summary>
        ///// <param name="map"></param>
        //private void LoadResources(Map map)
        //{
        //    IEnumerable<Resource> resources = _context.Resources
        //        .Where(x => x.MapId == map.DbId);

        //    ImageInfo image = new ImageInfo();
        //    image.ImageId = 0;

        //    foreach (Resource item in resources)
        //    {
        //        item.Map = map;
        //        MapPoint mp = _context.MapPoints.Single(x => x.DbId == item.DbId);

        //        mp.MapPointType = PointTypes.Resource;
        //        mp.ObjectUnderThis = Map.CreatePoint(Guid.Empty, 0, mp.X, mp.Y, image,
        //                                             PointTypes.Landscape, null, 0);
        //        mp.Id = GuidGenerator.Instance.GetUint(); // uint generating

        //        item.AddView(mp);
        //    }
        //}

        ///// <summary>
        ///// загрузка опорных точек для заливки
        ///// </summary>
        ///// <param name="map"></param>
        //private void LoadBasePoints(Map map)
        //{
        //    IEnumerable<BasePoint> basePoints = _context.Landscapes
        //        .Where(x => x.MapId == map.DbId);

        //    foreach (BasePoint item in basePoints)
        //    {
        //        item.Map = map;
        //        MapPoint mp = _context.MapPoints.Single(x => x.DbId == item.DbId);
        //        mp.Id = GuidGenerator.Instance.GetUint(); // uint generating
        //        item.AddView(mp);
        //    }
        //}

        //private void LoadAliances(Level level)
        //{

        //}

        //public AliveChessDataClassesDataContext Context
        //{
        //    get { return _context; }
        //}
    }
}
