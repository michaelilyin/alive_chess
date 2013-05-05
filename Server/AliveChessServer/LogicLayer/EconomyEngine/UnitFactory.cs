using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessServer.LogicLayer.EconomyEngine
{
    public class UnitFactory : IUnitFactory
    {
        private Castle _castle;
        private Economy _economy;

        public UnitFactory(Economy economy)
        {
            _economy = economy;
        }

        public CreationRequirements GetCreationRequirements(UnitType type)
        {
            return _economy.GetCreationRequirements(type);
        }

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }
    }
}
