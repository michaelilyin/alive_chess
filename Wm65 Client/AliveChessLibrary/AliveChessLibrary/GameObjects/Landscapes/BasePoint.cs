using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Landscapes
{
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

        private Map _map;

        public void AddView(MapPoint point)
        {
            this.ViewOnMap = point;
            this.ViewOnMap.SetOwner(this);
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
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }
    }
}
