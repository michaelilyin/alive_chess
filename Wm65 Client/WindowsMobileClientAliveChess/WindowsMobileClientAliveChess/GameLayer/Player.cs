using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using AliveChessLibrary;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Statistics;
using AliveChessLibrary.Interaction;
using GeneralSysLibrary;

namespace WindowsMobileClientAliveChess.GameLayer
{
    public class Player : IPlayer
    {
        #region Variables

        private int _playerId;
        private string _playerLogin;
        private string _playerPassword;

        private int _kingId;
        private int _levelId;
        private King _king;

        private Map _map;
        private VisibleSpace _sector;
        private Statistic _stat;
        private ICommunity _comm;

        #endregion

        #region Constructors

        public Player()
        {
            this._king = default(King);
        }

        #endregion

        #region Methods

        private void InitTest()
        {
            if (_king == null)
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
            return HasKing(kingId) ? _king : null;
        }

        public virtual void AddKing(King king)
        {
            this._kingId = king.Id;
            this._king = king;
        }

        public virtual void RemoveKing(King king)
        {

        }

        public void AddVisibleSector(IVisibleSpace space)
        {
            _sector = space as VisibleSpace;
        }

        public void RemoveVisibleSector(IVisibleSpace space)
        {
            _sector = null;
        }

        #endregion

        #region Propeties

        public virtual int Id
        {
            get
            {
                InitTest();
                return _king.Id;
            }
        }

        public Map Map
        {
            get { return _map; }
            set
            {
                _map = value;
                this._sector = new VisibleSpace(_king);
            }
        }

        public bool InGame { get; set; }

        public bool IsSuperUser { get; set; }

        public IVisibleSpace VisibleSpace
        {
            get { return _sector; }
            set { _sector = value as VisibleSpace; }
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

        public Statistic Statistics
        {
            get
            {
                return _stat;
            }
            set
            {
                _stat = value;
            }
        }

        public ICommunity Community
        {
            get
            {
                return _comm;
            }
            set
            {
                _comm = value;
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
