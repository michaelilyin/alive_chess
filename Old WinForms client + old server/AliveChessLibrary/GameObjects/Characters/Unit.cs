using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Utility;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Characters
{
    [ProtoContract]
    [Table(Name = "dbo.unit")]
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
       
        private EntityRef<King> _king;
        private EntityRef<FigureStore> _vault;

        public Unit()
        {
            this._king = default(EntityRef<King>);
            this._vault = default(EntityRef<FigureStore>);
        }

        [Column(Name = "unit_id", Storage = "_unitId", CanBeNull = false, 
            DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true)]
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

        [Column(Name = "unit_count", Storage = "_unitCount", CanBeNull = false, 
            DbType = Constants.DB_INT)]
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

        [Column(Name = "king_id", Storage = "_kingId", CanBeNull = true, 
            DbType = Constants.DB_INT)]
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

        [Column(Name = "figure_vault_id", Storage = "_vaultId", CanBeNull = true, 
            DbType = Constants.DB_INT)]
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

        [Column(Name = "unit_type", Storage = "_unitType", CanBeNull = false, DbType = Constants.DB_INT)]
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

        [Association(Name = "fk_unit_king", Storage = "_king", ThisKey = "KingId", IsForeignKey = true)]
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

        [Association(Name = "fk_figure_vault", Storage = "_vault", ThisKey = "VaultId", IsForeignKey = true)]
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
       
    }
}
