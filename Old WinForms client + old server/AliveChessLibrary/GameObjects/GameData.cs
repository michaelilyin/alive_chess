using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Objects;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChessLibrary.GameObjects
{
    public class GameData
    {
        #region Constants

        private Dictionary<string, int> wayCosts;
        private Dictionary<string, int> _resourcesCount;

        private int kingExperience;

        private const int MAP_OBJECT = 0;
        private const int IMPASSABLE_AREA_WAY_COST = 10;

        private const int OBJECT_TYPE_COUNT = 14;
        private const int LANDSCAPE_TYPE_COUNT = 4;
        private const int RESOURCE_TYPE_COUNT = 5;
        private const int BUILDING_TYPE_COUNT = 2;
        private const int SINGLE_OBJECT_TYPE_COUNT = 2;

        #endregion

        public GameData()
        {
            wayCosts = new Dictionary<string, int>();
            _resourcesCount = new Dictionary<string, int>();

            this.wayCosts.Add(PointTypes.Landscape.ToString(), 0);
            this.wayCosts.Add(PointTypes.Resource.ToString(), 1);
            this.wayCosts.Add(PointTypes.SingleObject.ToString(), 2);
            this.wayCosts.Add(PointTypes.MultyObject.ToString(), 3);
            this.wayCosts.Add(PointTypes.Mine.ToString(), 4);
            this.wayCosts.Add(PointTypes.Castle.ToString(), 5);
            this.wayCosts.Add(PointTypes.King.ToString(), 6);

            LoadXmlContent();
        }

        /// <summary>
        /// получение типа ландшафта по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LandscapeTypes GetLandscapeTypeByName(string name)
        {
            Debug.Assert(name != null);
            for (int index = 0; index < LANDSCAPE_TYPE_COUNT; index++)
                if (((LandscapeTypes)index).ToString().Equals(name))
                    return ((LandscapeTypes)index);
            throw new ApplicationException("Undefinite landscape");
        }

        /// <summary>
        /// получение типа ресурса по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResourceTypes GetResourceTypeByName(string name)
        {
            Debug.Assert(name != null);
            for (int index = 0; index < RESOURCE_TYPE_COUNT; index++)
                if (((ResourceTypes)index).ToString().Equals(name))
                    return ((ResourceTypes)index);
            throw new ApplicationException("Undefinite resource");
        }

        public SingleObjectType GetSingleObjectTypeByName(string name)
        {
            Debug.Assert(name != null);
            for (int index = 0; index < SINGLE_OBJECT_TYPE_COUNT; index++)
                if (((SingleObjectType)index).ToString().Equals(name))
                    return ((SingleObjectType)index);
            throw new ApplicationException("Undefinite object");
        }

        /// <summary>
        /// получение типа строения по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BuildingTypes GetBuildingTypeByName(string name)
        {
            Debug.Assert(name != null);
            for (int index = 0; index < BUILDING_TYPE_COUNT; index++)
                if (((BuildingTypes)index).ToString().Equals(name))
                    return ((BuildingTypes)index);
            throw new ApplicationException("Undefinite building");
        }

        /// <summary>
        /// получение типа объекта по порядковому номеру
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PointTypes GetMapObjectTypeById(int number)
        {
            if (number > -1 && number < OBJECT_TYPE_COUNT)
                return (PointTypes)number;
            else throw new ArgumentOutOfRangeException("Map object index is out of range");
        }

        /// <summary>
        /// получение типа объекта по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PointTypes GetMapObjectTypeByName(string name)
        {
            Debug.Assert(name != null);
            for (int index = 0; index < OBJECT_TYPE_COUNT; index++)
                if (((PointTypes)index).ToString().Equals(name))
                    return ((PointTypes)index);
            return 0;
        }

        public int GetWayCost(PointTypes moType)
        {
            return wayCosts[moType.ToString()];
        }

        private void LoadXmlContent()
        {
            string content = AliveChessLibrary.Properties.Resources.AC_XML;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            foreach (XmlNode n in doc.ChildNodes)
            {
                if (n.Name=="GameObjects")
                {
                    XmlNodeList nList = n.ChildNodes;
                    foreach (XmlNode nListNode in nList)
                    {
                        switch (nListNode.Name)
                        {
                            case "MapObjects":
                                LoadMapObjects(nListNode);
                                break;
                            case "AliveObjects":
                                LoadAliveObjects(nListNode);
                                break;
                            case "Resources":
                                LoadResources(nListNode);
                                break;
                        }
                    }
                }
            }
        }

        private void LoadAliveObjects(XmlNode node)
        {
            XmlNode n = node.ChildNodes[0];
            kingExperience = Convert.ToInt32(n.Attributes["Experience"].Value);
        }

        private void LoadMapObjects(XmlNode mObject)
        {
            foreach (XmlNode n in mObject.ChildNodes)
                wayCosts.Add(n.Name, Convert.ToInt32(n.Attributes["WayCost"].Value));
        }

        private void LoadResources(XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
                _resourcesCount.Add(n.Name, Convert.ToInt32(n.Attributes["Count"].Value));
        }

        public int GetResourceCount(ResourceTypes rType)
        {
            return _resourcesCount.ContainsKey(rType.ToString()) ? _resourcesCount[rType.ToString()] : -1;
        }

        public int Impassable
        {
            get { return IMPASSABLE_AREA_WAY_COST; }
        }
    }
}
