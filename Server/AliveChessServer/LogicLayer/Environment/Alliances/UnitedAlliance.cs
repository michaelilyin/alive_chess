using System.Collections.Generic;

namespace AliveChessServer.LogicLayer.Environment.Alliances
{
    public class UnitedAlliance
    {
        private List<Empire> _emperies;

        private object _emperiesSync = new object();

        public UnitedAlliance()
        {
            this._emperies = new List<Empire>();
        }

        public void AddEmperie(Empire e)
        {
            lock (_emperiesSync)
                _emperies.Add(e);
        }
    }
}
