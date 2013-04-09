using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Utility;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.LogicLayer.Environment.Alliances
{
    public class Union : IAlliance
    {
        private int _unionId;
        private EntityRef<Level> _level;
        private int? _levelId;
        private EntitySet<King> _kings;

        private bool _isVoteStarted;
        private PlayerManager _playerManager;
        private TimeSpan _timeSinceVoteStarting;

        private BallotBox _ballotBox;

        private object _kingsSync = new object();

       // private static TimeSpan _voteTime = TimeSpan.FromHours(5);
        private static TimeSpan _voteTime = TimeSpan.FromSeconds(15);

        private Func<Player, bool> _memberChecker;

        public Union()
        {
            _level = default(EntityRef<Level>);
            _kings = new EntitySet<King>(AttachKing, DetachKing);
        }

        /// <summary>
        /// жизненный цикл союза
        /// </summary>
        /// <param name="time"></param>
        public virtual void DoLogic(GameTime time)
        {
            if (this.IsVoteInProgress)
            {
                this.TimeSinceVoteStarting += TimeSpan.FromMilliseconds(10);
                if (this.TimeSinceVoteStarting > Union.VoteTime)
                    FinishVote();
            }
        }

        public virtual void AddMember(King king)
        {
            lock (_kingsSync)
            {
                _kings.Add(king);
            }
        }

        public virtual void RemoveMember(King king)
        {
            lock (_kingsSync)
            {
                _kings.Remove(king);
            }
        }

        /// <summary>
        /// начало выборов
        /// </summary>
        public virtual void StartVote(King candidate)
        {
            this._isVoteStarted = true;
            this.TimeSinceVoteStarting = TimeSpan.Zero;
            this._ballotBox = new BallotBox(this);
            this._ballotBox.Candidate = candidate;
        }

        /// <summary>
        /// завершение выборов
        /// </summary>
        public virtual void FinishVote()
        {
            this._isVoteStarted = false;
            this.TimeSinceVoteStarting = TimeSpan.Zero;
            if (VoteFinishEvent != null)
                VoteFinishEvent(this);
        }

        public virtual void DestroyBallotBox()
        {
            this._ballotBox = null;
        }

        public virtual IEnumerable<King> NextMember()
        {
            for (int i = 0; i < _kings.Count; i++)
                yield return _kings[i];
        }

        /// <summary>
        /// отправить новость всем игрокам в империи
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public virtual void PublishNews(NewsType type, string message)
        {
            Publisher p = new Publisher(_playerManager);
            p.AddReceivers(_kings.ToList());
            p.PublishNews(null, type, message);
        }

        /// <summary>
        /// отправить новость всем игрокам кроме отправителя
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public virtual void PublishNews(Player sender, NewsType type, string message)
        {
            Func<King, bool> predicate = x => x.Id != sender.King.Id;
            List<King> receivers = _kings.Filter(predicate);
            if (receivers != null)
            {
                Publisher p = new Publisher(_playerManager);
                p.AddReceivers(receivers);
                p.PublishNews(sender, type, message);
            }
        }

        /// <summary>
        /// отправить новость всем игрокам удовлетворяющих условию кроме отправителя
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public virtual void PublishNews(Player sender, NewsType type, string message,
            Func<King, bool> expression)
        {
            List<King> receivers = new List<King>();
            for (int i = 0; i < _kings.Count; i++)
                if (_kings[i] != sender.King && expression(_kings[i]))
                    receivers.Add(_kings[i]);

            if (receivers.Count != 0)
            {
                Publisher p = new Publisher(_playerManager);
                p.AddReceivers(receivers);
                p.PublishNews(sender, type, message);
            }
        }

        /// <summary>
        /// отправить новость всем игрокам удовлетворяющих условию включая отправителя
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public virtual void PublishNews(NewsType type, string message,
            Func<King, bool> expression)
        {
            List<King> receivers = _kings.Filter(expression);
            if (receivers != null)
            {
                Publisher p = new Publisher(_playerManager);
                p.AddReceivers(receivers);
                p.PublishNews(null, type, message);
            }
        }

        #region Attach and Detach Handlers

        private void AttachKing(King entity)
        {
            entity.UnionId = this.Id;
            entity.Player.Community = this;
        }

        private void DetachKing(King entity)
        {
            entity.UnionId = null;
            entity.Player.Community = null;
        }

        #endregion

        #region Properties

        public virtual bool IsVoteInProgress
        {
            get { return _isVoteStarted; }
            set { _isVoteStarted = value; }
        }

        public virtual AllianceStatus Status
        {
            get { return AllianceStatus.Union; }
        }

        public virtual PlayerManager PlayerManager
        {
            get { return _playerManager; }
            set { _playerManager = value; }
        }

        public virtual TimeSpan TimeSinceVoteStarting
        {
            get { return _timeSinceVoteStarting; }
            set { _timeSinceVoteStarting = value; }
        }

        public static TimeSpan VoteTime
        {
            get { return Union._voteTime; }
            set { Union._voteTime = value; }
        }

        public virtual BallotBox BallotBox
        {
            get { return _ballotBox; }
        }

        public virtual EntitySet<King> Kings
        {
            get { return _kings; }
            set { _kings.Assign(value); }
        }

        public virtual bool AllowStartVote
        {
            get { return !this.IsVoteInProgress; }
        }

        //[Column(Name = "union_id", Storage = "_unionId", CanBeNull = false, DbType = Constants.DB_INT,
        //    IsPrimaryKey = true, IsDbGenerated = true)]
        public virtual int Id
        {
            get
            {
                return this._unionId;
            }
            set
            {
                if (this._unionId != value)
                {
                    this._unionId = value;
                }
            }
        }

        public virtual double RequestPower()
        {
            // fake
            return Power;
        }

        public virtual double RequestWealth()
        {
            // fake
            return Wealth;
        }

        //[Column(Name = "level_id", Storage = "_levelId", CanBeNull = true, DbType = Constants.DB_INT)]
        public virtual int? LevelId
        {
            get
            {
                return this._levelId;
            }
            set
            {
                if (this._levelId != value)
                {
                    if (this._level.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._levelId = value;
                }
            }
        }

        //[Association(Name = "fk_union_level", Storage = "_level", ThisKey = "LevelId", IsForeignKey = true)]
        public virtual Level Level
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
                        value.Unions.Add(this);
                        _level.Entity = value;
                        _levelId = value.Id;
                    }
                }
            }
        }

        #endregion

        public delegate void UnionHandler(Union union);

        public event UnionHandler VoteFinishEvent;

        // fake
        public double Power { get; set; }
        public double Wealth { get; set; }
    }
}
