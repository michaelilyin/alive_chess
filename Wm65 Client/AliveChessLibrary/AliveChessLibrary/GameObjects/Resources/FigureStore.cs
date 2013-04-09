using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Utility;

namespace AliveChessLibrary.GameObjects.Resources
{
    public class FigureStore : IStore
    {
        private int _vaultId;
        private int? _storeId;

        private int _size;
        private const int CAPACITY = 1000;
        private List<Unit> _resources;

        public FigureStore()
        {
            this.Units = new List<Unit>();
        }

        #region Methods

        public void AddFigureToRepository(Unit addResource)
        {
            Unit tmpRes = GetFigure(addResource.UnitType);
            if (tmpRes != null)
                tmpRes.UnitCount += addResource.UnitCount;
            else this.Units.Add(addResource);
        }

        public Unit GetFigure(UnitType typeRes)
        {
            return Units.Search<Unit>(
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
            Unit u = Units.Search<Unit>(
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

        //[Column(Name = "figure_vault_id", Storage = "_vaultId", CanBeNull = false, 
        //    DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true)]
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

        //[Column(Name = "store_id", Storage = "_storeId", CanBeNull = true, DbType = Constants.DB_INT)]
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
        public List<Unit> Units
        {
            get { return _resources; }
            set { _resources = value; }
        }

    }
}
