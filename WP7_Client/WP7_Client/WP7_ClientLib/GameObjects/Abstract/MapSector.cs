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

        private PointTypes _mapPointType;
        private float _wayCost;
        private List<MapPoint> _mapPoints;

        #endregion

        #region Constructors

        public MapSector()
        {
            MapPoints = new List<MapPoint>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// перечислитель ячеек
        /// </summary>
        public IEnumerable<MapPoint> NextPoint()
        {
            return MapPoints;
        }

        /// <summary>
        /// добавление ячейки
        /// </summary>
        public void AddPoint(MapPoint point)
        {
            MapPoints.Add(point);
        }

        /// <summary>
        /// удаление ячейки
        /// </summary>
        public void RemovePoint(MapPoint point)
        {
            MapPoints.Remove(point);
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
                _mapPointType = value;
                foreach (var t in MapPoints)
                    t.PointType = value;
            }
        }

        /// <summary>
        /// владелец сектора
        /// </summary>
        public IMapObject Owner
        {
            get
            {
                return _mapPoints.Count > 0 ? _mapPoints[0].Owner : null;
            }
            set
            {
                foreach (var t in _mapPoints)
                {
                    t.SetOwner(value);
                }
            }
        }

        /// <summary>
        /// ширина сектора
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// высота сектора
        /// </summary>
        public int Height { get; set; }

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
        public int X { get; set; }

        /// <summary>
        /// Координата
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// стоимость прохождения
        /// </summary>
        public float WayCost
        {
            get { return _wayCost; }
            set
            {
                _wayCost = value;
                foreach (var t in MapPoints)
                    t.WayCost = value;
            }
        }

        #endregion
    }
}
