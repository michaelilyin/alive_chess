using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Buildings;

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

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }
    }
}
