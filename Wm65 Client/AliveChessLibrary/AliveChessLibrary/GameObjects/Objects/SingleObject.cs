using System;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;
namespace AliveChessLibrary.GameObjects.Objects
{
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
        private Map _map;

        public SingleObject()
        {
            this.Map = null;
        }

        public void Initialize(int id, Map map)
        {
            this.Map = map;
            this._singleObjectId = id;
        }

        public void AddView(MapPoint point)
        {
            this.ViewOnMap = point;
            ViewOnMap.SetOwner(this);
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
            get { return _map; }
            set { _map = value; }
        }
        public event DeferredTargetedLoadingHandler<SingleObject> OnDeferredLoadingMapPoint;
    }
}
