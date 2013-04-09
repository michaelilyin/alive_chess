using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;
#if !UNITY_EDITOR
using System.Data.Linq;
#endif

namespace AliveChessLibrary.GameObjects.Objects
{
    /// <summary>
    /// крупный объект, занимающий несколько ячеек
    /// </summary>
    [ProtoContract]
    public class MultyObject : IMultyPoint
    {
        [ProtoMember(1)]
        private int _multyObjectId;
        [ProtoMember(2)]
        private int _leftX;
        [ProtoMember(3)]
        private int _topY;
        [ProtoMember(4)]
        private int _width;
        [ProtoMember(5)]
        private int _height;
        [ProtoMember(6)]
        private float _wayCost;
        [ProtoMember(7)]
        private MultyObjectTypes _multyObjectType;

        private int _mapId;
        private int _imageId;
       
        private MapSector _viewOnMap;
       
#if !UNITY_EDITOR
        private EntityRef<Map> _map;
#else
        private Map _map;
#endif
        public MultyObject()
        {
#if !UNITY_EDITOR
            this._map = default(EntityRef<Map>);
#else
            this.Map = null;
#endif
        }

        /// <summary>
        /// добавление представления на карту
        /// </summary>
        /// <param name="sector"></param>
        public void AddView(MapSector sector)
        {
            ViewOnMap = sector;
            sector.SetOwner(this);
        }

        /// <summary>
        /// удаление с карты сектора
        /// </summary>
        public void RemoveView()
        {
            ViewOnMap.SetOwner(null);
        }

        public int X
        {
            get
            {
                return _leftX;
            }
            set
            {
                _leftX = value;
            }
        }

        public int Y
        {
            get
            {
                return _topY;
            }
            set
            {
                _topY = value;
            }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
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
            get { return PointTypes.MultyObject; }
        }

        public MapSector ViewOnMap
        {
            get { return _viewOnMap; }
            set { _viewOnMap = value; }
        }

        public int Id
        {
            get
            {
                return this._multyObjectId;
            }
            set
            {
                if (this._multyObjectId != value)
                {
                    this._multyObjectId = value;
                }
            }
        }

#if !UNITY_EDITOR
   
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
        public MultyObjectTypes MultyObjectType
        {
            get
            {
                return this._multyObjectType;
            }
            set
            {
                if (this._multyObjectType != value)
                {
                    this._multyObjectType = value;
                }
            }
        }

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
                    if (_map.Entity != null)
                    {
                        var previousMap = _map.Entity;
                        _map.Entity = null;
                        previousMap.MultyObjects.Remove(this);
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
    }
}
