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
#else
        private King _king;
#endif
        public Unit()
        {
#if !UNITY_EDITOR
            this._king = default(EntityRef<King>);
#else
            this.King = null;
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
#else
        public King King
        {
            get { return _king; }
            set { _king = value; }
        }
#endif
    }
}
