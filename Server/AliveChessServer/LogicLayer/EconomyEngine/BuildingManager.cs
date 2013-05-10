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
    public class BuildingManager : IBuildingManager
    {
        private Castle _castle;
        private Economy _economy;
        private LinkedList<BuildingQueueItem<InnerBuildingType>> _buildingQueue = new LinkedList<BuildingQueueItem<InnerBuildingType>>();

        public BuildingManager(Economy economy)
        {
            _economy = economy;
        }

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

        public LinkedList<BuildingQueueItem<InnerBuildingType>> BuildingQueue
        {
            get { return _buildingQueue; }
            set { _buildingQueue = value; }
        }

        public CreationRequirements GetCreationRequirements(InnerBuildingType type)
        {
            return _economy.GetCreationRequirements(type);
        }

        public void Update(TimeSpan timeFromLastUpdate)
        {
            if (_buildingQueue.Count > 0)
            {
                if (_buildingQueue.First().RemainingCreationTime <= timeFromLastUpdate.TotalSeconds)
                {
                    BuildingQueueItem<InnerBuildingType> item = _buildingQueue.First();
                    _buildingQueue.Remove(item);
                    InnerBuilding building = new InnerBuilding();
                    building.Id = GuidGenerator.Instance.GeneratePair().Id;
                    building.InnerBuildingType = item.Type;
                    _castle.AddBuilding(building);
                    if (_buildingQueue.Count > 0)
                    {
                        _buildingQueue.First().RemainingCreationTime -= timeFromLastUpdate.TotalSeconds - item.RemainingCreationTime;
                    }
                }
                else
                {
                    _buildingQueue.First().RemainingCreationTime -= timeFromLastUpdate.TotalSeconds;
                }
            }
        }

        public void Build(InnerBuildingType type)
        {
            CreationRequirements requirements = _economy.GetCreationRequirements(type);
            _castle.King.ResourceStore.TakeResources(requirements.Resources);
            _buildingQueue.AddLast(new BuildingQueueItem<InnerBuildingType>(type, requirements.CreationTime));
            /*InnerBuilding building = new InnerBuilding();
            building.Id = GuidGenerator.Instance.GeneratePair().Id;
            building.InnerBuildingType = type;
            _castle.AddBuilding(building);*/

        }

        public void Destroy(InnerBuildingType type)
        {
            BuildingQueueItem<InnerBuildingType> item = _getUnfinishedBuilding(type);
            if (item != null)
            {
                _buildingQueue.Remove(item);
                CreationRequirements requirements = _economy.GetCreationRequirements(type);
                //Возврат части ресурсов
                foreach (var resItem in requirements.Resources)
                {
                    _castle.King.ResourceStore.AddResource(resItem.Key, (int) (resItem.Value * (0.5 * (item.RemainingCreationTime / item.TotalCreationTime + 1))));
                }
            }
            else if (_castle.HasBuilding(type))
            {
                _castle.DestroyBuilding(type);
                CreationRequirements requirements = _economy.GetCreationRequirements(type);
                //Возврат части ресурсов
                foreach (var resItem in requirements.Resources)
                {
                    _castle.King.ResourceStore.AddResource(resItem.Key, resItem.Value / 2);
                }
            }
        }

        public bool HasUnfinishedBuilding(InnerBuildingType type)
        {
            return _buildingQueue.Any(buildingQueueItem => buildingQueueItem.Type == type);
        }

        public BuildingQueueItem<InnerBuildingType> _getUnfinishedBuilding(InnerBuildingType type)
        {
            return _buildingQueue.FirstOrDefault(buildingQueueItem => buildingQueueItem.Type == type);
        }
    }
}
