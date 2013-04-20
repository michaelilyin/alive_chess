using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AliveChessLibrary.Mathematic.GeometryUtils;

namespace AliveChessServer.LogicLayer.AI.MotionLayer
{
    public static class Direction
    {
        private readonly static Vector2D[,] _directions;
        private readonly static List<Vector2D> _availableDirections;
        private readonly static Random _rnd = new Random(DateTime.Now.Millisecond);

        static Direction()
        {
            _directions =
               new Vector2D[3, 3]
                    {
                        {
                            new Vector2D(-1, -1),
                            new Vector2D(-1, 0),
                            new Vector2D(-1, 1),
                        },
                        {
                            new Vector2D(0, -1),
                            new Vector2D(0, 0),
                            new Vector2D(0, 1),
                        },
                        {
                            new Vector2D(1, -1),
                            new Vector2D(1, 0),
                            new Vector2D(1, 1),
                        }
                    };
            _availableDirections = new List<Vector2D>();
        }

        public static Vector2D ChooseRandomDirection(Func<int, int, int, bool> checker,
            int x, int y, int costLimit)
        {
            _availableDirections.Clear();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((i != 0 || j != 0) && checker(x + i, y + j, costLimit))
                        _availableDirections.Add(_directions[i + 1, j + 1]);
                }
            }

            return
                _availableDirections.Count != 0
                    ? _availableDirections[_rnd.Next()%_availableDirections.Count]
                    : _directions[2, 2];
        }

        public static Vector2D ChooseOppositeDirection(Func<int, int, int, bool> checker,
             Vector2D subject, Vector2D target, int costLimit)
        {
            double tmp;
            double distance = 0.0;
            Vector2D desiredDist = null;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((i != 0 || j != 0) && checker((int)Math.Round(subject.X) + i, 
                        (int)Math.Round(subject.Y) + j, costLimit))
                    {
                        if ((tmp = Vector2D.Vec2DDistance(_directions[i + 1, j + 1], target)) > distance)
                        {
                            distance = tmp;
                            desiredDist = _directions[i + 1, j + 1];
                        }
                    }
                }
            }
            return desiredDist;
        }
    }
}
