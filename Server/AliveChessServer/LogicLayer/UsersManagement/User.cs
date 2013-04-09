using System.Data.Linq;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Net;
using AliveChessLibrary.Interfaces;

namespace AliveChessServer.LogicLayer.UsersManagement
{
    public abstract class User : IUser
    {
        protected int _playerId;
        protected int _levelId;
        protected string _playerLogin;
        protected string _playerPassword;
        protected EntityRef<ILevel> _level;
        protected ConnectionInfo _connection;
        protected IMessenger _messenger;
        protected ICommunity _community;
        protected int _discriminator;

        public User()
        {
            this._level = default(EntityRef<ILevel>);
        }

        public User(ILevel level, string login, string password)
            : this()
        {
            this.Level = level;
            this.Login = login;
            this.Password = password;
        }

        public int Discriminator
        {
            get { return _discriminator; }
            set { _discriminator = value; }
        }

        public abstract UserRole Role { get; }

        IStage IUser.Stage
        {
            get { return _level.Entity; }
        }

        ICommunity IUser.Community
        {
            get { return _community; }
        }

        IConnectionInfo IUser.Connection
        {
            get { return _connection; }
        }

        public ICommunity Community
        {
            get { return _community; }
            set { _community = value; }
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
    }
}
