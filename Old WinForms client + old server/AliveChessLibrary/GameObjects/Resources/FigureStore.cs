using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Utility;

namespace AliveChessLibrary.GameObjects.Resources
{
    [Table(Name = "dbo.figure_vault")]
    public class FigureStore : IStore
    {
        private int _vaultId;
        private int? _storeId;

        private int _size;
        private const int CAPACITY = 1000;

        private EntitySet<Unit> _resources;

        public FigureStore()
        {
            this._resources = new EntitySet<Unit>(AttachFigure, DetachFigure);
        }

        #region Methods

        public void AddFigureToRepository(Unit addResource)
        {
            Unit tmpRes = GetFigure(addResource.UnitType);
            if (tmpRes != null)
                tmpRes.UnitCount += addResource.UnitCount;
            else this._resources.Add(addResource);
        }

        public Unit GetFigure(UnitType typeRes)
        {
            return _resources.FindElement<Unit>(
                x =>
                {
                    return x.UnitType == typeRes;
                });
        }

        public bool RemoveFigure(UnitType type, int count)
        {
            Unit r = GetFigure(type);
            if (r == null || r.UnitCount >= count)
            {
                r.UnitCount -= count;
                return true;
            }
            else return false;
        }

        public int GetFigureCountInRepository(UnitType typeRes)
        {
            Unit u = _resources.FindElement<Unit>(
                x =>
                {
                    return x.UnitType == typeRes;
                });
            return u != null ? u.UnitCount : 0;
        }

        private void AttachFigure(Unit entity)
        {
            entity.Vault = this;
        }

        private void DetachFigure(Unit entity)
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

        [Column(Name = "figure_vault_id", Storage = "_vaultId", CanBeNull = false, 
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

        [Association(Name = "fk_figure_vault", Storage = "_resources", OtherKey = "VaultId")]
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
    }
}
