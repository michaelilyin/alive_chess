using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessServer.LogicLayer.EconomyEngine
{
    public class RecruitingManager : ProductionManager<UnitType>
    {
        public RecruitingManager(Economy economy)
            : base(economy)
        {
        }

        public override Dictionary<UnitType, CreationRequirements> CreationRequirements
        {
            get
            {
                return _economy.RecruitingRequirements;
            }
            set
            {
                _economy.RecruitingRequirements = value;
            }
        }

        protected override void _finishProduction(UnitType type)
        {
            _castle.Army.AddUnit(type, 1);
        }

        public override void Destroy(UnitType type)
        {
            BuildingQueueItem<UnitType> item = _getUnfinishedItem(type);
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
        }
    }
}
