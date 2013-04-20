using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Utility;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Characters
{
    [ProtoContract]
    [Table(Name = "dbo.vicegerent")]
    public class Vicegerent
    {
        [ProtoMember(1)]
        private int _vicegerentId;
        private string _vicegerentName;
        private int _castleId;

        private FigureStore _figureStore;
        private ResourceStore _resourceStore;
     
        private EntityRef<Castle> _castle;

        public Vicegerent()
        {
            _castle = new EntityRef<Castle>();//(AttachVicegerent, DetachVicegerent);
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

        //public void AddUnit(Unit unit)
        //{
        //    _units.Add(unit);
        //}

        //public void RemoveUnit(Unit unit)
        //{
        //    _units.Remove(unit);
        //}

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
            get
            {
                return this._castle.Entity;
            }
            set
            {
                if (_castle.Entity != value)
                {
                    if (value != null)
                    {
                        _castle.Entity = value;
                        _castleId = value.Id;
                        value.Vicegerent = this;
                    }
                }
            }
        }

        [Column(Name = "vicegerent_id", Storage = "_vicegerentId", CanBeNull = false,
            DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true)]
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

        [Column(Name = "castle_id", Storage = "_castleId", CanBeNull = false, DbType = Constants.DB_INT)]
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

        [Column(Name = "vicegerent_name", Storage = "_vicegerentName", CanBeNull = false, DbType = "varchar(20)")]
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
