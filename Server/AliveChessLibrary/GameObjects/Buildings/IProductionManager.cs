using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public interface IProductionManager<T>
    {
        Castle Castle { get; set; }

        LinkedList<BuildingQueueItem<T>> GetProductionQueueCopy();

        Dictionary<T, CreationRequirements> CreationRequirements { get; set; }

        CreationRequirements GetCreationRequirements(T type);

        void Update(TimeSpan timeFromLastUpdate);

        void Build(T type);

        void Destroy(T type);

        bool HasInQueue(T type);

        void SetProductionQueue(LinkedList<BuildingQueueItem<T>> queue);
    }
}
