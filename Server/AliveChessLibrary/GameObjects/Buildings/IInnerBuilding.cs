using System;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public interface IInnerBuilding
    {
        int Id { get; set; }

        Castle Castle { get; set; }

        InnerBuildingType InnerBuildingType { get; set; }
    }
}
