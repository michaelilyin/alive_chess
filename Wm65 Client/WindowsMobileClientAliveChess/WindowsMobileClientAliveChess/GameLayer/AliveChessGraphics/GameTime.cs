using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsMobileClientAliveChess.GameLayer
{
    public class GameTime
    {
        private DateTime _now;
        private DateTime _prev;

        public void Update()
        {
            this.Now = DateTime.Now;
        }

        public void Memorize()
        {
            this.Prev = DateTime.Now;
        }

        public DateTime Now
        {
            get { return _now; }
            set { _now = value; }
        }

        public DateTime Prev
        {
            get { return _prev; }
            set { _prev = value; }
        }

        public TimeSpan Elapsed
        {
            get { return Now - Prev; }
        }
    }
}
