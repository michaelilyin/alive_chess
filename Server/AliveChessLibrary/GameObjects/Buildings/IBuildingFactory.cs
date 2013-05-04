using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public interface IBuildingFactory
    {
        CreationCost GetBuildingCost(InnerBuildingType type);

        void Build(InnerBuildingType type);

        void Destroy(InnerBuildingType type);

        Castle Castle { get; set; }
    }
}
