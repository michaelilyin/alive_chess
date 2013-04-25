using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Utility;

namespace AliveChess.Utilities
{
    /// <summary>
    /// алгоритм заполнения замкнутой области
    /// </summary>
    public class ClientFloodAlgorithm
    {
        private Map _map;
        private ImageInfo _image;

        public ClientFloodAlgorithm(Map map)
        {
            this._map = map;
            this._image = new ImageInfo();
            this._image.ImageId = 0;
        }

        private MapPoint GetPoint(int x, int y)
        {
            return this._map[x, y];
        }

        private bool IsEmpty(int x, int y)
        {
            return this._map[x, y] == null;
        }

        private bool CompareTypes(int x, int y, PointTypes type)
        {
            return this._map[x, y].PointType == type;
        }

        private void ChangePointType(int x, int y, PointTypes newType)
        {
            this._map[x, y].PointType = newType;
        }

        public List<BasePoint> Run(BasePoint basePoint)
        {
            Queue<MapPoint> cells = new Queue<MapPoint>();
            cells.Enqueue(GetPoint(basePoint.X, basePoint.Y));

            List<BasePoint> result = new List<BasePoint>();

            while (cells.Count > 0)
            {
                MapPoint landscape = cells.Dequeue();
                BasePoint newBasePoint = new BasePoint(_map, landscape);
                newBasePoint.LandscapeType = basePoint.LandscapeType;
                newBasePoint.WayCost = landscape.WayCost;
                newBasePoint.X = landscape.X;
                newBasePoint.Y = landscape.Y;
                newBasePoint.ViewOnMap = landscape;
                landscape.SetOwner(newBasePoint);

                result.Add(newBasePoint);

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
            return result;
        }

        public Rectangle[,] FillRun(Rectangle[,] ground, BasePoint basePoint, ImageBrush[] _groundBrushes, int height, int width)
        {
            Queue<MapPoint> cells = new Queue<MapPoint>();
            cells.Enqueue(GetPoint(basePoint.X, basePoint.Y));

            while (cells.Count > 0)
            {
                MapPoint landscape = cells.Dequeue();
                Rectangle r = new Rectangle();
                r.Height = height + 1;
                r.Width = width + 1;
                r.Fill = _groundBrushes[(int)basePoint.LandscapeType];
                TranslateTransform t = new TranslateTransform();
                t.X = landscape.X * width;
                t.Y = landscape.Y * height;
                r.RenderTransform = t;
                ground[landscape.X, landscape.Y] = r;

                if (landscape.X > 0 && ground[landscape.X - 1, landscape.Y] == null)
                {
                    cells.Enqueue(GetPoint(landscape.X - 1, landscape.Y));
                }
                if (landscape.X < _map.SizeX - 1 && ground[landscape.X + 1, landscape.Y] == null)
                {
                    cells.Enqueue(GetPoint(landscape.X + 1, landscape.Y));
                }
                if (landscape.Y > 0 && ground[landscape.X, landscape.Y - 1] == null)
                {
                    cells.Enqueue(GetPoint(landscape.X, landscape.Y - 1));
                }
                if (landscape.Y < _map.SizeY - 1 && ground[landscape.X, landscape.Y + 1] == null)
                {
                    cells.Enqueue(GetPoint(landscape.X, landscape.Y + 1));
                }
            }
            return ground;
        }
    }
}