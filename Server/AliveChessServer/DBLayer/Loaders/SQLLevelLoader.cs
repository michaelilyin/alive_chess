using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using AliveChessLibrary;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Objects;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Utility;
using AliveChessServer.LogicLayer;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.DBLayer.Loaders
{
    public class SQLLevelLoader : ILevelLoader
    {
        private LevelRoutine _levelRoutine;
        private AliveChessDataContext _dqlContext;
        private AliveChessDataContext _dmlContext;
        private AliveChessLogger _logger;
       
        public SQLLevelLoader(string connectionString, AliveChessLogger logger)
        {
            this._logger = logger;
          
            _dqlContext = new AliveChessDataContext(connectionString);
            _dqlContext.DeferredLoadingEnabled = false;
            _dqlContext.ObjectTrackingEnabled = true;

            _dmlContext = new AliveChessDataContext(connectionString);
            _dmlContext.DeferredLoadingEnabled = false;
            _dmlContext.ObjectTrackingEnabled = true;

            DataLoadOptions options = new DataLoadOptions();
            options.LoadWith<Map>(x => x.Castles);
            options.LoadWith<Map>(x => x.Mines);
            this._dqlContext.LoadOptions = options;
         
            Castle.OnLoad += OnLoadCastle;
        }

        public LevelRoutine LevelRoutine
        {
            get { return _levelRoutine; }
            set { _levelRoutine = value; }
        }

        public void SaveMap(Map map, Action<Map> callback)
        {
            _dqlContext.Maps.InsertOnSubmit(map);
            _dqlContext.SubmitChanges();

            var level = new Level { Map = map, LevelType = LevelTypes.Easy };

            _dqlContext.Levels.InsertOnSubmit(level);
            _dqlContext.SubmitChanges();

            if (callback != null)
                callback.Invoke(map);
        }

        public void SaveBorders(List<Border> borders)
        {
            _dqlContext.Borders.InsertAllOnSubmit(borders);
            _dqlContext.SubmitChanges();
        }

        public void SaveBasePoints(List<BasePoint> basePoints)
        {
            _dqlContext.BasePoints.InsertAllOnSubmit(basePoints);
            _dqlContext.SubmitChanges();
        }

        public void SaveSingleObjects(List<SingleObject> singleObjects)
        {
            _dqlContext.SingleObjects.InsertAllOnSubmit(singleObjects);
            _dqlContext.SubmitChanges();
        }

        public void SaveMultyObjects(List<MultyObject> multyObjects)
        {
            _dqlContext.MultyObjects.InsertAllOnSubmit(multyObjects);
            _dqlContext.SubmitChanges();
        }

        public void SaveMines(List<Mine> mines, Action<List<Mine>> callback)
        {
            _dqlContext.Mines.InsertAllOnSubmit(mines);
            _dqlContext.SubmitChanges();

            if (callback != null)
                callback.Invoke(mines);
        }

        public void SaveCastles(List<Castle> castles, Action<List<Castle>> callback)
        {
            SaveStores(castles);
            _dqlContext.Castles.InsertAllOnSubmit(castles);
            _dqlContext.SubmitChanges();

            if (callback != null)
                callback.Invoke(castles);
        }

        private void SaveStores(IEnumerable<Castle> castles)
        {
            foreach (Castle castle in castles)
            {
                /*FigureStore figureStore = new FigureStore();
                this._dqlContext.FigureStores.InsertOnSubmit(figureStore);
                this._dqlContext.SubmitChanges();
                castle.FigureStore = figureStore;*/

                /*ResourceStore resourceStore = new ResourceStore();
                this._dqlContext.ResourceStores.InsertOnSubmit(resourceStore);
                this._dqlContext.SubmitChanges();
                castle.ResourceStore = resourceStore;*/
            }
        }

        public void SaveResources(List<Resource> resources, Action<List<Resource>> callback)
        {
            _dqlContext.Resources.InsertAllOnSubmit(resources);
            _dqlContext.SubmitChanges();

            if (callback != null)
                callback.Invoke(resources);
        }

        public void SavePlayer(Player player)
        {
            _dmlContext.Users.InsertOnSubmit(player);
            _dmlContext.SubmitChanges();
        }

        public void SaveKing(King king)
        {
            _dmlContext.Kings.InsertOnSubmit(king);
            _dmlContext.SubmitChanges();
        }

        public void CommitAllChanges()
        {
            _dmlContext.SubmitChanges();
        }

        public Level LoadLevel(LevelTypes levelType)
        {
            Level level = _dqlContext.Levels.FirstOrDefault(x => x.LevelType == levelType);
            if (level != null)
            {
                level.Map = _dqlContext.Maps.First(x => level.MapId == x.Id);
                level.Map.Initialize();
                Debug.Assert(level.Map.Castles.Count != 0);
                Debug.Assert(level.Map.Mines.Count != 0);
                BigMapRoutine.InitializeLandscape(level.Map);

                _dmlContext.Levels.Attach(level);
                _dmlContext.Maps.Attach(level.Map);

                return level;
            }
            else throw new KeyNotFoundException("Level with specified type doesn't exist");
        }

        public Player LoadPlayer(Identity identity)
        {
            if (_levelRoutine != null)
            {
                Player player = _dqlContext.Users.OfType<Player>().FirstOrDefault(x => x.Login == identity.Login
                    && x.Password == identity.Password);

                if (player != null)
                {
                    King king = _dqlContext.Kings.Single(x => x.PlayerId.HasValue && x.PlayerId == player.Id);
                   
                    //_logger.Log(king.Name, "Start:{X:" + king.X + " Y:" + king.Y + "}");

                    player.AddKing(king);
                    player.Level = _levelRoutine.Find(x => x.MapId == player.King.MapId);
                    player.Map = player.King.Map = player.Level.Map;
                    player.King.Castles.Assign(player.Level.Map.Castles.Filter(x => x.KingId == player.King.Id));
                    Debug.Assert(player.King.Castles.Count != 0);
                    player.King.Mines.Assign(player.Level.Map.Mines.Filter(x => x.KingId == player.King.Id));

                    return player;
                }
                else throw new AliveChessException("Player doesn't exist");
            }
            else throw new InvalidOperationException("LevelRoutine isn't intialized");
        }

        public Player FindPlayer(Func<Player, bool> predicate)
        {
            return _dqlContext.Users.OfType<Player>().FirstOrDefault(predicate);
        }

        public void DeferredLoadFigureStore(Castle sender)
        {
            /*FigureStore figureStore = _dqlContext
                .FigureStores.First(x => x.Id == sender.FigureStoreId);
            sender.FigureStore = figureStore;*/
        }

        public void DeferredLoadResourceStore(Castle sender)
        {
            /*ResourceStore resourceStore = _dqlContext
                .ResourceStores.First(x => x.Id == sender.ResourceStoreId);
            sender.ResourceStore = resourceStore;*/
        }

        private void OnLoadCastle(Castle target)
        {
            target.OnDeferredLoadingFigureStore   += DeferredLoadFigureStore;
            target.OnDeferredLoadingResourceStore += DeferredLoadResourceStore;
        }
    }
}
