using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;

namespace AliveChessClient.GameLayer
{
    public class FillAlgorithm
    {
        private Map _map;
        private GameData _data;
        private Landscape _start;
        private LandscapeTypes _type;
        private Stack<Landscape> _stack;

        public FillAlgorithm(Map map, Landscape start, LandscapeTypes type, GameData data)
        {
            this._map = map;
            this._start = start;
            this._type = type;
            this._data = data;
            this._stack = new Stack<Landscape>();
        }

        public void Fill(int x, int y)
        {
            if (!IsEmpty(x, y)) return;
            _map.SetObject(CreateMapPoint(x, y, 0, LandscapeTypes.Grass, 0));
            if (_map.Locate(x - 1, y))
                Fill(x - 1, y);
            if (_map.Locate(x + 1, y))
                Fill(x + 1, y);
            if (_map.Locate(x, y + 1))
                Fill(x, y + 1);
            if (_map.Locate(x, y - 1))
                Fill(x, y - 1);
        }

        private bool IsEmpty(int x, int y)
        {
            return _map[x, y] == null;
        }

        private MapPoint CreateMapPoint(int x, int y, int? imgId, LandscapeTypes type,
            int wayCost)
        {
            MapPoint mapObject = new MapPoint();
            mapObject.Id = 0;
            mapObject.DbId = Guid.Empty;
            mapObject.WayCost = wayCost;
            mapObject.ImageId = imgId;
            mapObject.X = x;
            mapObject.Y = y;
            mapObject.MapSector = null;
            mapObject.Detected = false;
            mapObject.MapPointType = GetPointType(type);
            mapObject.ObjectUnderThis = null;
            return mapObject;
        }

        private PointTypes GetPointType(LandscapeTypes lt)
        {
            switch (lt)
            {
                case LandscapeTypes.Grass:
                    return PointTypes.Landscape;
                default:
                    return PointTypes.None;
            }
        }
    }
}
