using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary.Utility;

namespace AliveChessLibrary.GameObjects.Resources
{
    /* Класс Хранилище ресурсов.
    * Описывает Хранилище ресурсов используемых в игре.
    */
    [Table(Name = "dbo.resource_vault")]
    public sealed class ResourceStore
    {
        private int _vaultId;
        private int? _storeId;
        private string vaultMesseger; // сообщение от Хранилища ресурсов

        private int _size;
        private const int CAPACITY = 1000;

        private EntitySet<Resource> _resources; // масcив хранимых ресурсов

        public ResourceStore()
        {
            this._resources = new EntitySet<Resource>(AttachResource, DetachResource);
        }

        #region Methods
       
        public void AddResourceToRepository(Resource addResource)
        {
            Resource tmpRes = GetResource(addResource.ResourceType);
            if (tmpRes != null)
            {
                tmpRes.CountResource += addResource.CountResource;
            }
            else
            {
                this._resources.Add(addResource);
            }
        }

        public bool DeleteResourceFromRepository(ResourceTypes type, int count)
        {
            Resource r = GetResource(type);
            if (r != null && r.CountResource >= count)
            {
                r.CountResource -= count;
                return true;
            }
            else return false;
        }

        public Resource GetResource(ResourceTypes typeRes)
        {
            for (int i = 0; i < this._resources.Count; i++)
            {
                if (_resources[i].ResourceType == typeRes)
                {
                    return this._resources[i];
                }
            }
            return null;
        }

        public Resource PushResource(ResourceTypes type, int count)
        {
            Resource result = null;
            Resource res = GetResource(type);
            if (res.CountResource >= count)
            {
                result = new Resource();
                result.Id = res.Id;
                result.Id = res.Id;
                result.CountResource = count;
                result.ResourceType = type;
                DeleteResourceFromRepository(type, count);
                return result;
            }
            else
            {
                result = GetResource(type);
                DeleteResourceFromRepository(type, result.CountResource);
                return result;
            }
        }

        public int GetResourceCountInRepository(ResourceTypes typeRes)
        {
            for (int i = 0; i < this._resources.Count; i++)
            {
                if (this._resources[i].ResourceType == typeRes)
                {
                    return this._resources[i].CountResource;
                }
            }
            return 0;
        }

        private void AttachResource(Resource entity)
        {
            entity.Vault = this;
        }

        private void DetachResource(Resource entity)
        {
            entity.Vault = null;
        }

        #endregion

        public int MaxCapacity
        {
            get { return CAPACITY; }
        }

        public int CurrentCapacity
        {
            get { return _size; }
            set { _size = value; }
        }

        public bool IsFull
        {
            get { return _size >= CAPACITY; }
        }

        public string VaultMesseger
        {
            get { return this.vaultMesseger; }
            set { this.vaultMesseger = value; }
        }

        [Column(Name = "resource_vault_id", Storage = "_vaultId", CanBeNull = false,
            DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._vaultId;
            }
            set
            {
                if (this._vaultId != value)
                {
                    this._vaultId = value;
                }
            }
        }

        [Column(Name = "store_id", Storage = "_storeId", CanBeNull = true, DbType = Constants.DB_INT)]
        public int? StoreId
        {
            get
            {
                return this._storeId;
            }
            set
            {
                if (this._storeId != value)
                {
                    this._storeId = value;
                }
            }
        }

        [Association(Name = "fk_resource_vault", Storage = "_resources", OtherKey = "VaultId")]
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
    }
}
