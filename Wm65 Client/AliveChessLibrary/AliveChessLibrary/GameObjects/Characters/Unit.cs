using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;


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

        private King _king;
        private FigureStore _vault;

        public Unit()
        {
            this.King = null;
            this.Vault = null;
        }

        //[Column(Name = "unit_id", Storage = "_unitId", CanBeNull = false, 
        //    DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true)]
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

        //[Column(Name = "unit_count", Storage = "_unitCount", CanBeNull = false, 
        //    DbType = Constants.DB_INT)]
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


        //[Column(Name = "unit_type", Storage = "_unitType", CanBeNull = false, DbType = Constants.DB_INT)]
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
            get { return _king; }
            set { _king = value; }
        }

        public FigureStore Vault
        {
            get { return _vault; }
            set { _vault = value; }
        }
    }
}
