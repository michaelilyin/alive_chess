using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;
using System.Data.Linq;

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

        private ResourceStore _resourceStore;

        private EntityRef<Castle> _castle;

        public Vicegerent()
        {
            _castle = new EntityRef<Castle>();
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
