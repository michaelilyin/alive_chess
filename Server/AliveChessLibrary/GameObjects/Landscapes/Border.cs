using System;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;
#if !UNITY_EDITOR
using System.Data.Linq;
#endif

namespace AliveChessLibrary.GameObjects.Landscapes
{
    [ProtoContract]
    public class Border : ISinglePoint
    {
        [ProtoMember(1)]
        private int _borderId;
        [ProtoMember(2)]
        private int _x;
        [ProtoMember(3)]
        private int _y;
        [ProtoMember(4)]
        private float _wayCost;

        private int _imageId;
        private int _mapId;
      
#if !UNITY_EDITOR
        private EntityRef<Map> _map;
#else
        private Map _map;
#endif
        private MapPoint _viewOnMap;

        /// <summary>
        /// добавление представления на карту
        /// </summary>
        /// <param name="point"></param>
        public virtual void AddView(MapPoint point)
        {
            this._viewOnMap = point;
            this._viewOnMap.SetOwner(this);
        }

        /// <summary>
        /// удаление с карты ячейки
        /// </summary>
        public void RemoveView()
        {
            ViewOnMap.SetOwner(null);
        }

        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public float WayCost
        {
            get { return _wayCost; }
            set { _wayCost = value; }
        }

        public PointTypes Type
        {
            get { return PointTypes.Landscape; }
        }

        public MapPoint ViewOnMap
        {
            get { return _viewOnMap; }
            set { _viewOnMap = value; }
        }

        //public MapPoint ViewOnMap
        //{
        //    get
        //    {
        //        if (_viewOnMap == null && OnDeferredLoadingMapPoint != null)
        //            OnDeferredLoadingMapPoint(this, this.MapPointId);

        //        return this._viewOnMap;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            if (_viewOnMap != value || _mapPointId != value.Id)
        //            {
        //                _viewOnMap = value;
        //                _mapPointId = _viewOnMap.Id;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// идентификатор
        /// </summary>
        //[Column(Name = "border_id", Storage = "_landscapeId", CanBeNull = false, DbType = Constants.DB_INT,
        //   IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._borderId;
            }
            set
            {
                if (this._borderId != value)
                {
                    this._borderId = value;
                }
            }
        }

        //[Column(Name = "landscape_image_id", CanBeNull = false, DbType = Constants.DB_INT, Storage = "_imageId")]
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

        //[Column(Name = "map_point_id", Storage = "_mapPointId", CanBeNull = false, DbType = Constants.DB_INT)]
        //public int MapPointId
        //{
        //    get
        //    {
        //        return this._mapPointId;
        //    }
        //    set
        //    {
        //        if (this._mapPointId != value)
        //        {
        //            this._mapPointId = value;
        //        }
        //    }
        //}

#if !UNITY_EDITOR
        /// <summary>
        /// идентификатор карты (внешний ключ)
        /// </summary>
        //[Column(Name = "map_id", Storage = "_mapId", CanBeNull = false, DbType = Constants.DB_INT)]
        public int MapId
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
#endif
#if !UNITY_EDITOR
        /// <summary>
        /// ссылка на карту
        /// </summary>
        //[Association(Name = "fk_border_point_map", Storage = "_map", ThisKey = "MapId", IsForeignKey = true)]
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
                    if (_map.Entity != null)
                    {
                        var previousMap = _map.Entity;
                        _map.Entity = null;
                        previousMap.Borders.Remove(this);
                    }
                    _map.Entity = value;
                    if (value != null)
                    {
                        _mapId = value.Id;
                    }
                    else
                    {
                        _mapId = -1;
                    }
                }
            }
        }

#else
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }
#endif
        public event DeferredTargetedLoadingHandler<Border> OnDeferredLoadingMapPoint;
    }
}
