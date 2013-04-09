using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;
#if !UNITY_EDITOR
using System.Data.Linq;
#endif

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
     
#if !UNITY_EDITOR
        private EntityRef<Castle> _castle;
#else
        private Castle _castle;
#endif

        public Vicegerent()
        {
#if !UNITY_EDITOR
            _castle = new EntityRef<Castle>();
#else
            Castle = null;
#endif
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

#if !UNITY_EDITOR
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
#else
        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }
#endif

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
