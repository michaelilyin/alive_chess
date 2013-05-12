using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessServer.DBLayer;
using AliveChessServer.LogicLayer.Environment;

namespace AliveChessServer.LogicLayer.EconomyEngine
{
    public abstract class ProductionManager<T> : IProductionManager<T>
    {
        protected Castle _castle;
        protected Economy _economy;
        protected LinkedList<BuildingQueueItem<T>> _productionQueue = new LinkedList<BuildingQueueItem<T>>();

        public ProductionManager(Economy economy)
        {
            _economy = economy;
        }

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

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

        public virtual Dictionary<T, CreationRequirements> CreationRequirements
        {
            get { return null; }
            set { }
        }

        public CreationRequirements GetCreationRequirements(T type)
        {
            return CreationRequirements[type];
        }

        public void Update(TimeSpan timeFromLastUpdate)
        {
            lock (_productionQueue)
            {
                if (_productionQueue.Count > 0)
                {
                    if (_productionQueue.First().RemainingCreationTime <= timeFromLastUpdate.TotalSeconds)
                    {
                        BuildingQueueItem<T> item = _productionQueue.First();
                        _productionQueue.Remove(item);
                        _finishProduction(item.Type);
                        if (_productionQueue.Count > 0)
                        {
                            _productionQueue.First().RemainingCreationTime -= timeFromLastUpdate.TotalSeconds - item.RemainingCreationTime;
                        }
                    }
                    else
                    {
                        _productionQueue.First().RemainingCreationTime -= timeFromLastUpdate.TotalSeconds;
                    }
                }
            }
        }

        protected abstract void _finishProduction(T type);

        public void Build(T type)
        {
            CreationRequirements requirements = GetCreationRequirements(type);
            lock (_castle.King.ResourceStore)
            {
                if (_castle.King.ResourceStore.HasEnoughResources(requirements.Resources))
                {
                    _castle.King.ResourceStore.TakeResources(requirements.Resources);
                    lock (_productionQueue)
                    {
                        _productionQueue.AddLast(new BuildingQueueItem<T>(type, requirements.CreationTime));
                    }
                }
            }
        }

        public abstract void Destroy(T type);

        public bool HasInQueue(T type)
        {
            lock (_productionQueue)
            {
                return _productionQueue.Any(productionQueueItem => productionQueueItem.Type.Equals(type));
            }
        }

        public void SetProductionQueue(LinkedList<BuildingQueueItem<T>> queue)
        {
            lock (_productionQueue)
            {
                _productionQueue = queue;
            }
        }

        protected BuildingQueueItem<T> _getUnfinishedItem(T type)
        {
            lock (_productionQueue)
            {
                return _productionQueue.LastOrDefault(productionQueueItem => productionQueueItem.Type.Equals(type));
            }
        }
    }
}
