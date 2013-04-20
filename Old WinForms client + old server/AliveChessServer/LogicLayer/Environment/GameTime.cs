using System;

namespace AliveChessServer.LogicLayer.Environment
{
    public class GameTime
    {
        private DateTime _time;

        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
    }
}
