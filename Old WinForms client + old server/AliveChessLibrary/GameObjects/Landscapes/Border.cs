using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    [Table(Name = "dbo.border")]
    public class Border : ISinglePoint
    {
        private int _imageId;
        private int _landscapeId;
        private int _mapId;
        private int _mapPointId;
        private EntityRef<Map> _map;
        private MapPoint _viewOnMap;

        private UpdateCheck updateCheck;
        public UpdateCheck UpdateCheck
        {
            get { return updateCheck; }
            set { updateCheck = value; }
        }

        public void AddView(AliveChessLibrary.GameObjects.Abstract.MapPoint point)
        {
            throw new NotImplementedException();
        }

        private void InitTest()
        {
            if (_viewOnMap == null)
                throw new AliveChessException("Object is not initialized");
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

        /// <summary>
        /// идентификатор
        /// </summary>
        [Column(Name = "border_id", Storage = "_landscapeId", CanBeNull = false, DbType = Constants.DB_INT,
           IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._landscapeId;
            }
            set
            {
                if (this._landscapeId != value)
                {
                    this._landscapeId = value;
                }
            }
        }

        [Column(Name = "landscape_image_id", CanBeNull = false, DbType = Constants.DB_INT, Storage = "_imageId")]
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

        [Column(Name = "map_point_id", Storage = "_mapPointId", CanBeNull = false, DbType = Constants.DB_INT)]
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

        /// <summary>
        /// идентификатор карты (внешний ключ)
        /// </summary>
        [Column(Name = "map_id", Storage = "_mapId", CanBeNull = false, DbType = Constants.DB_INT)]
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

        /// <summary>
        /// ссылка на карту
        /// </summary>
        [Association(Name = "fk_border_point_map", Storage = "_map", ThisKey = "MapId", IsForeignKey = true)]
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
    }
}
