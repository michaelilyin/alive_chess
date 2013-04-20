using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Net;
using AliveChessLibrary.Utility;

namespace AliveChessServer.LogicLayer.UsersManagement
{
    [Table(Name = "dbo.player")]
    public class Player : IPlayer
    {
        #region Variables

        private int _playerId;
        private string _playerLogin;
        private string _playerPassword;

        private int _kingId;
        private int _levelId;
        private EntityRef<King> _king;
        private EntityRef<ILevel> _level;

        private Map _map;
        private VisibleSpace _sector;
        //private PlayerInfo _information;
        private ConnectionInfo _connection;
        private IMessenger _messenger;

        #endregion

        #region Constructors

        public Player()
        {
            this._king = default(EntityRef<King>);
            this._level = default(EntityRef<ILevel>);
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

        public bool HasKing(int kingId)
        {
            return this.King.Id == kingId;
        }

        public King GetKing()
        {
            return this._king.Entity;
        }

        public King GetKing(int kingId)
        {
            return HasKing(kingId) ? _king.Entity : null;
        }

        public void AddKing(King king)
        {
            //this._kingId = king.DbId;
            this._king.Entity = king;
        }

        public void RemoveKing(King king)
        {
            if (this._king.Entity == king)
                this._king.Entity.Player = null;
        }

        #endregion

        #region Propeties

        public Map Map
        {
            get { return _map; }
            set
            {
                _map = value;
                this._sector = new VisibleSpace(value);
            }
        }

        public bool Ready { get; set; }

        public bool IsSuperUser { get; set; }

        public bool Bot { get { return false; } }

        public VisibleSpace VisibleSpace
        {
            get { return _sector; }
            set { _sector = value; }
        }

        public IMessenger Messenger
        {
            get { return _messenger; }
            set { _messenger = value; }
        }

        public ConnectionInfo Connection
        {
            get { return _connection; }
            set { _connection = value; }
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

        public ILevel Level
        {
            get
            {
                return this._level.Entity;
            }
            set
            {
                if (_level.Entity != value)
                {
                    if (value != null)
                    {
                        _level.Entity = value;
                        _levelId = value.Id;
                    }
                }
            }
        }

        [Column(Name = "player_id", Storage = "_playerId", CanBeNull = false, DbType = Constants.DB_INT, 
            IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
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

        [Column(Name = "level_id", Storage = "_levelId", CanBeNull = false, DbType = Constants.DB_INT,
            UpdateCheck = UpdateCheck.Never)]
        public int LevelId
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

        [Column(Name = "player_login", Storage = "_playerLogin", CanBeNull = false, DbType = "varchar(20)",
            UpdateCheck = UpdateCheck.Never)]
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

        [Column(Name = "player_password", Storage = "_playerPassword", CanBeNull = false, DbType = "varchar(20)",
            UpdateCheck = UpdateCheck.Never)]
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

        #endregion
    }
}
