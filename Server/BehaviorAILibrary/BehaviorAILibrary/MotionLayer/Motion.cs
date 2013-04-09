using System;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;

namespace BehaviorAILibrary.MotionLayer
{
    public class Motion
    {
        public static bool VerifyPath(Map map, List<FPosition> path,
           Func<MapPoint, bool> verifier, out int mistakeNumber)
        {
            mistakeNumber = 0;
            int prevX = (int)path[0].X;
            int prevY = (int)path[0].Y;
            for (int i = 1; i < path.Count; i++)
            {
                int x = (int)path[i].X;
                int y = (int)path[i].Y;
                if (!verifier(map.GetObject(x, y)) || !VerifyDistance(x, y, prevX, prevY))
                    mistakeNumber++;
                prevX = x;
                prevY = y;
            }

            return (mistakeNumber == 0);
        }

        public static bool VerifyPath(Map map, List<FPosition> path,
          Func<MapPoint, bool> verifier, out int mistakeNumber, out FPosition last)
        {
            last = null;
            mistakeNumber = 0;
            int prevX = (int) path[0].X;
            int prevY = (int) path[0].Y;
            bool isLastCreaterd = false;
            for (int i = 1; i < path.Count; i++)
            {
                int x = (int)path[i].X;
                int y = (int)path[i].Y;
                if (!verifier(map.GetObject(x, y)) || !VerifyDistance(x, y, prevX, prevY))
                {
                    mistakeNumber++;
                    if (!isLastCreaterd)
                    {
                        last = path[i - 1];
                        isLastCreaterd = true;
                    }
                }
                prevX = x;
                prevY = y;
            }
            if (!isLastCreaterd)
                last = path[path.Count - 1];

            return (mistakeNumber == 0);
        }

        private static bool VerifyDistance(int x, int y, int prevX, int prevY)
        {
            return Math.Abs(x - prevX) <= 1 && Math.Abs(y - prevY) <= 1;
        }

        public static Queue<FPosition> Transform(List<Position> path)
        {
            Queue<FPosition> positions = new Queue<FPosition>();
            for (int i = 0; i < path.Count; i++)
            {
                positions.Enqueue(new FPosition(path[i].X, path[i].Y));
            }
            return positions;
        }

        public static Queue<FPosition> Transform(List<FPosition> path)
        {
            Queue<FPosition> positions = new Queue<FPosition>();
            for (int i = 0; i < path.Count; i++)
            {
                positions.Enqueue(new FPosition(path[i].X, path[i].Y));
            }
            return positions;
        }
    }
}
