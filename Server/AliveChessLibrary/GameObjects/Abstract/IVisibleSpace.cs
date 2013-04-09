using System.Collections.Generic;

namespace AliveChessLibrary.GameObjects.Abstract
{
    /// <summary>
    /// область видимости
    /// </summary>
    public interface IVisibleSpace
    {
        /// <summary>
        /// проверка принадлежности координаты области видимости
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool Contains(int x, int y);

        /// <summary>
        /// проверка принадлежности координаты области видимости
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool Contains(float x, float y);

        /// <summary>
        /// обход области видимости
        /// </summary>
        /// <returns></returns>
        IEnumerable<MapPoint> Walk();
    }
}
