using System;
using System.Collections.Generic;
using System.Diagnostics;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Abstract
{
    /// <summary>
    /// класс описывающий область видимости
    /// </summary>
    public class VisibleSpace : IVisibleSpace
    {
        private IObserver _observer;
        private const int Distance = 8;

        public VisibleSpace(IObserver observer)
        {
            this._observer = observer;
        }

        public bool Contains(int x, int y)
        {
            return Math.Sqrt(Math.Pow(_observer.X - x, 2) + Math.Pow(_observer.Y - y, 2)) <= Distance;
        }

        public bool Contains(float x, float y)
        {
            return Contains((int) x, (int) y);
        }

        public IEnumerable<MapPoint> Walk()
        {
            Debug.Assert(_observer.Map != null);

            int lX = _observer.X - Distance;
            int tY = _observer.Y - Distance;
            for (int i = lX; i < lX + 2*Distance + 1; i++)
            {
                for (int j = tY; j < tY + 2*Distance + 1; j++)
                {
                    if (_observer.Map.Locate(i, j))
                        yield return _observer.Map[i, j];
                }
            }
        }
    }
}
