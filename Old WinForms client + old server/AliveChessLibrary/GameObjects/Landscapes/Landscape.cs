using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using ProtoBuf;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    [ProtoContract]
    [Table(Name = "alive_chess.base_point")]
    public class Landscape : ISinglePoint
    {
        private Guid _landscapeId;

        [ProtoMember(1)]
        private MapPoint _viewOnMap;
        [ProtoMember(2)]
        private LandscapeTypes _landscapePointType;

        private Guid? _mapId;
        private EntityRef<Map> _map;

        public void AddView(MapPoint point)
        {
            this.ViewOnMap = point;
            this._map.Entity.SetObject(_viewOnMap);
        }

        public uint Id
        {
            get
            {
                if (_viewOnMap != null)
                    return _viewOnMap.Id;
                else throw new NullReferenceException("Landscape: Map Point must be not null");
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
                        //_mapPointId = value.Id;
                    }
                }
            }
        }

        public int X
        {
            get { return _viewOnMap.X; }
            set { _viewOnMap.X = value; }
        }

        public int Y
        {
            get { return _viewOnMap.Y; }
            set { _viewOnMap.Y = value; }
        }

        public float WayCost
        {
            get { return _viewOnMap.WayCost; }
            set { _viewOnMap.WayCost = value; }
        }

        public int? ImageId
        {
            get { return _viewOnMap.ImageId; }
            set { _viewOnMap.ImageId = value; }
        }

        /// <summary>
        /// идентификатор
        /// </summary>
        [Column(Name = "base_point_id", Storage = "_landscapeId", CanBeNull = false, DbType = "binary(16)",
           IsPrimaryKey = true)]
        public Guid DbId
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

        /// <summary>
        /// тип местности
        /// </summary>
        [Column(Name = "landscape_type", CanBeNull = false, DbType = "int", Storage = "_landscapePointType")]
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

        /// <summary>
        /// идентификатор карты (внешний ключ)
        /// </summary>
        [Column(Name = "map_id", Storage = "_mapId", CanBeNull = false, DbType = "binary(16)")]
        public Guid? MapId
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
        [Association(Name = "fk_landscape_point_map", Storage = "_map", ThisKey = "DbId",
            IsForeignKey = true)]
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
                        previousMap.BasePoints.Remove(this);
                    }
                    _map.Entity = value;
                    if (value != null)
                    {
                        _mapId = value.DbId;
                    }
                    else
                    {
                        _mapId = null;
                    }
                }
            }
        }
    }
}
