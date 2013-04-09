using System.Collections.Generic;

namespace AliveChessLibrary.GameObjects.Abstract
{
    public class CompositeVisibleSpace : IVisibleSpace
    {
        private IList<IVisibleSpace> _parts;

        public CompositeVisibleSpace()
        {
            this._parts = new List<IVisibleSpace>();
        }

        public void AddSector(IVisibleSpace sector)
        {
            if (!_parts.Contains(sector))
                _parts.Add(sector);
        }

        public void RemoveSector(IVisibleSpace sector)
        {
            if (_parts.Contains(sector))
                _parts.Remove(sector);
        }

        public bool Contains(int x, int y)
        {
            for (int i = 0; i < _parts.Count; i++)
            {
                if (_parts[i].Contains(x, y))
                    return true;
            }
            return false;
        }

        public bool Contains(float x, float y)
        {
            return Contains((int)x, (int)y);
        }

        public IEnumerable<MapPoint> Walk()
        {
            List<MapPoint> cells = new List<MapPoint>();
            for (int i = 0; i < _parts.Count; i++)
                foreach (var mapPoint in _parts[i].Walk())
                    cells.Add(mapPoint);
            for (int i = 0; i < cells.Count; i++)
                yield return cells[i];
        }
    }
}
