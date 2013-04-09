using System;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;
namespace AliveChessLibrary.GameObjects.Resources
{
    [ProtoContract]
    public class Resource : IResource, IEquatable<int>, IEquatable<MapPoint>, IDynamic<Resource>, ISinglePoint
    {
        #region Fields
        [ProtoMember(1)]
        private int _resourceId;
        [ProtoMember(2)]
        private int _x;
        [ProtoMember(3)]
        private int _y;
        [ProtoMember(4)]
        private ResourceTypes _resourceType;
        [ProtoMember(5)]
        private int _resourceCount;
        [ProtoMember(6)]
        private float _wayCost;

        private int _imageId;
        private int? _kingId;
        private int? _mapId;
        private int? _vaultId;
        private int? _mapPointId;

        private MapPoint _viewOnMap;

        private Map _map;
        private King _king;
        private ResourceStore _vault;


        #endregion

        #region Constructors

        public Resource()
        {
            this.Map = null;
            this.Vault = null;
        }

        #endregion

        #region Initialization

        public void Initialize(Map map)
        {
            this.Map = map;
        }

        public void Initialize(Map map, ResourceTypes rType)
        {
            this._resourceType = rType;

            Initialize(map);
        }

        public void AddView(MapPoint point)
        {
            this._viewOnMap = point;
            _viewOnMap.SetOwner(this);
        }

        #endregion

        #region Methods

        public bool Equals(int other)
        {
            return Id.CompareTo(other) == 0 ? true : false;
        }

        public bool Equals(MapPoint other)
        {
            return Id.CompareTo(other.Owner.Id) == 0 ? true : false;
        }

        public void Disappear()
        {
            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, new UpdateWorldEventArgs(Map, new FPosition(X, Y),
                                          UpdateType.ResourceDisappear));
        }

        #endregion

        #region Properties

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

        public PointTypes Type
        {
            get { return PointTypes.Resource; }
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

        public int Id
        {
            get
            {
                return this._resourceId;
            }
            set
            {
                if (this._resourceId != value)
                {
                    this._resourceId = value;
                }
            }
        }

        public MapPoint ViewOnMap
        {
            get { return _viewOnMap; }
            set { _viewOnMap = value; }
        }

        //[Column(Name = "map_point_id", Storage = "_mapPointId", CanBeNull = true, DbType = Constants.DB_INT)]
        public int? MapPointId
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

        //[Column(Name = "resource_type", Storage = "_resourceType", CanBeNull = false, 
        //    DbType = Constants.DB_INT)]
        public ResourceTypes ResourceType
        {
            get
            {
                return this._resourceType;
            }
            set
            {
                if (this._resourceType != value)
                {
                    this._resourceType = value;
                }
            }
        }

        //[Column(Name = "resource_count", Storage = "_resourceCount", CanBeNull = false, 
        //    DbType = Constants.DB_INT)]
        public int CountResource
        {
            get
            {
                return this._resourceCount;
            }
            set
            {
                if (this._resourceCount != value)
                {
                    this._resourceCount = value;
                }
            }
        }
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        public King King
        {
            get { return _king; }
            set { _king = value; }
        }

        public ResourceStore Vault
        {
            get { return _vault; }
            set { _vault = value; }
        }
        #endregion

        public event ChangeMapStateHandler<Resource> ChangeMapStateEvent;
    }
}
