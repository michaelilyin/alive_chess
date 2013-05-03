using System;
using System.Collections.Generic;
using System.Data.Linq;
using AliveChessLibrary;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Utility;

namespace AliveChessServer.LogicLayer.Environment.Alliances
{
    public class Store
    {
        private int _storeId;
        private EntitySet<FigureStore> _figureStore;
        private EntitySet<ResourceStore> _resourceStore;

        private object _figireStoreSync = new object();
        private object _resourceStoreSync = new object();

        public Store()
        {
            this._figureStore = new EntitySet<FigureStore>(AttachUnitStore, DetachUnitStore);
            this._resourceStore = new EntitySet<ResourceStore>(AttachResourceStore, DetachResourceStore);
        }

        public void AddResourceStore(ResourceStore store)
        {
            lock (_resourceStoreSync)
                _resourceStore.Add(store);
        }

        public void RemoveResourceStore(ResourceStore store)
        {
            lock (_resourceStoreSync)
                _resourceStore.Remove(store);
        }

        public void AddFigureStore(FigureStore store)
        {
            lock (_figireStoreSync)
                _figureStore.Add(store);
        }

        public void RemoveFigureStore(FigureStore store)
        {
            lock (_figireStoreSync)
                _figureStore.Remove(store);
        }

        public void AddResource(Resource resource)
        {
            ResourceStore store = null;
            lock (_resourceStoreSync)
                store = _resourceStore.Search(x => !x.IsFull);
            if (store != null) store.AddResourceToStore(resource);
            else throw new AliveChessException("All stores are full");
        }

        public void AddFigure(Unit unit)
        {
            FigureStore store = null;
            lock (_figireStoreSync)
                store = _figureStore.Search(x => !x.IsFull);
            if (store != null) store.AddFigureToRepository(unit);
            else throw new AliveChessException("All stores are full");
        }

        /// <summary>
        /// получение хранилища фигур по условию
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public FigureStore GetFigureStore(Func<FigureStore, bool> predicate)
        {
            lock (_figireStoreSync)
                return _figureStore.Search(predicate);
        }

        /// <summary>
        /// получение хранилища ресурсов по условию
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public ResourceStore GetResourceStore(Func<ResourceStore, bool> predicate)
        {
            lock (_resourceStoreSync)
                return _resourceStore.Search(predicate);
        }

        /// <summary>
        /// получение всех фигур из заданного хранилища по условию
        /// </summary>
        /// <param name="store"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<Unit> GetUnitsFromStore(FigureStore store, Func<Unit, bool> predicate, 
            bool removeFromStore)
        {
            List<Unit> result = new List<Unit>();
            for (int i = 0; i < store.Units.Count; i++)
            {
                if (predicate(store.Units[i])) result.Add(store.Units[i]);
                if (removeFromStore) store.Units.Remove(store.Units[i]);
            }

            return result;
        }

        /// <summary>
        /// получение заданного количества фигур из заданного хранилища по условию
        /// </summary>
        /// <param name="store"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<Unit> GetUnitsFromStore(FigureStore store, Func<Unit, bool> predicate,
            bool removeFromStore, int count)
        {
            List<Unit> result = new List<Unit>();
            //store.Units.ForEach(
            //    x =>
            //    {
            //        if (predicate(x)) result.Add(x);
            //        if (removeFromStore) store.Units.Remove(x);
            //    });
            return result;
        }

        /// <summary>
        /// получение ресурсов из заданного хранилища по условию
        /// </summary>
        /// <param name="store"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<Resource> GetResourcesFromStore(ResourceStore store, Func<Resource, bool> predicate,
            bool removeFromStore)
        {
            List<Resource> result = new List<Resource>();
            for (int i = 0; i < store.Resources.Count; i++)
            {
                if (predicate(store.Resources[i])) result.Add(store.Resources[i]);
                if (removeFromStore) store.Resources.Remove(store.Resources[i]);
            }
 
            return result;
        }

        /// <summary>
        /// получение заданного количества ресурсов из заданного хранилища по условию
        /// </summary>
        /// <param name="store"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<Resource> GetResourcesFromStore(ResourceStore store, Func<Resource, bool> predicate,
            bool removeFromStore, int count)
        {
            List<Resource> result = new List<Resource>();
            //store.Resources.ForEach(
            //    x =>
            //    {
            //        if (predicate(x))
            //        {
            //            result.Add(r);
            //        }
            //        if (removeFromStore) store.Resources.Remove(x);
            //    });
            return result;
        }

        private void AttachUnitStore(FigureStore entity)
        {
            entity.StoreId = this.Id;
        }

        private void DetachUnitStore(FigureStore entity)
        {
            entity.StoreId = null;
        }

        private void AttachResourceStore(ResourceStore entity)
        {
            entity.StoreId = this.Id;
        }

        private void DetachResourceStore(ResourceStore entity)
        {
            entity.StoreId = null;
        }

        //[Column(Name = "store_id", Storage = "_storeId", CanBeNull = false, DbType = Constants.DB_INT,
        //  IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
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

        public EntitySet<FigureStore> UnitStores
        {
            get
            {
                return this._figureStore;
            }
            set
            {
                this._figureStore.Assign(value);
            }
        }

        public EntitySet<ResourceStore> ResourceStores
        {
            get
            {
                return this._resourceStore;
            }
            set
            {
                this._resourceStore.Assign(value);
            }
        }
    }
}
