using System;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interfaces;
using ProtoBuf;
#if !UNITY_EDITOR
using System.Data.Linq;
#endif

namespace AliveChessLibrary.GameObjects.Resources
{
    /// <summary>
    /// ресурс
    /// </summary>
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

#if !UNITY_EDITOR
        private EntityRef<Map> _map;
        private EntityRef<King> _king;
        private EntityRef<ResourceStore> _vault;
#else
        private Map _map;
        private King _king;
        private ResourceStore _vault;
#endif

        #endregion

        #region Constructors

        public Resource()
        {
#if !UNITY_EDITOR
            this._map = default(EntityRef<Map>);
            this._vault = default(EntityRef<ResourceStore>);
#else
            this.Map = null;
            this.Vault = null;
#endif
        }

        #endregion

        #region Initialization

        public void Initialize(Map map)
        {
            this._map.Entity = map;
            this._mapId = map.Id;
        }

        public void Initialize(Map map, ResourceTypes rType)
        {
            this._resourceType = rType;

            Initialize(map);
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

#if !UNITY_EDITOR
        //[Column(Name = "map_id", Storage = "_mapId", CanBeNull = true, DbType = Constants.DB_INT)]
        public int? MapId
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
#if !UNITY_EDITOR
        //[Column(Name = "king_id", Storage = "_kingId", CanBeNull = true, DbType = Constants.DB_INT)]
        public int? KingId
        {
            get
            {
                return this._kingId;
            }
            set
            {
                if (this._kingId != value)
                {
                    if (this._king.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._kingId = value;
                }
            }
        }

        //[Column(Name = "resource_vault_id", Storage = "_vaultId", CanBeNull = true, DbType = Constants.DB_INT)]
        public int? VaultId
        {
            get
            {
                return this._vaultId;
            }
            set
            {
                if (this._vaultId != value)
                {
                    if (this._vault.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._vaultId = value;
                }
            }
        }
#endif
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

#if !UNITY_EDITOR
        //[Association(Name = "fk_resource_map", Storage = "_map", ThisKey = "MapId", IsForeignKey = true)]
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
                        previousMap.Resources.Remove(this);
                    }
                    _map.Entity = value;
                    if (value != null)
                    {
                        _mapId = value.Id;
                    }
                    else
                    {
                        _mapId = null;
                    }
                }
            }
        }

        //[Association(Name = "fk_resource_vault", Storage = "_vault", ThisKey = "VaultId", IsForeignKey = true)]
        public ResourceStore Vault
        {
            get
            {
                return this._vault.Entity;
            }
            set
            {
                if (_vault.Entity != value)
                {
                    if (_vault.Entity != null)
                    {
                        var previousVault = _vault.Entity;
                        _vault.Entity = null;
                        previousVault.Resources.Remove(this);
                    }
                    _vault.Entity = value;
                    if (value != null)
                    {
                        _vault.Entity.Resources.Add(this);
                        _vaultId = value.Id;
                    }
                    else
                    {
                        _vaultId = null;
                    }
                }
            }
        }

        //[Association(Name = "fk_resource_king", Storage = "_king", ThisKey = "KingId", IsForeignKey = true)]
        public King King
        {
            get
            {
                return this._king.Entity;
            }
            set
            {
                if (_king.Entity != value)
                {
                    if (_king.Entity != null)
                    {
                        var previousKing = _king.Entity;
                        _king.Entity = null;
                        previousKing.Resources.Remove(this);
                    }
                    _king.Entity = value;
                    if (value != null)
                    {
                        value.Resources.Add(this);
                        _kingId = value.Id;
                    }
                    else
                    {
                        _kingId = null;
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
#endif
        #endregion

        public event ChangeMapStateHandler<Resource> ChangeMapStateEvent;
    }
}
