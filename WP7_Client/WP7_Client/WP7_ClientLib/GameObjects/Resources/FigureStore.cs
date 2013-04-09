using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Utility;
#if UNITY_EDITOR
using System.Data.Linq;
#endif

namespace AliveChessLibrary.GameObjects.Resources
{
    public class FigureStore : IStore
    {
        private int _size;
        private const int CAPACITY = 1000;
#if UNITY_EDITOR
        private EntitySet<Unit> _resources;
#else
#endif

        public FigureStore()
        {
#if UNITY_EDITOR
            this._resources = new EntitySet<Unit>(AttachFigure, DetachFigure);
#else
            Units = new List<Unit>();
#endif
        }

        #region Methods

        public void AddFigureToRepository(Unit addResource)
        {
            Unit tmpRes = GetFigure(addResource.UnitType);
            if (tmpRes != null)
                tmpRes.UnitCount += addResource.UnitCount;
            else Units.Add(addResource);
        }

        public Unit GetFigure(UnitType typeRes)
        {
            return Units.Search(
                x => x.UnitType == typeRes);
        }

        public bool RemoveFigure(UnitType type, int count)
        {
            Unit r = GetFigure(type);
            if (r == null || r.UnitCount >= count)
            {
                if (r != null) r.UnitCount -= count;
                return true;
            }
            return false;
        }

        public int GetFigureCountInRepository(UnitType typeRes)
        {
            var u = Units.Search(
                x => x.UnitType == typeRes);
            return u != null ? u.UnitCount : 0;
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

        //[Column(Name = "figure_vault_id", Storage = "_vaultId", CanBeNull = false, 
        //    DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        //[Column(Name = "store_id", Storage = "_storeId", CanBeNull = true, DbType = Constants.DB_INT)]
        public int? StoreId { get; set; }

#if UNITY_EDITOR
        //[Association(Name = "fk_figure_vault", Storage = "_resources", OtherKey = "VaultId")]
        public EntitySet<Unit> Units
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
        public List<Unit> Units { get; set; }

#endif

    }
}
