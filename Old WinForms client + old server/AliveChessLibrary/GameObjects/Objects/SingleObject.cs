using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;

namespace AliveChessLibrary.GameObjects.Objects
{
    [Table(Name="dbo.single_object")]
    public class SingleObject : ISinglePoint
    {
        private int _mapId;
        private int _singleObjectId;
        private int _mapPointId;
        private MapPoint _viewOnMap;
        private SingleObjectType _singleObjectType;
        
        private EntityRef<Map> _map;

        public SingleObject()
        {
            this._map = default(EntityRef<Map>);
        }

        private void InitTest()
        {
            if (_viewOnMap == null)
                throw new AliveChessException("Object is not initialized");
        }

        public void Initialize(Guid id, Map map)
        {
            this.Map = map;
            //this._singleObjectId = id;
        }

        public void AddView(MapPoint point)
        {
            this.ViewOnMap = point;
            this._map.Entity.SetObject(_viewOnMap);
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

        [Column(Name = "single_object_id", Storage = "_singleObjectId", CanBeNull = false,
            DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
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

        [Column(Name = "map_id", Storage = "_mapId", CanBeNull = false, DbType = Constants.DB_INT, UpdateCheck = UpdateCheck.Never)]
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

        [Column(Name = "map_point_id", Storage = "_mapPointId", CanBeNull = false, DbType = Constants.DB_INT, UpdateCheck = UpdateCheck.Never)]
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

        [Column(Name = "single_object_type", Storage = "_singleObjectType", CanBeNull = false,
            DbType = Constants.DB_INT, UpdateCheck = UpdateCheck.Never)]
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

        [Association(Name = "fk_single_object_map", Storage = "_map", ThisKey = "MapId", IsForeignKey = true)]
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
