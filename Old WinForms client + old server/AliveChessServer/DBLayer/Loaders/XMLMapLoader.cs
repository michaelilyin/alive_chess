using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Objects;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Utility;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.NetLayer;

namespace AliveChessServer.DBLayer.Loaders
{
    public class XMLMapLoader
    {
        private GameWorld environment;
        private GameData context;
        private EconomyRoutine ecomonyRoutine;
        private Dictionary<string, BuildObject> constructors;

        public XMLMapLoader(GameWorld environment, GameData context)
        {
            Debug.Assert(environment.EconomyRoutine != null);

            this.context = context;
            this.environment = environment;
            this.ecomonyRoutine = environment.EconomyRoutine;

            this.constructors = new Dictionary<string, BuildObject>();
            constructors.Add("Castle", new BuildObject(CreateCastle));
            constructors.Add("Mine", new BuildObject(CreateMine));
            constructors.Add("Single", new BuildObject(CreateSingle));
            constructors.Add("Resource", new BuildObject(CreateResource));
        }

        public Map Load()
        {
            Map m = null;
            XmlTextReader r = new XmlTextReader("map_xml_adv.xml");
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element)
                {
                    switch (r.Name)
                    {
                        case "map":
                            {
                                GuidIDPair g = GuidGenerator.Instance.GeneratePair();
                                m = new Map(
                                    Convert.ToInt32(r.GetAttribute("sizeX")),
                                    Convert.ToInt32(r.GetAttribute("sizeY")));
                                break;
                            }
                        case "mapobject":
                            {
                                constructors[r.GetAttribute("type")].Invoke(m, r);
                            } break;
                    }
                }
            }

            CreateLandscape(m);

            return m;
        }

        private void CreateSingle(Map map, XmlReader reader)
        {
            SingleObject single = new SingleObject();
            SingleObjectType sType = context.GetSingleObjectTypeByName(reader.GetAttribute("stype"));

            int x = Convert.ToInt32(reader.GetAttribute("x"));
            int y = Convert.ToInt32(reader.GetAttribute("y"));
            int imgId = Convert.ToInt32(reader.GetAttribute("imgId"));

            ImageInfo image = new ImageInfo();
            image.ImageId = imgId;

            GuidIDPair id = GuidGenerator.Instance.GeneratePair();
            MapPoint mapObject = Map.CreatePoint(x, y, image, PointTypes.SingleObject,
                null, context.GetWayCost(PointTypes.SingleObject));

            single.Initialize(id.Guid, map);
            single.AddView(mapObject);
            map.AddSingleObject(single);
        }

        private void CreateResource(Map map, XmlReader reader)
        {
            Resource resource = new Resource();
            ResourceTypes rType = context.GetResourceTypeByName(reader.GetAttribute("rtype"));

            int x = Convert.ToInt32(reader.GetAttribute("x"));
            int y = Convert.ToInt32(reader.GetAttribute("y"));
            int imgId = Convert.ToInt32(reader.GetAttribute("imgId"));

            GuidIDPair r_id = GuidGenerator.Instance.GeneratePair();
            GuidIDPair mp_id = GuidGenerator.Instance.GeneratePair();

            ImageInfo underImg = new ImageInfo();
            underImg.ImageId = 0;

            ImageInfo image = new ImageInfo();
            image.ImageId = imgId;

            MapPoint under = Map.CreatePoint(x, y, underImg, PointTypes.Landscape,
               null, context.GetWayCost(PointTypes.Landscape));

            MapPoint mapObject = Map.CreatePoint(mp_id.Id, x, y, image, PointTypes.Resource,
               under, context.GetWayCost(PointTypes.Resource));

            resource.Id = r_id.Id;
       
            resource.Initialize(map, context, rType);
            resource.CountResource = context.GetResourceCount(rType);

            resource.AddView(mapObject);
            map.AddResource(resource);
        }

        private void CreateCastle(Map map, XmlReader reader)
        {
            Castle castle = new Castle();
           
            GuidIDPair c_id = GuidGenerator.Instance.GeneratePair();
            GuidIDPair mp_id = GuidGenerator.Instance.GeneratePair();

            int leftX = Convert.ToInt32(reader.GetAttribute("centerX")); // получаем координаты
            int topY = Convert.ToInt32(reader.GetAttribute("centerY"));
            int imgId = Convert.ToInt32(reader.GetAttribute("imgId")); // получаем id картинки

            ImageInfo image = new ImageInfo();
            image.ImageId = imgId;
            image.Width = 1;
            image.Height = 1;

            MapSector sector = Map.CreateSector(mp_id.Id, new Position(leftX, topY), image, PointTypes.Castle,
                                                context.GetWayCost(PointTypes.Castle));
            map.InitializeSector(sector);

            // инициализируем замок
            castle.Initialize(c_id.Guid, map, context, 
                delegate() { return GuidGenerator.Instance.GeneratePair(); });
            castle.AddView(sector);

            castle.Id = c_id.Id;

            FigureStore store = new FigureStore();
            //store.Id = id.Id;
            //store.Id = id.Guid;
            castle.FigureStore = store;

            castle.CreatStartArmy(delegate() { return GuidGenerator.Instance.GeneratePair(); });

            // создаем наместника
            Vicegerent vic = new Vicegerent();
            //vic.Id = id.Id;
            //vic.DbId = id.Guid;
            vic.Name = "Vicegerent";
            castle.Vicegerent = vic;

            ResourceStore vv = new ResourceStore();
            //vv.Id = id.Id;
            //vv.Id = id.Guid;
            castle.ResourceStore = vv;

            // добавляем замок на карту
            map.AddCastle(castle);
        }

        private void CreateMine(Map map, XmlReader reader)
        {
            Mine mine = new Mine();
            ResourceTypes res = ResourceTypes.Gold; // тип ресурса по - умолчанию
            PointTypes rt = PointTypes.Mine; // тип объекта - неопределен
            string rtype = reader.GetAttribute("rtype").ToString(); // определяем тип ресурса
            // инициализируем тип ресурса и тип объекта на карте
            switch (rtype)
            {
                case "Gold":
                    {
                        res = ResourceTypes.Gold;
                        break;
                    }
                case "Wood":
                    {
                        res = ResourceTypes.Wood;
                        break;
                    }
            }

            // проверяем корректность инициализации
            Debug.Assert(rt != 0);

            GuidIDPair m_id = GuidGenerator.Instance.GeneratePair();
            GuidIDPair mp_id = GuidGenerator.Instance.GeneratePair();

            int leftX = Convert.ToInt32(reader.GetAttribute("centerX")); // получаем координаты
            int topY = Convert.ToInt32(reader.GetAttribute("centerY"));
            int imgId = Convert.ToInt32(reader.GetAttribute("imgId")); // получаем id картинки

            ImageInfo image = new ImageInfo();
            image.ImageId = imgId;
            image.Width = 1;
            image.Height = 1;

            MapSector sector = Map.CreateSector(mp_id.Id, new Position(leftX, topY), image, PointTypes.Mine,
                                                context.GetWayCost(PointTypes.Mine));
            map.InitializeSector(sector);

            mine.Initialize(m_id.Guid, context, map, res, 100, 10);
            mine.AddView(sector);
            // инициализируем шахту
            mine.MineType = res;
            //mine.AddView(sector);
            mine.Id = m_id.Id;

            // назначаем обработчик события на создание ресурса
            mine.GetResourceEvent += new Mine.GetResourceHandler(ecomonyRoutine.SendResource);
            // добавляем шахту на карту
            map.AddMine(mine);
        }

        private void CreateLandscape(Map map)
        {
            ImageInfo image = new ImageInfo();
            image.ImageId = 0;

            GuidIDPair id = GuidGenerator.Instance.GeneratePair();
            BasePoint basePoint = new BasePoint();
            basePoint.LandscapeType = LandscapeTypes.Grass;
            basePoint.ViewOnMap = Map.CreatePoint(10, 10, image, PointTypes.Landscape, null, 0);
            LandscapeTypes lt = basePoint.LandscapeType;
            FloodFillAlgorithm fa = new FloodFillAlgorithm(map, basePoint, lt, environment.LevelManager.GameData);
            fa.Run(10, 10);

            for (int k = 0; k < map.SizeX; k++)
                for (int j = 0; j < map.SizeY; j++)
                    if (map[k, j] == null)
                        map.SetObject(Map.CreatePoint(k, j, image, PointTypes.Landscape, null, 0));

            map.BasePoints.Add(basePoint);
        }

        public GameData Context
        {
            get { return context; }
        }

        private delegate void BuildObject(Map map, XmlReader reader);
    }
}
