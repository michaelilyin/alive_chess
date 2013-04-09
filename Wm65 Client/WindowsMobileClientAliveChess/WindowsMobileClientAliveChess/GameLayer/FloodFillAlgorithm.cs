﻿using System;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Utility;

namespace WindowsMobileClientAliveChess.GameLayer
{
    public class FloodFillAlgorithm
    {
        private Map _map;
        private BasePoint _start;
        private LandscapeTypes _type;
        private Stack<BasePoint> _stack;
        private ImageInfo _image;

        public FloodFillAlgorithm(Map map, BasePoint start, LandscapeTypes type)
        {
            this._map = map;
            this._start = start;
            this._type = type;
            this._stack = new Stack<BasePoint>();

            this._image = new ImageInfo();
            this._image.ImageId = 0;
        }

        public void Run(int x, int y)
        {
            if (!IsEmpty(x, y)) return;

            _map.SetObject(Map.CreatePoint(x, y,
                PointTypes.Landscape));

            if (_map.Locate(x - 1, y))
                Run(x - 1, y);
            if (_map.Locate(x + 1, y))
                Run(x + 1, y);
            if (_map.Locate(x, y + 1))
                Run(x, y + 1);
            if (_map.Locate(x, y - 1))
                Run(x, y - 1);
        }

        private bool IsEmpty(int x, int y)
        {
            return _map[x, y] == null;
        }
    }
}