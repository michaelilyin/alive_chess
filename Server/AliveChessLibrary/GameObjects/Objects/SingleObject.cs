using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;
using System.Data.Linq;

namespace AliveChessLibrary.GameObjects.Objects
{
    /// <summary>
    /// небольшой объект, занимающий одну ячейку
    /// </summary>
    [ProtoContract]
    public class SingleObject : ISinglePoint
    {
        [ProtoMember(1)]
        private int _singleObjectId;
        [ProtoMember(2)]
        private int _x;
        [ProtoMember(3)]
        private int _y;
        [ProtoMember(4)]
        private SingleObjectType _singleObjectType;
        [ProtoMember(5)]
        private float _wayCost;

        private int _mapId;
        private int _imageId;
        private MapPoint _viewOnMap;
        
        private EntityRef<Map> _map;

        public SingleObject()
        {
            this._map = default(EntityRef<Map>);
        }

        public void Initialize(Map map)
        {
            this._map.Entity = map;
            this._mapId = map.Id;
        }


        public void Initialize(int id, Map map)
        {
            Initialize(map);
            this._singleObjectId = id;
        }

        public void Initialize(Map map, MapPoint point)
        {
            Initialize(map);
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

        public int ImageId
        {
            get { return _imageId; }
            set { _imageId = value; }
        }

        public float WayCost
        {
            get { return _wayCost; }
            set { _wayCost = value; }
        }

        public PointTypes Type
        {
            get { return PointTypes.SingleObject; }
        }

        public MapPoint ViewOnMap
        {
            get { return _viewOnMap; }
            set { _viewOnMap = value; }
        }

        public int Id
        {
            get
            {
                return this._singleObjectId;
            }
            set
            {
                if (this._singleObjectId != value)
                {
                    this._singleObjectId = value;
                }
            }
        }
 
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
    
        public SingleObjectType SingleObjectType
        {
            get
            {
                return this._singleObjectType;
            }
            set
            {
                if (this._singleObjectType != value)
                {
                    this._singleObjectType = value;
                }
            }
        }
       
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
                        previousMap.SingleObjects.Remove(this);
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
    }
}
