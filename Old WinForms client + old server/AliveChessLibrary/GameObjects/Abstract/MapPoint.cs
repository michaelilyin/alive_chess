using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Abstract
{ 
    /// <summary>
    /// класс представляет собой отдельную ячейку карты.
    /// </summary>
    [ProtoContract]
    [Table(Name = "dbo.map_point")]
    public class MapPoint : ILocation, IEquatable<MapPoint>
    {
        #region Variables
       
        [ProtoMember(1)]
        private int _pointId;

        [ProtoMember(2)]
        private int _mapPointX;

        [ProtoMember(3)]
        private int _mapPointY; 

        [ProtoMember(4)]
        private PointTypes _mapPointType;

        [ProtoMember(5)]
        private float _wayCost;

        [ProtoMember(6)]
        private int? _imageId;

        [ProtoMember(7)]
        private string _imageName;

        [ProtoMember(8)]
        private int? _textureId;

        [ProtoMember(9)]
        private int? _borderTextureId;

        [ProtoMember(10)]
        private int? _direction;

        private bool _detected;
  
        private int? _objectUnderThisId;
        
        private MapSector _mapSector;
        private EntityRef<MapPoint> _objectUnderThis;

        private IMapObject _owner;

        #endregion

        public MapPoint()
        {
            this._imageId = null;
            this._imageName = null;
            this._textureId = null;
            this._borderTextureId = null;
            this._direction = null;
        }

        public bool Equals(MapPoint other)
        {
            return this.Id == other.Id;
        }

        #region Properties

        public IMapObject Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public string ImageName
        {
            get { return _imageName; }
            set { _imageName = value; }
        }

        public int? TextureId
        {
            get { return _textureId; }
            set { _textureId = value; }
        }

        public int? BorderTextureId
        {
            get { return _borderTextureId; }
            set { _borderTextureId = value; }
        }

        public int? Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// флаг признака посещенности ячейки
        /// </summary>
        public bool Detected
        {
            get { return _detected; }
            set { _detected = value; }
        }

        /// <summary>
        /// тип ячейки
        /// </summary>
        public PointTypes MapPointType
        {
            get { return _mapPointType; }
            set { _mapPointType = value; }
        }

        /// <summary>
        /// ячейка под данной ячейкой
        /// </summary>
        public MapPoint ObjectUnderThis
        {
            get
            {
                return _objectUnderThis.Entity;
            }
            set
            {
                if (_objectUnderThis.Entity != value)
                {
                    _objectUnderThis.Entity = value;
                    _objectUnderThisId = value.Id;
                }
            }
        }

        /// <summary>
        /// ссылка на сектор в случае если ячейка принадлежит сектору
        /// </summary>
        public MapSector MapSector
        {
            get { return _mapSector; }
            set
            {
                if (_mapSector != value)
                {
                    _mapSector = value;
                    _mapSector.MapPoints.Add(this);
                }
            }
        }

        [Column(Name = "map_point_id", CanBeNull = false, DbType = Constants.DB_INT, Storage = "_pointId",
          IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _pointId; }
            set
            {
                if (_pointId != value)
                {
                    _pointId = value;
                }
            }
        }

        [Column(Name = "map_point_image_id", CanBeNull = true, DbType = Constants.DB_INT, Storage = "_imageId",
            UpdateCheck = UpdateCheck.Never)]
        public int? ImageId
        {
            get
            {
                return this._imageId;
            }
            set
            {
                if (this._imageId != value)
                {
                    this._imageId = value;
                }
            }
        }

        /// <summary>
        /// координата X
        /// </summary>
        [Column(Name = "map_point_x", CanBeNull = false, DbType = Constants.DB_INT, Storage = "_mapPointX",
            UpdateCheck = UpdateCheck.Never)]
        public int X
        {
            get { return _mapPointX; }
            set 
            {
                if (_mapPointX != value)
                {
                    _mapPointX = value;
                }
            }
        }

        /// <summary>
        /// координата Y
        /// </summary>
        [Column(Name = "map_point_y", CanBeNull = false, DbType = Constants.DB_INT, Storage = "_mapPointY",
            UpdateCheck = UpdateCheck.Never)]
        public int Y
        {
            get { return _mapPointY; }
            set 
            {
                if (_mapPointY != value)
                {
                    _mapPointY = value;
                }
            }
        }

        /// <summary>
        /// стоимость прохождения
        /// </summary>
        [Column(Name = "way_cost", CanBeNull = false, DbType = "float", Storage = "_wayCost",
            UpdateCheck = UpdateCheck.Never)]
        public float WayCost
        {
            get { return _wayCost; }
            set 
            {
                if (_wayCost != value)
                {
                    _wayCost = value;
                }
            }
        }

        /// <summary>
        /// идентификатор нижней ячейки в БД
        /// </summary>
        [Column(Name = "map_point_under_id", CanBeNull = true, DbType = Constants.DB_INT, Storage = "_objectUnderThisId",
            UpdateCheck = UpdateCheck.Never)]
        public int? ObjectUnderThisId
        {
            get
            {
                return this._objectUnderThisId;
            }
            set
            {
                if (this._objectUnderThisId != value)
                {
                    if (this._objectUnderThis.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._objectUnderThisId = value;
                }
            }
        }

        #endregion
    }
}
