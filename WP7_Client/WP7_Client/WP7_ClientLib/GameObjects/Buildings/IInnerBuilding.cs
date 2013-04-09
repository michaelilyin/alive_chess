using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public interface IInnerBuilding//Slisarenko
    {
        Unit CreateUnit(int count, UnitType type);

        int Id { get; set; }

        string Name { get; set; }

        Castle Castle { get; set; }

        UnitType ProducedUnitType { get; set; }

        InnerBuildingType InnerBuildingType { get; set; }

        int ResourceCountToBuild { get; set; }

        ResourceTypes ResourceTypeToBuild { get; set; }

        int ResourceCountToProduceUnit { get; set; }

        ResourceTypes ResourceTypeToProduceUnit { get; set; }
    }
}
