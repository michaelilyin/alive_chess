using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Utility;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    /// <summary>
    /// алгоритм заполнения замкнутой области
    /// </summary>
    public class FloodAlgorithm
    {
        private readonly ImageInfo _image;
        private readonly Map _map;

        public FloodAlgorithm(Map map)
        {
            _map = map;
            _image = new ImageInfo {ImageId = 0};
        }

        public ImageInfo Image
        {
            get { return _image; }
        }

        private MapPoint GetPoint(int x, int y)
        {
            return _map[x, y];
        }

        private bool IsEmpty(int x, int y)
        {
            return _map[x, y] == null;
        }

        private bool CompareTypes(int x, int y, PointTypes type)
        {
            return _map[x, y].PointType == type;
        }

        private void ChangePointType(int x, int y, PointTypes newType)
        {
            _map[x, y].PointType = newType;
        }

        public void Run(BasePoint basePoint)
        {
            var cells = new Queue<MapPoint>();

            for (int x = 0; x < _map.SizeX; x++)
            {
                for (int y = 0; y < _map.SizeY; y++)
                {
                    if (IsEmpty(x, y))
                        _map.SetObject(Map.CreatePoint(x, y, PointTypes.None));
                }
            }

            cells.Enqueue(GetPoint(basePoint.X, basePoint.Y));

            while (cells.Count > 0)
            {
                MapPoint landscape = cells.Dequeue();

                landscape.SetOwner(basePoint);

                if (landscape.X > 0 && CompareTypes(landscape.X - 1, landscape.Y,
                                                    PointTypes.None))
                {
                    cells.Enqueue(GetPoint(landscape.X - 1, landscape.Y));

                    ChangePointType(landscape.X - 1, landscape.Y,
                                    basePoint.ViewOnMap.PointType);
                }
                if (landscape.X < _map.SizeX - 1 && CompareTypes(landscape.X + 1, landscape.Y,
                                                                 PointTypes.None))
                {
                    cells.Enqueue(GetPoint(landscape.X + 1, landscape.Y));

                    ChangePointType(landscape.X + 1, landscape.Y,
                                    basePoint.ViewOnMap.PointType);
                }
                if (landscape.Y > 0 && CompareTypes(landscape.X, landscape.Y - 1,
                                                    PointTypes.None))
                {
                    cells.Enqueue(GetPoint(landscape.X, landscape.Y - 1));

                    ChangePointType(landscape.X, landscape.Y - 1,
                                    basePoint.ViewOnMap.PointType);
                }
                if (landscape.Y < _map.SizeY - 1 && CompareTypes(landscape.X, landscape.Y + 1,
                                                                 PointTypes.None))
                {
                    cells.Enqueue(GetPoint(landscape.X, landscape.Y + 1));

                    ChangePointType(landscape.X, landscape.Y + 1,
                                    basePoint.ViewOnMap.PointType);
                }
            }
        }
    }
}