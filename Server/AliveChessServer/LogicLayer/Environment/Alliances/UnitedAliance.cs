using System.Collections.Generic;

namespace AliveChessServer.LogicLayer.Environment.Alliances
{
    public class UnitedAliance
    {
        private List<Empire> _emperies;

        private object _emperiesSync = new object();

        public UnitedAliance()
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
