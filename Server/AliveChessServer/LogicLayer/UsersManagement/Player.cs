using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Statistics;
using AliveChessServer.LogicLayer.Environment;
using AliveChessLibrary.Interfaces;

namespace AliveChessServer.LogicLayer.UsersManagement
{
    public class Player : User, IPlayer
    {
        #region Variables

        private Map _map;
        private King _king;
        private CompositeVisibleSpace _sector;
        private Statistic _statistics;

        private bool _waiting;
        private bool _isAuthorized;

        private GameTime _elapsed;

        #endregion

        #region Constructors

        public Player()
        {
            this._sector = new CompositeVisibleSpace();
        }

        public Player(ILevel level, string login, string password)
            : base(level, login, password)
        {
            this._map = level.Map;
            this._sector = new CompositeVisibleSpace();
        }

        #endregion

        #region Methods

        public void AddKing(King king)
        {
            if (this._king == null || !this._king.Equals(king))
            {
                king.Player = this;
                this._king = king;
                this._sector.AddSector(king.VisibleSpace);
            }
        }

        public void RemoveKing(King king)
        {
            if (this._king.Equals(king))
            {
                king.Player = null;
                this._king.Player = null;
                this._sector.RemoveSector(king.VisibleSpace);
            }
        }

        public void AddVisibleSector(IVisibleSpace space)
        {
            _sector.AddSector(space);
        }

        public void RemoveVisibleSector(IVisibleSpace space)
        {
            _sector.RemoveSector(space);
        }

        #endregion

        #region Propeties

        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        public King King
        {
            get
            {
                return this._king;
            }
            set
            {
                if (_king != value)
                {
                    if (value != null)
                    {
                        _king = value;
                        value.Player = this;
                    }
                    else
                    {
                        _king = null;
                        value.Player.RemoveKing(value);
                    }
                }
            }
        }

        public Statistic Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        public override UserRole Role
        {
            get { return UserRole.Player; }
        }

        public bool Ready { get; set; }

        public bool IsSuperUser { get; set; }

        public bool Bot { get { return false; } }

        public IVisibleSpace VisibleSpace
        {
            get { return _sector; }
        }

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set { _isAuthorized = value; }
        }

        public GameTime Time
        {
            get { return _elapsed; }
            set { _elapsed = value; }
        }

        public bool Waiting
        {
            get { return _waiting; }
            set { _waiting = value; }
        }

        #endregion
    }
}
