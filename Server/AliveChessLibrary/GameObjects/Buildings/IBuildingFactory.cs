using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public interface IBuildingFactory
    {
        CreationRequirements GetCreationRequirements(InnerBuildingType type);

        void Build(InnerBuildingType type);

        void Destroy(InnerBuildingType type);

        Castle Castle { get; set; }
    }
}
