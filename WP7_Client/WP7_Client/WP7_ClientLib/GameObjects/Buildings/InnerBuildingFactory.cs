using System;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public class InnerBuildingFactory//Slisarenko
    {
        public InnerBuilding Build(Guid guid, int id, InnerBuildingType type, string name)
        {
            const int resourceCountToBuild = 10;
            const int resourceCountToProduceUnit = 10;
            const ResourceTypes resourceTypeToBuild = ResourceTypes.Gold;
            const ResourceTypes resourceTypeToProduceUnit = ResourceTypes.Gold;
            var ib = new InnerBuilding
                         {
                             Name = name,
                             ResourceCountToBuild = resourceCountToBuild,
                             ResourceCountToProduceUnit = resourceCountToProduceUnit,
                             ResourceTypeToBuild = resourceTypeToBuild,
                             ResourceTypeToProduceUnit = resourceTypeToProduceUnit
                         };
            return ib;
        }
    }
}
