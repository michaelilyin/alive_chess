using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Buildings;

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

        public CreationCost GetBuildingCost(InnerBuildingType type)
        {
            return _economy.GetBuildingCost(type);
        }

        public void Build(InnerBuildingType type)
        {
            throw new NotImplementedException();
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
