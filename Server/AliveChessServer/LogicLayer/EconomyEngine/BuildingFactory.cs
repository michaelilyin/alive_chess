using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessServer.DBLayer;

namespace AliveChessServer.LogicLayer.EconomyEngine
{
    public class BuildingFactory : IBuildingFactory
    {
        private Castle _castle;
        private Economy _economy;

        public BuildingFactory(Economy economy)
        {
            _economy = economy;
        }

        public CreationRequirements GetCreationRequirements(InnerBuildingType type)
        {
            return _economy.GetCreationRequirements(type);
        }

        public void Build(InnerBuildingType type)
        {
            if(_castle.HasBuilding(type))
                return;
            CreationRequirements requirements = _economy.GetCreationRequirements(type);
            if(requirements == null)
                return;
            if (!_castle.King.ResourceStore.HaveEnoughResources(requirements.Resources))
                return;
            foreach (var b in requirements.RequiredBuildings)
            {
                if(!_castle.HasBuilding(b))
                    return;
            }
            _castle.King.ResourceStore.TakeResources(requirements.Resources);
            InnerBuilding building = new InnerBuilding();
            building.Id = GuidGenerator.Instance.GeneratePair().Id;
            building.InnerBuildingType = type;
            _castle.AddBuilding(building);

        }

        public void Destroy(InnerBuildingType type)
        {
            throw new NotImplementedException();
        }

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }
    }
}
