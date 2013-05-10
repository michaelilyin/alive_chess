using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public interface IBuildingManager
    {
        Castle Castle { get; set; }

        LinkedList<BuildingQueueItem<InnerBuildingType>> BuildingQueue { get; set; }

        Dictionary<InnerBuildingType, CreationRequirements> CreationRequirements { get; set; }

        CreationRequirements GetCreationRequirements(InnerBuildingType type);

        void Update(TimeSpan timeFromLastUpdate);

        void Build(InnerBuildingType type);

        void Destroy(InnerBuildingType type);

        bool HasUnfinishedBuilding(InnerBuildingType type);
    }
}
