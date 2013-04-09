using System;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;

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
        private Map _map;
        private MapPoint _viewOnMap;

        public void AddView(MapPoint point)
        {
            throw new NotImplementedException();
        }

        //private void InitTest()
        //{
        //    if (_viewOnMap == null)
        //        throw new AliveChessException("Object is not initialized");
        //}

        //[ProtoBeforeSerialization]
        //public void BeforeSerialization()
        //{
        //    Trace.Assert(this.ViewOnMap != null);
        //}

        //[ProtoAfterDeserialization]
        //public void AfterDeserialization()
        //{
        //    this.MapPointId = this.ViewOnMap.Id;
        //}

        public int X
        {
            get
            {
                //InitTest();
                //return _viewOnMap.X;
                return _x;
            }
            set
            {
                //InitTest();
                //_viewOnMap.X = value;
                _x = value;
            }
        }

        public int Y
        {
            get
            {
                //InitTest();
                //return _viewOnMap.Y;
                return _y;
            }
            set
            {
                //InitTest();
                //_viewOnMap.Y = value;
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
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }
        public event DeferredTargetedLoadingHandler<Border> OnDeferredLoadingMapPoint;
    }
}
