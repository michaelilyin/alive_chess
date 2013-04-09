using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;
using ProtoBuf;
#if !UNITY_EDITOR
using System.Data.Linq;
#endif

namespace AliveChessLibrary.GameObjects.Landscapes
{
    /// <summary>
    /// индикатор местности
    /// </summary>
    [ProtoContract]
    public class BasePoint : ISinglePoint
    {
        [ProtoMember(1)]
        private int _basePointId;
        [ProtoMember(2)]
        private int _x;
        [ProtoMember(3)]
        private int _y;
        [ProtoMember(4)]
        private LandscapeTypes _landscapePointType;
        [ProtoMember(5)]
        private float _wayCost;

        private int _imageId;
        private MapPoint _viewOnMap;
        private int? _mapId;

#if !UNITY_EDITOR
        private EntityRef<Map> _map;
#else
        private Map _map;
#endif

        public BasePoint()
        {
            
        }

        public BasePoint(Map map, MapPoint point)
        {
            _map.Entity = map;
            _mapId = map.Id;
            if (point != null)
                AddView(point);
        }

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

        public MapPoint ViewOnMap
        {
            get { return _viewOnMap; }
            set { _viewOnMap = value; }
        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public PointTypes Type
        {
            get { return PointTypes.Landscape; }
        }

        public float WayCost
        {
            get { return _wayCost; }
            set { _wayCost = value; }
        }

        public int Id
        {
            get
            {
                return this._basePointId;
            }
            set
            {
                if (this._basePointId != value)
                {
                    this._basePointId = value;
                }
            }
        }

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

#if !UNITY_EDITOR
       
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
#endif
#if !UNITY_EDITOR
       
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
#else
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }
#endif
    }
}
