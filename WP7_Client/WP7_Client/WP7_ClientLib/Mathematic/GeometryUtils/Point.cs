using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.Mathematic.GeometryUtils
{
    public class Point
    {
        private int x;
        private int y;

        public int X
        {
            set { x = value;}
            get { return x;}
        }

        public int Y
        {
            set { y = value; }
            get { return y; }
        }
    }
}
