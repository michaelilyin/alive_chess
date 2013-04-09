using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Characters
{
    /// <summary>
    /// наместник
    /// </summary>
    [ProtoContract]
    public class Vicegerent
    {
        [ProtoMember(1)]
        private int _vicegerentId;
        private string _vicegerentName;
        private int _castleId;

        private FigureStore _figureStore;
        private ResourceStore _resourceStore;
     
        private Castle _castle;

        public Vicegerent()
        {
            Castle = null;
        }

        public void getArmyCastlToVicegerent()
        {
            //for (int i = 0; i < _castle.Entity.ArmyInsideCastle.Count; i++)
            //{
            //    _units.Add(_castle.Entity.ArmyInsideCastle[i]);
            //}
        }

        public void getArmyVicegerentToArmyCastle()
        {
            //for (int i = 0; i < _units.Count; i++)
            //{
            //    _castle.Entity.ArmyInsideCastle.Add(_units[i]);
            //}
        }

        private void AttachVicegerent(Castle cas)
        {
            cas.Vicegerent = this;
        }

        private void DetachVicegerent(Castle cas)
        {
            cas.Vicegerent = null;
        }

        public FigureStore FigureStore
        {
            get { return _figureStore; }
            set { _figureStore = value; }
        }

        public ResourceStore ResourceStore
        {
            get { return _resourceStore; }
            set { _resourceStore = value; }
        }

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

        //[Column(Name = "vicegerent_id", Storage = "_vicegerentId", CanBeNull = false,
        //    DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._vicegerentId;
            }
            set
            {
                if (this._vicegerentId != value)
                {
                    this._vicegerentId = value;
                }
            }
        }

        //[Column(Name = "castle_id", Storage = "_castleId", CanBeNull = false, DbType = Constants.DB_INT)]
        public int CastleId
        {
            get
            {
                return this._castleId;
            }
            set
            {
                if (this._castleId != value)
                {
                    this._castleId = value;
                }
            }
        }

        //[Column(Name = "vicegerent_name", Storage = "_vicegerentName", CanBeNull = false, DbType = "varchar(20)")]
        public string Name
        {
            get
            {
                return this._vicegerentName;
            }
            set
            {
                if (this._vicegerentName != value)
                {
                    this._vicegerentName = value;
                }
            }
        }
    }
}
