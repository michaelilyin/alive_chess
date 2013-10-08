using System;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Utility;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    /// <summary>
    /// алгоритм заполнения замкнутой области
    /// </summary>
    public class FloodAlgorithm
    {
        private Map _map;
        private ImageInfo _image;

        public FloodAlgorithm(Map map)
        {
            this._map = map;
            this._image = new ImageInfo();
            this._image.ImageId = 0;
        }

        private MapPoint GetPoint(int x, int y)
        {
            return this._map[x, y];
        }

        public void Run(BasePoint basePoint)
        {
            var cells = new Queue<MapPoint>();

            cells.Enqueue(GetPoint(basePoint.X, basePoint.Y));

            int[,] directions = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            while (cells.Count > 0)
            {
                MapPoint landscape = cells.Dequeue();

                for (int i = 0; i < directions.GetLongLength(0); i++) //для всех смежных с landscape клеток
                {
                    int x = landscape.X + directions[i, 0];
                    int y = landscape.Y + directions[i, 1];
                    if (x >= 0 && x < _map.SizeX && y >= 0 && y < _map.SizeY) //если точка не выходит за пределы карты
                    {
                        MapPoint point = GetPoint(x, y);
                        if (!point.Initialized)
                        {
                            if (point.PointType != PointTypes.Landscape) //если точка имеет тип ландшафта, она является границей, и заливка не должна идти дальше нее
                            {
                                cells.Enqueue(point);
                            }
                            if (point.PointType == PointTypes.None) //если точка не имеет типа, ей присваивается тип текущей базовой точки
                            {
                                point.SetOwner(basePoint);
                            }
                            point.Initialized = true;
                        }
                    }
                }
            }
        }

    }
}
