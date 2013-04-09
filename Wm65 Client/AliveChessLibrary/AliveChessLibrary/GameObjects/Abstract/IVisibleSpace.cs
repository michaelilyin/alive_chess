using System.Collections.Generic;

namespace AliveChessLibrary.GameObjects.Abstract
{
    public interface IVisibleSpace
    {
        bool Contains(int x, int y);

        bool Contains(float x, float y);

        IEnumerable<MapPoint> Walk();
    }
}
