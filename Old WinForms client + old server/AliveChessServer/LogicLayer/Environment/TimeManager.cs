using System;

namespace AliveChessServer.LogicLayer.Environment
{
    public class TimeManager
    {
        private GameTime _time;
        private TimeSpan _timeSpan;
        private Boolean _isUpdated;

        public TimeManager()
        {
            _time = new GameTime();
            _timeSpan = TimeSpan.Zero;
        }

        public GameTime Time
        {
            get
            {
                _isUpdated = true;
                _time.Time = DateTime.Now;
                return _time;
            }
        }

        public Boolean IsUpdated
        {
            get { return _isUpdated; }
        }
    }
}
