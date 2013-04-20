using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessServer.LogicLayer.AI.MonitorLayer
{
    public class AStarNode
    {
        private float f;
        private float g;
        private float h;
        private int x; // координата вершины по x
        private int y; // координата вершины по y
        private int pX; // координата родительской вершины по x
        private int pY; // координата родительской вершины по y

        public float F
        {
            get { return f; }
            set { f = value; }
        }

        public float G
        {
            get { return g; }
            set { g = value; }
        }

        public float H
        {
            get { return h; }
            set { h = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int PX
        {
            get { return pX; }
            set { pX = value; }
        }

        public int PY
        {
            get { return pY; }
            set { pY = value; }
        }

    }
}
