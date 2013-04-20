namespace AliveChessServer.LogicLayer.AI.MotionLayer
{
    // Node class (class that has x, y, heuristic and parent pointer values
    //public class Node
    //{
    //    private float g = 0;
    //    public Node Parent { get; set; }

    //    // cost of this node + its parents
    //    public float G
    //    {
    //        get
    //        {
    //            return this.g + (float)this.WayCost;
    //        }
    //        set
    //        {
    //            this.g = value;
    //        }
    //    }
    //    public float H { get; set; } // heuristic estimate
    //    public float F
    //    {
    //        get
    //        {
    //            return this.G + this.H;
    //        }
    //        //set;
    //    } // g + h

    //    public int X { get; set; }
    //    public int Y { get; set; }
    //    public float WayCost { get; set; }

    //    public Node()
    //    { }

    //    /*****DistanceEstimate()*****
    //    *TAKES: int endx:	x value of the endNode
    //    *		int endy:	y value of the endNode
    //    *RETURNS: float:	the value of the distance equation squared
    //    *PURPOSE: to calculate and return the distance from this
    //    *node to the endNode
    //    ********************************/
    //    public float DistanceEstimate(int endx, int endy)
    //    {
    //        float dx = (float)(X - (float)endx);
    //        float dy = (float)(Y - (float)endy);

    //        return (float)((dx * dx) + (dy * dy));
    //        //return dx + dy;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (obj is Node)
    //        {
    //            Node rhs = obj as Node;
    //            if (X == rhs.X && Y == rhs.Y)
    //                return true;
    //        }
    //        return false;
    //    }
    //    public override int GetHashCode()
    //    {
    //        return base.GetHashCode();
    //    }
    //}

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
