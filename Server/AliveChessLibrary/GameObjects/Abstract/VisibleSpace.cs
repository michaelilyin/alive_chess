using System;
using System.Collections.Generic;
using System.Diagnostics;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Abstract
{
    /// <summary>
    /// область видимости
    /// </summary>
    public class VisibleSpace : IVisibleSpace
    {
        private IObserver _observer;
        private const int Distance = 8;

        public VisibleSpace(IObserver observer)
        {
            this._observer = observer;
        }

        /// <summary>
        /// проверка принадлежности координаты области видимости
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Contains(int x, int y)
        {
            return Math.Sqrt(Math.Pow(_observer.X - x, 2) + Math.Pow(_observer.Y - y, 2)) <= Distance;
        }

        /// <summary>
        /// проверка принадлежности координаты области видимости
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Contains(float x, float y)
        {
            return Contains((int) x, (int) y);
        }

        /// <summary>
        /// обход области видимости
        /// </summary>
        /// <returns></returns>
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
