using System.Collections.Generic;
using System.Threading;

namespace AliveChessServer.LogicLayer.Environment
{
    public class TimeManager
    {
        private const int Interval = 10;
        private List<GameTime> _times;
        private object _timesSync = new object();
       
        public TimeManager()
        {
            _times = new List<GameTime>();
        }

        public void AddTime(GameTime time)
        {
            lock (_timesSync)
                _times.Add(time);
        }

        public void RemoveTime(GameTime time)
        {
            lock (_timesSync)
                _times.Remove(time);
        }

        public void Update()
        {
            for (int i = 0; i < _times.Count; i++)
                _times[i].Update();

            Thread.Sleep(Interval);
        }
    }
}
