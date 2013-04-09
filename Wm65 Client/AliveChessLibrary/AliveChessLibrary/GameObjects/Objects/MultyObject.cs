using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;


namespace AliveChessLibrary.GameObjects.Objects
{
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
        private int _mapSectorId;
       
        private MapSector _viewOnMap;
        private Map _map;
        public MultyObject()
        {
            this.Map = null;
        }

        //private void InitTest()
        //{
        //    if (_viewOnMap == null)
        //        throw new AliveChessException("Object is not initialized");
        //}

        public void AddView(MapSector sector)
        {
            this.ViewOnMap = sector;
            foreach (MapPoint mp in sector.MapPoints)
                //Map.SetObject(mp);
                mp.SetOwner(this);
        }

        //[ProtoBeforeSerialization]
        //public void BeforeSerialization()
        //{
        //    Trace.Assert(this.ViewOnMap != null);
        //}

        //[ProtoAfterDeserialization]
        //public void AfterDeserialization()
        //{
        //    this.MapSectorId = this.ViewOnMap.Id;
        //}

        public int X
        {
            get
            {
                //InitTest();
                //return _viewOnMap.X;
                return _leftX;
            }
            set
            {
                //InitTest();
                //_viewOnMap.X = value;
                _leftX = value;
            }
        }

        public int Y
        {
            get
            {
                //InitTest();
                //return _viewOnMap.Y;
                return _topY;
            }
            set
            {
                //InitTest();
                //_viewOnMap.Y = value;
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

        //public MapSector ViewOnMap
        //{
        //    get
        //    {
        //        if (_viewOnMap == null && OnDeferredLoadingMapPoint != null)
        //            OnDeferredLoadingMapPoint(this, this.MapSectorId);

        //        return this._viewOnMap;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            if (_viewOnMap != value || _mapSectorId != value.Id)
        //            {
        //                this._viewOnMap = value;
        //                this._mapSectorId = _viewOnMap.Id;
        //            }
        //        }
        //    }
        //}

        //[Column(Name = "multy_object_id", Storage = "_multyObjectId", CanBeNull = false,
        //    DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true)]
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
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }
        public event DeferredTargetedLoadingHandler<MultyObject> OnDeferredLoadingMapPoint;
    }
}
