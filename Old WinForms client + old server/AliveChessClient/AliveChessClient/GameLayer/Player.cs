using System;
using System.Data.Linq;
using AliveChessLibrary;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.GameLayer
{
    public class Player : IPlayer
    {
        #region Variables

        private int _playerId;
        private string _playerLogin;
        private string _playerPassword;

        private int _kingId;
        private int _levelId;
        private EntityRef<King> _king;

        private Map _map;
        private VisibleSpace _sector;

        #endregion

        #region Constructors

        public Player()
        {
            this._king = default(EntityRef<King>);
        }

        #endregion

        #region Methods

        private void InitTest()
        {
            if (_king.Entity == null)
                throw new AliveChessException("Object is not initialized");
        }

        public void UpdateVisibleSpace(VisibleSpace sector)
        {
            this._sector = sector;
        }

        public virtual bool HasKing(int kingId)
        {
            return this.Id == kingId;
        }

        public virtual King GetKing()
        {
            return null;
        }

        public virtual King GetKing(int kingId)
        {
            return HasKing(kingId) ? _king.Entity : null;
        }

        public virtual void AddKing(King king)
        {
            this._kingId = king.Id;
            this._king.Entity = king;
        }

        public virtual void RemoveKing(King king)
        {

        }

        #endregion

        #region Propeties

        public virtual int Id
        {
            get
            {
                InitTest();
                return _king.Entity.Id;
            }
        }

        public Map Map
        {
            get { return _map; }
            set
            {
                _map = value;
                this._sector = new VisibleSpace(value);
            }
        }

        public bool InGame { get; set; }

        public bool IsSuperUser { get; set; }

        public VisibleSpace VisibleSpace
        {
            get { return _sector; }
            set { _sector = value; }
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
                    if (value != null)
                    {
                        _king.Entity = value;
                        value.Player = this;
                    }
                    else
                    {
                        throw new NullReferenceException("King must be not null");
                    }
                }
            }
        }

        public virtual int DbId
        {
            get
            {
                return this._playerId;
            }
            set
            {
                if (this._playerId != value)
                {
                    this._playerId = value;
                }
            }
        }

        public virtual int LevelId
        {
            get
            {
                return this._levelId;
            }
            set
            {
                if (this._levelId != value)
                {
                    this._levelId = value;
                }
            }
        }

        public string Login
        {
            get
            {
                return this._playerLogin;
            }
            set
            {
                if (this._playerLogin != value)
                {
                    this._playerLogin = value;
                }
            }
        }

        public string Password
        {
            get
            {
                return this._playerPassword;
            }
            set
            {
                if (this._playerPassword != value)
                {
                    this._playerPassword = value;
                }
            }
        }

        public bool Ready { get; set; }

        #endregion

        #region IPlayer Members


        public bool Bot
        {
            get { return false; }
        }

        public IMessenger Messenger
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ILevel Level
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
