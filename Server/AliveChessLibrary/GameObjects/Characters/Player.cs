using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Text;
using System.Threading;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Net;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Characters
{
    [ProtoContract]
    [Table(Name = "alive_chess.player")]
    public class Player  
    {
        #region Variables

        private Guid _playerId;

        [ProtoMember(1)]
        private string _playerLogin;

        [ProtoMember(2)]
        private string _playerPassword;

        private Guid _kingId;
        private Nullable<Guid> _levelId;
        private EntityRef<King> _king;
      
        private bool _inGame;
        private bool isSuperUser;

        private Map map;
        private VisibleSpace sector;

        #endregion

        #region Constructors

        public Player()
        {
            this._king = default(EntityRef<King>);
        }

        #endregion

        #region Methods

        public void UpdateVisibleSpace(VisibleSpace sector)
        {
            this.sector = sector;
        }

        #endregion

        #region Propeties

        public uint Id
        {
            get 
            {
                if (_king.Entity != null)
                    return _king.Entity.Id;
                else throw new NullReferenceException("King must be not null");
            }
        }

        public Map Map
        {
            get { return map; }
            set
            {
                map = value;
                this.sector = new VisibleSpace(value);
            }
        }

        public bool InGame
        {
            get { return _inGame; }
            set { _inGame = value; }
        }

        public bool IsSuperUser
        {
            get { return isSuperUser; }
            set { isSuperUser = value; }
        }

        public VisibleSpace VisibleSpace
        {
            get { return sector; }
            set { sector = value; }
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

        [Column(Name = "player_id", Storage = "_playerId", CanBeNull = false, DbType = "binary(16)", 
            IsPrimaryKey = true)]
        public Guid DbId
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

        [Column(Name = "king_id", Storage = "_kingId", CanBeNull = false, DbType = "binary(16)")]
        public Guid KingId
        {
            get
            {
                return this._kingId;
            }
            set
            {
                if (this._kingId != value)
                {
                    if (this._king.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._kingId = value;
                }
            }
        }

        [Column(Name = "level_id", Storage = "_levelId", CanBeNull = true, DbType = "binary(16)")]
        public Nullable<Guid> LevelId
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

        [Column(Name = "player_login", Storage = "_playerLogin", CanBeNull = false, DbType = "varchar(20)")]
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

        [Column(Name = "player_password", Storage = "_playerPassword", CanBeNull = false, DbType = "varchar(20)")]
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
