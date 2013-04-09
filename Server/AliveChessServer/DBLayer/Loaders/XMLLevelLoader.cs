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

        public XMLLevelLoader(GameWorld environment)
        {
            _constructors = new Dictionary<string, BuildObject>();
            _constructors.Add("Castle",   CreateCastle);
            _constructors.Add("Mine",     CreateMine);
            _constructors.Add("Single",   CreateSingle);
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
                        case "mapObject":
                            {
                                _constructors[reader.GetAttribute("type")].Invoke(map, reader);
                            } break;
                        case "basePoint":
                            {
                                int x = Convert.ToInt32(reader.GetAttribute("x"));
                                int y = Convert.ToInt32(reader.GetAttribute("y"));

                                basePoint = new BasePoint();
                                basePoint.Id = basePointGuidGen.Id;
                                basePoint.X = x;
                                basePoint.Y = y;
                                basePoint.LandscapeType = LandscapeTypes.Grass;
                            } break;
                        case "player":
                            {
                                string login = reader.GetAttribute("login");
                                string password = reader.GetAttribute("password");
                                _players.Add(new Identity(login, password));
                            } break;
                        case "players":
                            {
                                //
                            } break;
                    }
                }
            }

            CreateLandscape(map, basePoint);

            return level;
        }

        private static void CreateSingle(Map map, XmlReader reader)
        {
            string type = reader.GetAttribute("stype");

            SingleObjectType objType = SingleObjectType.Tree;

            switch (type)
            {
                case  "Tree":
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
                             WayCost = 4,
                             SingleObjectType = objType,
                         };
           
            map.AddSingleObject(single);
        }

        private static void CreateResource(Map map, XmlReader reader)
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
                               WayCost = 1,
                               ResourceType = objType
                           };

            map.AddResource(resource);
        }

        private static void CreateCastle(Map map, XmlReader reader)
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
                                WayCost = 2
                            };

            FigureStore figures = new FigureStore {Id = ID};
            castle.FigureStore = figures;
            Unit uu = new Unit();
            uu.UnitType = UnitType.Knight;
            uu.UnitCount = 1;
            figures.AddFigureToRepository(uu);

            ResourceStore resources = new ResourceStore {Id = ID};
            castle.ResourceStore = resources;
            Resource rr = new Resource();
            rr.ResourceType = ResourceTypes.Gold;
            rr.CountResource = 500;
            resources.AddResourceToRepository(rr);

            castle.CreatStartArmy();

            Vicegerent vicegerent = new Vicegerent
                                    {
                                        Id = ID,
                                        Name = "Vicegerent"
                                    };

            castle.Vicegerent = vicegerent;

            map.AddCastle(castle);
        }

        private static void CreateMine(Map map, XmlReader reader)
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
                           WayCost = 3,
                           MineType = objType,
                           SizeMine = 100,
                       };
    
            mine.Initialize(ID, map, objType, 100, 100);
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

            if (_players.Exists(x=>x.Login == identity.Login 
                && x.Password == identity.Password))
            {
                int id = GuidGenerator.Instance.GeneratePair().Id;
               
                var player = new Player(level, identity.Login, identity.Password);

                player.Id = id;

                var king = new King
                           {
                               Id = id,
                               Experience = 0,
                               MilitaryRank = 0
                           };

                player.AddKing(king);
                player.Map = player.King.Map = player.Level.Map;

                Castle castle = player.Level.Map.SearchFreeCastle();

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
