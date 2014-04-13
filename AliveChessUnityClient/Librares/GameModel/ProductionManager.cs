using AliveChessLibrary.GameObjects.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.CastleManagers
{
    public class ProductionManager<T> : IProductionManager<T>
    {
        private Castle _castle;
        private LinkedList<BuildingQueueItem<T>> _productionQueue = new LinkedList<BuildingQueueItem<T>>();
        private Dictionary<T, CreationRequirements> _creationRequirements = new Dictionary<T, CreationRequirements>();

        public LinkedList<BuildingQueueItem<T>> GetProductionQueueCopy()
        {
            LinkedList<BuildingQueueItem<T>> result = new LinkedList<BuildingQueueItem<T>>();
            lock (_productionQueue)
            {
                foreach (var buildingQueueItem in _productionQueue)
                {
                    BuildingQueueItem<T> item = new BuildingQueueItem<T>();
                    item.RemainingCreationTime = buildingQueueItem.RemainingCreationTime;
                    item.TotalCreationTime = buildingQueueItem.TotalCreationTime;
                    item.Type = buildingQueueItem.Type;
                    result.AddLast(item);
                }
            }
            return result;
        }

        public CreationRequirements GetCreationRequirements(T type)
        {
            foreach (var creationRequirements in _creationRequirements)
            {
                if (creationRequirements.Key.Equals(type))
                    return creationRequirements.Value;
            }
            return null;
        }

        public void Update(TimeSpan timeFromLastUpdate)
        {
            throw new NotImplementedException();
        }

        public void Build(T type)
        {
            throw new NotImplementedException();
        }

        public void Destroy(T type)
        {
            throw new NotImplementedException();
        }

        public bool HasInQueue(T type)
        {
            lock (_productionQueue)
            {
                return _productionQueue.Any(buildingQueueItem => buildingQueueItem.Type.Equals(type));
            }
        }

        public void SetProductionQueue(LinkedList<BuildingQueueItem<T>> queue)
        {
            lock (_productionQueue)
            {
                if (queue != null)
                {
                    _productionQueue = queue;
                }
                else
                {
                    _productionQueue.Clear();
                }
            }
        }

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

        public Dictionary<T, CreationRequirements> CreationRequirements
        {
            get { return _creationRequirements; }
            set { _creationRequirements = value; }
        }
    }
}
