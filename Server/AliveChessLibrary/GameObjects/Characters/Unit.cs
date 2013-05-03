using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;
#if !UNITY_EDITOR
using System.Data.Linq;
#endif

namespace AliveChessLibrary.GameObjects.Characters
{
    /// <summary>
    /// фигура
    /// </summary>
    [ProtoContract]
    public class Unit
    {
        [ProtoMember(1)]
        private int _unitId;
        [ProtoMember(2)]
        private UnitType _unitType;
        [ProtoMember(3)]
        private int _quantity = 1;

        private int? _figureStoreId;
        private int? _kingId;
#if !UNITY_EDITOR
        private EntityRef<King> _king;
        private EntityRef<FigureStore> _figureStore;
#else
        private King _king;
        private FigureStore _vault;
#endif
        public Unit()
        {
#if !UNITY_EDITOR
            this._king = default(EntityRef<King>);
            this._figureStore = default(EntityRef<FigureStore>);
#else
            this.King = null;
            this.Vault = null;
#endif
        }

        public int Id
        {
            get
            {
                return this._unitId;
            }
            set
            {
                if (this._unitId != value)
                {
                    this._unitId = value;
                }
            }
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
            }
        }

#if !UNITY_EDITOR

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

        public int? FigureStoreId
        {
            get
            {
                return this._figureStoreId;
            }
            set
            {
                if (this._figureStoreId != value)
                {
                    if (this._figureStore.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._figureStoreId = value;
                }
            }
        }
#endif

        public UnitType UnitType
        {
            get
            {
                return this._unitType;
            }
            set
            {
                if (this._unitType != value)
                {
                    this._unitType = value;
                }
            }
        }

#if !UNITY_EDITOR

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
                        previousKing.Units.Remove(this);
                    }

                    if (value != null)
                    {
                        value.Units.Add(this);
                        _kingId = value.Id;
                        _king.Entity = value;
                    }
                    else
                    {
                        _kingId = null;
                    }
                }
            }
        }

        public FigureStore FigureStore
        {
            get
            {
                return this._figureStore.Entity;
            }
            set
            {
                if (_figureStore.Entity != value)
                {
                    if (_figureStore.Entity != null)
                    {
                        var previousVault = _figureStore.Entity;
                        _figureStore.Entity = null;
                        previousVault.Units.Remove(this);
                    }
                    _figureStore.Entity = value;
                    if (value != null)
                    {
                        _figureStore.Entity.Units.Add(this);
                        _figureStoreId = value.Id;
                    }
                    else
                    {
                        _figureStoreId = null;
                    }
                }
            }
        }
#else
        public King King
        {
            get { return _king; }
            set { _king = value; }
        }

        public FigureStore Vault
        {
            get { return _vault; }
            set { _vault = value; }
        }
#endif
    }
}
