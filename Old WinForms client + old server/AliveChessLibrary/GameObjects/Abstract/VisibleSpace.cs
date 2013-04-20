using System.Collections;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Landscapes;

namespace AliveChessLibrary.GameObjects.Abstract
{
    /// <summary>
    /// класс описывающий область видимости
    /// </summary>
    public class VisibleSpace
    {
        private Map _map;
        private BitArray _bitArray;

        public VisibleSpace(Map map)
        {
            this._map = map;
            this._bitArray = new BitArray(map.SizeX * map.SizeY);
        }

        public void Init(List<MapPoint> sector)
        {
            foreach (MapPoint mo in sector)
                _bitArray.Set(mo.X * _map.SizeX + mo.Y, true);
        }

        public void Clear()
        {
            _bitArray.SetAll(false);
        }

        public void Add(MapPoint o)
        {
            _bitArray.Set(o.X * _map.SizeX + o.Y, true);
        }

        public void Add(VisibleSpace space)
        {
            _bitArray.Or(space._bitArray);
        }

        public bool Contains(int x, int y)
        {
            return _bitArray.Get(x * _map.SizeX + y);
        }

        private List<MapPoint> TransformToArray()
        {
            List<MapPoint> result = new List<MapPoint>();
            for (int i = 0; i < _map.SizeX; i++)
            {
                for (int j = 0; j < _map.SizeY; j++)
                {
                    if (_bitArray.Get(i * _map.SizeX + j))
                        result.Add(_map[i, j]);
                }
            }
            return result;
        }

        public List<MapPoint> Sector
        {
            get { return TransformToArray(); }
        }
    }
}
