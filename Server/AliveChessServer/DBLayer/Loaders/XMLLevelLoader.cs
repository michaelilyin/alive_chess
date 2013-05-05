using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Objects;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Utility;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.DBLayer.Loaders
{
    public class XMLLevelLoader : ILevelLoader
    {
        private LevelRoutine _levelRoutine;
        private readonly Dictionary<string, BuildObject> _constructors;
        private readonly List<Identity> _players = new List<Identity>();
        Dictionary<string, float> wayCosts = new Dictionary<string, float>();

        public XMLLevelLoader(GameWorld environment)
        {
            _constructors = new Dictionary<string, BuildObject>();
            _constructors.Add("Castle", CreateCastle);
            _constructors.Add("Mine", CreateMine);
            _constructors.Add("Single", CreateSingle);
            _constructors.Add("Resource", CreateResource);
        }

        public Level LoadLevel(LevelTypes levelType)
        {
            Map map = null;
            Level level = null;
            BasePoint basePoint = null;
            GuidIDPair mapGuidGen = GuidGenerator.Instance.GeneratePair();
            GuidIDPair basePointGuidGen = GuidGenerator.Instance.GeneratePair();
            XmlTextReader reader = new XmlTextReader("..\\XML\\Map.xml");
            bool mapIsReady = false;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "map":
                            {
                                int sizeX = Convert.ToInt32(reader.GetAttribute("sizeX"));
                                int sizeY = Convert.ToInt32(reader.GetAttribute("sizeY"));

                                map = new Map(sizeX, sizeY);
                                map.Id = mapGuidGen.Id;
                                level = new Level(map, levelType);
                                break;
                            }
                        case "wayCost":
                            {
                                double value;
                                try
                                {
                                    value = Convert.ToDouble(reader.GetAttribute("value"));
                                }
                                catch (FormatException e)
                                {
                                    break;
                                }
                                string key = reader.GetAttribute("type");
                                if (key != null)
                                    wayCosts.Add(key, (float) value);
                                break;
                            }
                        case "mapObject":
                            {
                                if (!mapIsReady && map != null)
                                {
                                    mapIsReady = true;
                                    map.Fill();
                                }
                                _constructors[reader.GetAttribute("type")].Invoke(map, reader);
                            }
                            break;
                        case "basePoint":
                            {
                                int x = Convert.ToInt32(reader.GetAttribute("x"));
                                int y = Convert.ToInt32(reader.GetAttribute("y"));
                                basePoint = new BasePoint();
                                basePoint.Id = basePointGuidGen.Id;
                                basePoint.X = x;
                                basePoint.Y = y;
                                string type = reader.GetAttribute("type");
                                if (type == null)
                                    break;
                                LandscapeTypes landscapeType = LandscapeTypes.None;
                                switch (type)
                                {
                                    case "Grass":
                                        landscapeType = LandscapeTypes.Grass;
                                        break;
                                    case "Snow":
                                        landscapeType = LandscapeTypes.Snow;
                                        break;
                                    case "Ground":
                                        landscapeType = LandscapeTypes.Ground;
                                        break;
                                }
                                basePoint.WayCost = wayCosts[type];
                                basePoint.LandscapeType = landscapeType;
                                map.AddBasePoint(basePoint);
                            }
                            break;
                        case "player":
                            {
                                string login = reader.GetAttribute("login");
                                string password = reader.GetAttribute("password");
                                _players.Add(new Identity(login, password));
                            }
                            break;
                        case "players":
                            {
#warning Тут пусто
                            }
                            break;
                    }
                }
            }

            //CreateLandscape(map, basePoint);
            if (!mapIsReady)
            {
                mapIsReady = true;
                map.Fill();
            }

            return level;
        }

        private void CreateSingle(Map map, XmlReader reader)
        {
            string type = reader.GetAttribute("stype");

            SingleObjectType objType = SingleObjectType.Tree;

            switch (type)
            {
                case "Tree":
                    objType = SingleObjectType.Tree;
                    break;
                case "Obstacle":
                    objType = SingleObjectType.Obstacle;
                    break;
            }

            int x = Convert.ToInt32(reader.GetAttribute("x"));
            int y = Convert.ToInt32(reader.GetAttribute("y"));

            int ID = GuidGenerator.Instance.GeneratePair().Id;

            var single = new SingleObject
                         {
                             Id = ID,
                             Map = map,
                             X = x,
                             Y = y,
                             WayCost = wayCosts[type ?? objType.ToString()],
                             SingleObjectType = objType,
                         };

            map.AddSingleObject(single);
        }

        private void CreateResource(Map map, XmlReader reader)
        {
            string type = reader.GetAttribute("rtype");
            ResourceTypes objType = ResourceTypes.Gold;

            switch (type)
            {
                case "Gold":
                    objType = ResourceTypes.Gold;
                    break;
                case "Wood":
                    objType = ResourceTypes.Wood;
                    break;
                case "Iron":
                    objType = ResourceTypes.Iron;
                    break;
                case "Coal":
                    objType = ResourceTypes.Coal;
                    break;
                case "Stone":
                    objType = ResourceTypes.Stone;
                    break;
            }

            int x = Convert.ToInt32(reader.GetAttribute("x"));
            int y = Convert.ToInt32(reader.GetAttribute("y"));

            int ID = GuidGenerator.Instance.GeneratePair().Id;

            var resource = new Resource
                           {
                               Id = ID,
                               Map = map,
                               X = x,
                               Y = y,
                               WayCost = wayCosts["Resource"],
                               ResourceType = objType
                           };
            resource.Quantity = Convert.ToInt32(reader.GetAttribute("quantity"));

#if DEBUG
            AliveChessLibrary.DebugConsole.WriteLine("XMLLevelLoader", "Resource: " + objType.ToString() + " x = " + resource.X + " y = " + resource.Y);
#endif
            map.AddResource(resource);
        }

        private void CreateCastle(Map map, XmlReader reader)
        {
            int lX = Convert.ToInt32(reader.GetAttribute("leftX"));
            int tY = Convert.ToInt32(reader.GetAttribute("topY"));

            int ID = GuidGenerator.Instance.GeneratePair().Id;

            Castle castle = new Castle
                            {
                                Id = ID,
                                Map = map,
                                X = lX,
                                Y = tY,
                                Width = 2,
                                Height = 2,
                                WayCost = wayCosts["Castle"]
                            };

            FigureStore figures = new FigureStore { Id = ID };
            castle.FigureStore = figures;
            Unit uu = new Unit();
            uu.UnitType = UnitType.Knight;
            uu.Quantity = 1;
            figures.AddFigureToRepository(uu);

            /*ResourceStore resourceStore = new ResourceStore { Id = ID };
            castle.ResourceStore = resourceStore;*/

            castle.CreateInitialArmy();

            Vicegerent vicegerent = new Vicegerent
                                    {
                                        Id = ID,
                                        Name = "Vicegerent"
                                    };

            castle.Vicegerent = vicegerent;
            castle.Initialize(map);

            map.AddCastle(castle);
        }

        private void CreateMine(Map map, XmlReader reader)
        {
            string type = reader.GetAttribute("rtype");

            ResourceTypes objType = ResourceTypes.Gold;

            switch (type)
            {
                case "Gold":
                    objType = ResourceTypes.Gold;
                    break;
                case "Wood":
                    objType = ResourceTypes.Wood;
                    break;
                case "Iron":
                    objType = ResourceTypes.Iron;
                    break;
                case "Stone":
                    objType = ResourceTypes.Stone;
                    break;
                case "Coal":
                    objType = ResourceTypes.Coal;
                    break;
            }
            int lX = Convert.ToInt32(reader.GetAttribute("leftX"));
            int tY = Convert.ToInt32(reader.GetAttribute("topY"));

            int ID = GuidGenerator.Instance.GeneratePair().Id;

            var mine = new Mine
                       {
                           Id = ID,
                           Map = map,
                           X = lX,
                           Y = tY,
                           Width = 2,
                           Height = 2,
                           WayCost = wayCosts["Mine"],
                           MineType = objType,
                           SizeMine = 100,
                       };

#if DEBUG
            AliveChessLibrary.DebugConsole.WriteLine("XMLLevelLoader", "Mine: " + objType.ToString() + " x = " + mine.X + " y = " + mine.Y);
#endif
            //TODO: Читать из XML экономики
            int size = 10;
            int intensivity = 10;
            mine.Initialize(ID, map, objType, size, intensivity);
            map.AddMine(mine);
        }

        private static void CreateLandscape(Map map, BasePoint basePoint)
        {
            map.AddBasePoint(basePoint);
            map.Fill();
            Correct(map, basePoint);
        }

        private static void Correct(Map map, BasePoint basePoint)
        {
            for (int i = 0; i < map.SizeX; i++)
            {
                for (int j = 0; j < map.SizeY; j++)
                {
                    if (map[i, j].Owner == null)
                        map[i, j].SetOwner(basePoint);

                    if (map[i, j].PointType == PointTypes.None)
                        Debugger.Break();
                }
            }
        }

        public LevelRoutine LevelRoutine
        {
            get { return _levelRoutine; }
            set { _levelRoutine = value; }
        }

        private delegate void BuildObject(Map map, XmlReader reader);

        #region ILevelLoader Members


        public Player LoadPlayer(Identity identity)
        {
            Level level = _levelRoutine.GetLevelByType(LevelTypes.Easy);

            if (_players.Exists(x => x.Login == identity.Login
                && x.Password == identity.Password))
            {
                int id = GuidGenerator.Instance.GeneratePair().Id;

                var player = new Player(level, identity.Login, identity.Password);

                player.Id = id;
                ResourceStore resourceStore = new ResourceStore();
                resourceStore.Id = player.Id;
                var king = new King
                           {
                               Id = id,
                               Experience = 0,
                               MilitaryRank = 0,
                               ResourceStore = resourceStore
                           };

                Resource rr = new Resource();
                rr.ResourceType = ResourceTypes.Gold;
                rr.Quantity = 500;
                resourceStore.AddResourceToStore(rr);
                rr = new Resource();
                rr.ResourceType = ResourceTypes.Wood;
                rr.Quantity = 0;
                resourceStore.AddResourceToStore(rr);
                rr = new Resource();
                rr.ResourceType = ResourceTypes.Stone;
                rr.Quantity = 0;
                resourceStore.AddResourceToStore(rr);
                rr = new Resource();
                rr.ResourceType = ResourceTypes.Coal;
                rr.Quantity = 0;
                resourceStore.AddResourceToStore(rr);
                rr = new Resource();
                rr.ResourceType = ResourceTypes.Iron;
                rr.Quantity = 0;
                resourceStore.AddResourceToStore(rr);

                player.AddKing(king);
                player.Map = player.King.Map = player.Level.Map;

                Castle castle = player.Level.Map.SearchFreeCastle();
                if (castle == null)
                {
                    //TODO: Нужно перенаправлять игрока на другую карту
                }

                king.X = castle.X != player.Map.SizeX - 1
                             ? castle.X - 1
                             : castle.X + castle.Width + 1;
                king.Y = castle.Y + castle.Height != player.Map.SizeY - 1
                             ? castle.Y + castle.Height + 1
                             : castle.Y - 1;

                king.Castles.Add(castle);

                return player;
            }

            return null;
        }

        public void SavePlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public void SaveKing(King king)
        {
            throw new NotImplementedException();
        }

        public void CommitAllChanges()
        {
            throw new NotImplementedException();
        }

        public Player FindPlayer(Func<Player, bool> predicate)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
