using System.Collections.Generic;
using System.Drawing;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;

namespace BehaviorAILibrary.MotionLayer
{
    public class PathPlanner
    {
        private AStar astar;
        private IDictionary<PathKey, IList<Position>> calculatedPaths = new Dictionary<PathKey, IList<Position>>();

        public PathPlanner(ILocalizable place)
        {
            astar = new AStar(place);
        }

        public IList<Position> FindPath(Point start, Point end)
        {
            var pKey = new PathKey(start, end);
            if (!calculatedPaths.ContainsKey(pKey))
            {
                astar.SetStartEnd(start, end);
                IList<Position> path = astar.FindPath();
                if (path != null)
                {
                    calculatedPaths.Add(new PathKey(start, end), path);
                    return path;
                }
            }
            else
            {
                return calculatedPaths[pKey];
            }
            return null;
        }

        class PathKey
        {
            public Point Start { get; set; }
            public Point End { get; set; }
            public override bool Equals(object obj)
            {
                if (obj is PathKey)
                {
                    PathKey pKey = obj as PathKey;
                    if (pKey.Start.Equals(this.Start) && pKey.End.Equals(this.End))
                    {
                        return true;
                    }
                }
                return false;
            }
            public PathKey(Point start, Point end)
            {
                this.Start = start;
                this.End = end;
            }
        }
    }// END CLASS DEFINITION PathPlanner

}
