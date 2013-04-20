using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary.Utility;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Abstract
{
    /// <summary>
    /// сектро на карте
    /// </summary>
    [ProtoContract]
    [Table(Name = "dbo.map_sector")]
    public sealed class MapSector : ILocation, IEquatable<MapSector>
    {
        #region Variables

        [ProtoMember(1)]
        private int _sectorId;

        [ProtoMember(2)]
        private int _leftX;

        [ProtoMember(3)]
        private int _topY;

        [ProtoMember(4)]
        private PointTypes _mapPointType;

        [ProtoMember(5)]
        private float _wayCost;

        [ProtoMember(6)]
        private int _width;

        [ProtoMember(7)]
        private int _height;

        [ProtoMember(8)]
        private int _imageId;

        private EntitySet<MapPoint> _mapPoints;

        #endregion

        private UpdateCheck updateCheck;
        public UpdateCheck UpdateCheck
        {
            get { return updateCheck; }
            set { updateCheck = value; }
        }

        #region Constructors

        public MapSector()
        {
            this._mapPoints = new EntitySet<MapPoint>(AttachMapPoint, DetachMapPoint);
        }

        #endregion

        #region Methods

        public bool Equals(MapSector other)
        {
            return this.Id == other.Id;
        }

        /// <summary>
        /// перечислитель ячеек
        /// </summary>
        public IEnumerable<MapPoint> NextPoint()
        {
            for (int i = 0; i < _mapPoints.Count; i++)
                yield return _mapPoints[i];
        }

        /// <summary>
        /// добавление ячейки
        /// </summary>
        public void AddPoint(MapPoint point)
        {
            this._mapPoints.Add(point);
        }

        /// <summary>
        /// удаление ячейки
        /// </summary>
        public void RemovePoint(MapPoint point)
        {
            this._mapPoints.Remove(point);
        }

        /// <summary>
        /// триггер на добавление ячейки
        /// </summary>
        private void AttachMapPoint(MapPoint entity)
        {
            entity.MapSector = this;
        }

        /// <summary>
        /// триггер на удаление ячейки
        /// </summary>
        private void DetachMapPoint(MapPoint entity)
        {
            entity.MapSector = null;
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
                return _mapPoints[index];
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
                for (int i = 0; i < _mapPoints.Count; i++)
                    this._mapPoints[i].MapPointType = value;
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
        /// список ячеек
        /// </summary>
        public EntitySet<MapPoint> MapPoints
        {
            get { return _mapPoints; }
            set { _mapPoints.Assign(value); }
        }

        [Column(Name = "map_sector_id", CanBeNull = false, Storage = "_sectorId",
          DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _sectorId; }
            set
            {
                if (_sectorId != value)
                {
                    _sectorId = value;
                }
            }
        }

        [Column(Name = "map_sector_image_id", CanBeNull = false, DbType = Constants.DB_INT,
          Storage = "_imageId")]
        public int ImageId
        {
            get { return _imageId; }
            set { _imageId = value; }
        }

        /// <summary>
        /// координата верхнего левого угла
        /// </summary>
        [Column(Name = "left_x", CanBeNull = false, DbType = Constants.DB_INT, Storage = "_leftX")]
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
        /// координата верхнего левого угла
        /// </summary>
        [Column(Name = "top_y", CanBeNull = false, DbType = Constants.DB_INT, Storage = "_topY")]
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
        /// стоимость прохождения всех ячеек
        /// </summary>
        [Column(Name = "way_cost", CanBeNull = false, DbType = "float", Storage = "_wayCost")]
        public float WayCost
        {
            get { return _wayCost; }
            set
            {
                if (_wayCost != value)
                {
                    _wayCost = value;
                    for (int i = 0; i < _mapPoints.Count; i++)
                        this._mapPoints[i].WayCost = value;
                }
            }
        }

        #endregion
    }
}
