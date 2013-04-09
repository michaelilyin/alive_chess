using System;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public class InnerBuildingFactory//Slisarenko
    {
        public InnerBuilding Build(Guid guid, int id, InnerBuildingType type, string name)
        {
            int resourceCountToBuild = 10;
            int resourceCountToProduceUnit = 10;
            ResourceTypes resourceTypeToBuild = ResourceTypes.Gold;
            ResourceTypes resourceTypeToProduceUnit = ResourceTypes.Gold;
            InnerBuilding ib = new InnerBuilding();
            //ib.Id = id;
            //ib.Id = guid;
            ib.Name = name;
            ib.ResourceCountToBuild = resourceCountToBuild;
            ib.ResourceCountToProduceUnit = resourceCountToProduceUnit;
            ib.ResourceTypeToBuild = resourceTypeToBuild;
            ib.ResourceTypeToProduceUnit = resourceTypeToProduceUnit;
            return ib;
        }
    }
}
