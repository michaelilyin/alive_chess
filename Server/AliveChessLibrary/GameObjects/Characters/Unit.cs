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
        private int _unitCount = 1;

        private int? _vaultId;
        private int? _kingId;
#if !UNITY_EDITOR
        private EntityRef<King> _king;
        private EntityRef<FigureStore> _vault;
#else
        private King _king;
        private FigureStore _vault;
#endif
        public Unit()
        {
#if !UNITY_EDITOR
            this._king = default(EntityRef<King>);
            this._vault = default(EntityRef<FigureStore>);
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

        public int UnitCount
        {
            get
            {
                return this._unitCount;
            }
            set
            {
                if (this._unitCount != value)
                {
                    this._unitCount = value;
                }
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

        public FigureStore Vault
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
                        previousVault.Units.Remove(this);
                    }
                    _vault.Entity = value;
                    if (value != null)
                    {
                        _vault.Entity.Units.Add(this);
                        _vaultId = value.Id;
                    }
                    else
                    {
                        _vaultId = null;
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
