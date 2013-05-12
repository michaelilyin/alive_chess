using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessServer.DBLayer;

namespace AliveChessServer.LogicLayer.EconomyEngine
{
    public class BuildingManager : ProductionManager<InnerBuildingType>
    {
        public BuildingManager(Economy economy)
            : base(economy)
        {
        }

        public override Dictionary<InnerBuildingType, CreationRequirements> CreationRequirements
        {
            get { return _economy.BuildingRequirements; }
            set { _economy.BuildingRequirements = value; }
        }

        protected override void _finishProduction(InnerBuildingType type)
        {
            InnerBuilding building = new InnerBuilding();
            building.Id = GuidGenerator.Instance.GeneratePair().Id;
            building.InnerBuildingType = type;
            _castle.AddBuilding(building);
        }

        public override void Destroy(InnerBuildingType type)
        {
            BuildingQueueItem<InnerBuildingType> item = _getUnfinishedItem(type);
            if (item != null)
            {
                lock (_productionQueue)
                {
                    _productionQueue.Remove(item);
                }
                CreationRequirements requirements = _economy.GetCreationRequirements(type);
                //Возврат части ресурсов
                foreach (var resItem in requirements.Resources)
                {
                    _castle.King.ResourceStore.AddResource(resItem.Key, (int)(resItem.Value * (0.5 * (item.RemainingCreationTime / item.TotalCreationTime + 1))));
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
    }
}
