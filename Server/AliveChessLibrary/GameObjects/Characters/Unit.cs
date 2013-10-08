using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;
using System.Data.Linq;

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
        private EntityRef<King> _king;
        public Unit()
        {
            this._king = default(EntityRef<King>);
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
                    }

                    if (value != null)
                    {
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
    }
}
