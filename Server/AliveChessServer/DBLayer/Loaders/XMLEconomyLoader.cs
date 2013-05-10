using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessServer.LogicLayer.EconomyEngine;
using AliveChessServer.LogicLayer.Environment;

namespace AliveChessServer.DBLayer.Loaders
{
    public class XMLEconomyLoader : IEconomyLoader
    {
        public Economy LoadEconomy(LevelTypes levelType)
        {
            XmlDocument myXmlDocument = new XmlDocument();
            myXmlDocument.Load("..\\XML\\Economy.xml");

            XmlNode document = myXmlDocument.DocumentElement;
            AliveChessLibrary.DebugConsole.WriteLine(this, document.Name);
            Economy economy = new Economy();
            XmlNode node = document.FirstChild;
            while (node != null)
            {
                AliveChessLibrary.DebugConsole.WriteLine(this, node.Name + " " + node.NodeType);
                switch (node.Name)
                {
                    case "unit":
                        ReadUnit(node, economy);
                        break;
                    case "building":
                        ReadBuilding(node, economy);
                        break;
                }
                node = node.NextSibling;
            }
            return economy;
        }

        private void ReadUnit(XmlNode node, Economy economy)
        {
            XmlNamedNodeMap attributes = node.Attributes;
            UnitType type = UnitType.Pawn;
            if (attributes != null)
            {
                XmlNode typeNode = attributes.GetNamedItem("type");
                if (typeNode != null)
                {
                    type = _getUnitType(typeNode.Value);
                }/*
                foreach (XmlNode attrnode in attributes)
                {
                    switch (attrnode.Name)
                    {
                        case "type":
                            type = _getUnitType(attrnode.Value);
                            break;
                    }
                }*/
            }
            economy.SetCreationRequirements(type, _readCreationRequirements(node));
        }

        private void ReadBuilding(XmlNode node, Economy economy)
        {
            XmlNamedNodeMap attributes = node.Attributes;
            InnerBuildingType type = InnerBuildingType.Quarters;
            if (attributes != null)
            {
                XmlNode typeNode = attributes.GetNamedItem("type");
                if (typeNode != null)
                {
                    type = _getBuildingType(typeNode.Value);
                }
            }
            economy.SetCreationRequirements(type, _readCreationRequirements(node));
        }

        private UnitType _getUnitType(string type)
        {
            switch (type)
            {
                case "Pawn":
                    return UnitType.Pawn;
                case "Bishop":
                    return UnitType.Bishop;
                case "Knight":
                    return UnitType.Knight;
                case "Rook":
                    return UnitType.Rook;
                case "Queen":
                    return UnitType.Queen;
            }
            return UnitType.Pawn;
        }

        private ResourceTypes _getResourceType(string type)
        {
            switch (type)
            {
                case "Gold":
                    return ResourceTypes.Gold;
                case "Wood":
                    return ResourceTypes.Wood;
                case "Stone":
                    return ResourceTypes.Stone;
                case "Iron":
                    return ResourceTypes.Iron;
                case "Coal":
                    return ResourceTypes.Coal;
            }
            return ResourceTypes.Gold;
        }

        private InnerBuildingType _getBuildingType(string type)
        {
            switch (type)
            {
                case "Quarters":
                    return InnerBuildingType.Quarters;
                case "TrainingGround":
                    return InnerBuildingType.TrainingGround;
                case "Stabling":
                    return InnerBuildingType.Stabling;
                case "Workshop":
                    return InnerBuildingType.Workshop;
                case "RoyalGuardQuarters":
                    return InnerBuildingType.RoyalGuardQuarters;
                case "Forge":
                    return InnerBuildingType.Forge;
                case "Hospital":
                    return InnerBuildingType.Hospital;
            }
            return InnerBuildingType.Quarters;
        }



        private CreationRequirements _readCreationRequirements(XmlNode node)
        {
            CreationRequirements requirements = new CreationRequirements();
            if (node.HasChildNodes)
            {
                XmlNode requirementNode = node.FirstChild;
                while (requirementNode != null)
                {
                    switch (requirementNode.Name)
                    {
                        case "resource":
                            {
                                ResourceTypes type = ResourceTypes.Gold;
                                XmlNamedNodeMap attributes = requirementNode.Attributes;
                                if (attributes != null)
                                {
                                    XmlNode typeNode = attributes.GetNamedItem("type");
                                    if (typeNode != null)
                                    {
                                        type = _getResourceType(typeNode.Value);
                                    }
                                }
                                int value = Convert.ToInt32(requirementNode.InnerText);
                                requirements.Resources[type] = value;
                                break;
                            }
                        case "time":
                            double time = Convert.ToDouble(requirementNode.InnerText);
                            requirements.CreationTime = time;
                            break;
                        case "requiredBuilding":
                            InnerBuildingType buildingType = _getBuildingType(requirementNode.InnerText);
                            requirements.RequiredBuildings.Add(buildingType);
                            break;
                    }
                    requirementNode = requirementNode.NextSibling;
                }
            }
#if DEBUG
            string t = "unknown";
            if(node.Attributes != null && node.Attributes.GetNamedItem("type") != null);
                t = node.Attributes.GetNamedItem("type").Value;
            AliveChessLibrary.DebugConsole.WriteLine(this, "Required for " + node.Name + " \"" + t + "\":");
            foreach (var VARIABLE in requirements.Resources)
            {
                AliveChessLibrary.DebugConsole.WriteLine(this, "Required resource: " + VARIABLE.Key.ToString() + " " + VARIABLE.Value.ToString());
            }
            foreach (var VARIABLE in requirements.RequiredBuildings)
            {
                AliveChessLibrary.DebugConsole.WriteLine(this, "Required building: " + VARIABLE.ToString());
            }
            AliveChessLibrary.DebugConsole.WriteLine(this, "Required time: " + requirements.CreationTime);
#endif
            return requirements;
        }
    }
}
