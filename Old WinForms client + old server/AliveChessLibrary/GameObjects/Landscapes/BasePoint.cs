using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    [ProtoContract]
    [Table(Name = "dbo.base_point")]
    public class BasePoint : ISinglePoint
    {
        private int _imageId;
        private int _landscapeId;
        private int _mapPointId;

        [ProtoMember(1)]
        private MapPoint _viewOnMap;
        [ProtoMember(2)]
        private LandscapeTypes _landscapePointType;

        private int? _mapId;
        private EntityRef<Map> _map;

        private UpdateCheck updateCheck;
        public UpdateCheck UpdateCheck
        {
            get { return updateCheck; }
            set { updateCheck = value; }
        }

        private void InitTest()
        {
            if (_viewOnMap == null)
                throw new AliveChessException("Object is not initialized");
        }

        public void AddView(MapPoint point)
        {
            this.ViewOnMap = point;
            this._map.Entity.SetObject(_viewOnMap);
        }

        public MapPoint ViewOnMap
        {
            get
            {
                return this._viewOnMap;
            }
            set
            {
                if (_viewOnMap != value)
                {
                    if (value != null)
                    {
                        _viewOnMap = value;
                        _mapPointId = _viewOnMap.Id;
                    }
                }
            }
        }

        public int X
        {
            get
            {
                InitTest();
                return _viewOnMap.X;
            }
            set
            {
                InitTest();
                _viewOnMap.X = value;
            }
        }

        public int Y
        {
            get
            {
                InitTest();
                return _viewOnMap.Y;
            }
            set
            {
                InitTest();
                _viewOnMap.Y = value;
            }
        }

        public float WayCost
        {
            get
            {
                InitTest();
                return _viewOnMap.WayCost;
            }
            set
            {
                InitTest();
                _viewOnMap.WayCost = value;
            }
        }

        /// <summary>
        /// идентификатор
        /// </summary>
        [Column(Name = "base_point_id", Storage = "_landscapeId", CanBeNull = false, DbType = Constants.DB_INT,
           IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._landscapeId;
            }
            set
            {
                if (this._landscapeId != value)
                {
                    this._landscapeId = value;
                }
            }
        }

        [Column(Name = "map_point_id", Storage = "_mapPointId", CanBeNull = false, DbType = Constants.DB_INT)]
        public int MapPointId
        {
            get
            {
                return this._mapPointId;
            }
            set
            {
                if (this._mapPointId != value)
                {
                    this._mapPointId = value;
                }
            }
        }

        [Column(Name = "landscape_image_id", CanBeNull = false, DbType = Constants.DB_INT, Storage = "_imageId")]
        public int ImageId
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
        /// тип местности
        /// </summary>
        [Column(Name = "base_point_landscape_type", CanBeNull = false, DbType = Constants.DB_INT, 
            Storage = "_landscapePointType")]
        public LandscapeTypes LandscapeType
        {
            get { return _landscapePointType; }
            set
            {
                if (_landscapePointType != value)
                {
                    _landscapePointType = value;
                }
            }
        }

        /// <summary>
        /// идентификатор карты (внешний ключ)
        /// </summary>
        [Column(Name = "map_id", Storage = "_mapId", CanBeNull = false, DbType = Constants.DB_INT)]
        public int? MapId
        {
            get
            {
                return this._mapId;
            }
            set
            {
                if (this._mapId != value)
                {
                    if (this._map.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._mapId = value;
                }
            }
        }

        /// <summary>
        /// ссылка на карту
        /// </summary>
        [Association(Name = "fk_landscape_point_map", Storage = "_map", ThisKey = "MapId", IsForeignKey = true)]
        public Map Map
        {
            get
            {
                return this._map.Entity;
            }
            set
            {
                if (_map.Entity != value)
                {
                    _map.Entity = value;
                    value.BasePoints.Add(this);
                    _mapId = value.Id;
                }
            }
        }
    }
}
