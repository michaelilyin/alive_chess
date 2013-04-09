using System.Collections.Generic;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Abstract
{
    /// <summary>
    /// сектор, составленный из нескольких ячеек
    /// </summary>
    public sealed class MapSector : ILocation
    {
        #region Variables

        private int _leftX;
        private int _topY;
        private PointTypes _mapPointType;
        private float _wayCost;
        private int _width;
        private int _height;
        private List<MapPoint> _mapPoints;

        #endregion

        #region Constructors

        public MapSector()
        {
            this.MapPoints = new List<MapPoint>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// перечислитель ячеек
        /// </summary>
        public IEnumerable<MapPoint> NextPoint()
        {
            for (int i = 0; i < MapPoints.Count; i++)
                yield return MapPoints[i];
        }

        /// <summary>
        /// добавление ячейки
        /// </summary>
        public void AddPoint(MapPoint point)
        {
            this.MapPoints.Add(point);
        }

        /// <summary>
        /// удаление ячейки
        /// </summary>
        public void RemovePoint(MapPoint point)
        {
            this.MapPoints.Remove(point);
        }

        /// <summary>
        /// установка владельца сектора
        /// </summary>
        /// <param name="owner"></param>
        public void SetOwner(IMapObject owner)
        {
            for (int i = 0; i < _mapPoints.Count; i++)
                MapPoints[i].SetOwner(owner);
        }

        #endregion

        #region Properties

        /// <summary>
        /// индексатор ячеек
        /// </summary>
        public MapPoint this[int index]
        {
            get
            {
                return MapPoints[index];
            }
        }

        /// <summary>
        /// тип всех ячеек
        /// </summary>
        public PointTypes MapPointType
        {
            get { return _mapPointType; }
            set
            {
                this._mapPointType = value;
                for (int i = 0; i < MapPoints.Count; i++)
                    this.MapPoints[i].PointType = value;
            }
        }

        /// <summary>
        /// владелец сектора
        /// </summary>
        public IMapObject Owner
        {
            get
            {
                if (_mapPoints.Count > 0)
                    return _mapPoints[0].Owner;
                else return null;
            }
            set
            {
                for (int i = 0; i < _mapPoints.Count; i++)
                {
                    _mapPoints[i].SetOwner(value);
                }
            }
        }

        /// <summary>
        /// ширина сектора
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// высота сектора
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Ячейки
        /// </summary>
        public List<MapPoint> MapPoints
        {
            get { return _mapPoints; }
            set { _mapPoints = value; }
        }
    
        /// <summary>
        /// Координата
        /// </summary>
        public int X
        {
            get { return _leftX; }
            set
            {
                if (_leftX != value)
                {
                    _leftX = value;
                }
            }
        }

        /// <summary>
        /// Координата
        /// </summary>
        public int Y
        {
            get { return _topY; }
            set
            {
                if (_topY != value)
                {
                    _topY = value;
                }
            }
        }

        /// <summary>
        /// стоимость прохождения
        /// </summary>
        public float WayCost
        {
            get { return _wayCost; }
            set
            {
                if (_wayCost != value)
                {
                    _wayCost = value;
                    for (int i = 0; i < MapPoints.Count; i++)
                        this.MapPoints[i].WayCost = value;
                }
            }
        }

        #endregion
    }
}
