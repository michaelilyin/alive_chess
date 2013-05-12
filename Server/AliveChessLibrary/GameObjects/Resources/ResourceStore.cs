using System.Collections.Generic;

#if !UNITY_EDITOR
using System.Data.Linq;

#endif

namespace AliveChessLibrary.GameObjects.Resources
{
    /* Класс Хранилище ресурсов.
    * Описывает Хранилище ресурсов используемых в игре.
    */
    //[Table(Name = "dbo.resource_vault")]
    public sealed class ResourceStore
    {
        private int _id;
        private int? _storeId;

        private int _size; //текущее количество ресурсов в хранилище, видимо
        private const int CAPACITY = 1000;
#if !UNITY_EDITOR
        private EntitySet<Resource> _resources; // масcив хранимых ресурсов
#else
        private List<Resource> _resources;
#endif

        public ResourceStore()
        {
#if !UNITY_EDITOR
            this._resources = new EntitySet<Resource>();
#else
            this.Resources = new List<Resource>();
#endif
        }

        #region Methods

        public void AddResource(Resource resource)
        {
            lock (_resources)
            {
                Resource tmpRes = _getResource(resource.ResourceType);
                if (tmpRes != null)
                {
                    tmpRes.Quantity += resource.Quantity;
                }
                else
                {
                    _resources.Add(resource);
                }
            }
        }

        public void AddResource(ResourceTypes type, int quantity)
        {
            lock (_resources)
            {
                Resource tmpRes = _getResource(type);
                if (tmpRes != null)
                {
                    tmpRes.Quantity += quantity;
                }
                else
                {
                    Resource res = new Resource();
                    res.ResourceType = type;
                    res.Quantity = quantity;
                    _resources.Add(res);
                }
            }
        }

        private bool _deleteResourceFromStore(ResourceTypes type, int count)
        {
            Resource r = _getResource(type);
            if (r != null && r.Quantity >= count)
            {
                r.Quantity -= count;
                return true;
            }
            else return false;
        }

        private Resource _getResource(ResourceTypes type)
        {
            lock (_resources)
            {
                foreach (var resource in _resources)
                {
                    if (resource.ResourceType == type)
                        return resource;
                }
                return null;
            }
        }

        public bool HasEnoughResources(Dictionary<ResourceTypes, int> resources)
        {
            lock (_resources)
            {
                foreach (var item in resources)
                {
                    if (_getResourceQuantity(item.Key) < item.Value)
                        return false;
                }
            }
            return true;

        }

        public void TakeResources(Dictionary<ResourceTypes, int> resources)
        {
            lock (_resources)
            {
                foreach (var item in resources)
                {
                    Resource resource = _getResource(item.Key);
                    if (resource != null)
                        resource.Quantity -= item.Value;
                }
            }
        }

        public void SetResources(List<Resource> resources)
        {
            lock (_resources)
            {
                _resources.Clear();
                if(resources == null)
                    return;
                foreach (var resource in resources)
                {
                    _resources.Add(resource);
                }
            }
        }

        //Что-то непонятное, используется один раз в Emipre
        public Resource PushResource(ResourceTypes type, int quantity)
        {
            lock (_resources)
            {
                Resource result = null;
                Resource res = _getResource(type);
                if (res.Quantity >= quantity)
                {
                    result = new Resource();
                    result.Id = res.Id;
                    result.Id = res.Id;
                    result.Quantity = quantity;
                    result.ResourceType = type;
                    _deleteResourceFromStore(type, quantity);
                    return result;
                }
                else
                {
                    result = _getResource(type);
                    _deleteResourceFromStore(type, result.Quantity);
                    return result;
                }
            }
        }

        private int _getResourceQuantity(ResourceTypes typeRes)
        {
            for (int i = 0; i < this._resources.Count; i++)
            {
                if (this._resources[i].ResourceType == typeRes)
                {
                    return this._resources[i].Quantity;
                }
            }
            return 0;
        }

        /*private void AttachResource(Resource entity)
        {
            entity.ResourceStore = this;
        }

        private void DetachResource(Resource entity)
        {
            entity.ResourceStore = null;
        }*/

        #endregion

        public int MaxCapacity
        {
            get { return CAPACITY; }
        }

        /// <summary>
        /// Число хранящихся ресурсов
        /// </summary>
        public int CurrentCapacity
        {
            get { return _size; }
            set { _size = value; }
        }

        public bool IsFull
        {
            get { return _size >= CAPACITY; }
        }

        //[Column(Name = "resource_vault_id", Storage = "_vaultId", CanBeNull = false,
        //    DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        //[Column(Name = "store_id", Storage = "_storeId", CanBeNull = true, DbType = Constants.DB_INT)]
        public int? StoreId
        {
            get
            {
                return this._storeId;
            }
            set
            {
                this._storeId = value;
            }
        }
/*
#if !UNITY_EDITOR
        //[Association(Name = "fk_resource_vault", Storage = "_resources", OtherKey = "VaultId")]
        public EntitySet<Resource> Resources
        {
            get
            {
                return this._resources;
            }
            set
            {
                this._resources.Assign(value);
            }
        }
#else
        public List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }
#endif*/
        public List<Resource> GetResourceListCopy()
        {
            List<Resource> result = new List<Resource>();
            lock (_resources)
            {
                foreach (var resource in _resources)
                {
                    result.Add(resource);
                }
            }
            return result;
        }
    }
}
