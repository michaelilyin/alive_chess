using System;
using System.Collections.Generic;
using System.Data.Linq;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.LogicLayer.Environment.Alliances
{
    /// <summary>
    /// империя
    /// </summary>
    public class Empire : Union
    {
        private int _empireId;
        private int _leaderId;
        private int _storeId;
        private int? _levelId;
        private int _unionId;

        private EntityRef<Union> _union;
        private EntityRef<Level> _level;
        private EntitySet<King> _kings;
        private EntityRef<Store> _store;
        private EntityRef<Leader> _leader;

        //private Tax _tax;
        private BallotBox _ballotBox;

        private int _taxRate;
        private bool _isVoteStarted;
        private bool _withoutLeader;
        private bool _isImpeachmentStarted;
        private bool _isTakeTax;

        private TimeSpan _startTimeOfTaxPeriod;
        private TimeSpan _currentTimeWithoutLeader;
        private TimeSpan _timeSinceImpeachmentStarting;

        private static TimeSpan _timeWithoutLeader = TimeSpan.FromSeconds(10);
        private static TimeSpan _timePayTaxPeriod = TimeSpan.FromSeconds(10);

        public Empire()
        {
            _level = default(EntityRef<Level>);
            _kings = new EntitySet<King>(AttachKing, DetachKing);

            _startTimeOfTaxPeriod = TimeSpan.Zero;
            _currentTimeWithoutLeader = TimeSpan.Zero;
            _timeSinceImpeachmentStarting = TimeSpan.Zero;
        }

        public Empire(Union union)
            : this()
        {
            this.Union = union;
            this._unionId = union.Id;
            this._empireId = union.Id;
            this._levelId = union.LevelId;
           
            this._store = default(EntityRef<Store>);
            this._leader = default(EntityRef<Leader>);

            this._kings.Assign(union.Kings);
            this._level.Entity = union.Level;

            this._isTakeTax = true;
        }

        #region Methods

        public override void DoLogic(GameTime time)
        {
            // сбор налога
            if (CurrentTimePeriod > TaxPeriod)
                TakeTax();
            else CurrentTimePeriod += TimeSpan.FromMilliseconds(10);

            // ход импичмента
            if (this.IsImpeachment)
            {
                this.TimeSinceVoteStarting += TimeSpan.FromMilliseconds(10);
                if (this.TimeSinceVoteStarting > Union.VoteTime)
                    FinishExile();
            }

            // ход голосования
            if (this.IsVoteInProgress)
            {
                if (this.TimeSinceVoteStarting > Union.VoteTime)
                    FinishVote();
                else this.TimeSinceVoteStarting += TimeSpan.FromMilliseconds(10);
            }

            // подсчет времени без лидера
            if (this.WithoutLeader)
                this.CurrentTimeWithoutLeader += TimeSpan.FromMilliseconds(10);

            if (this.CurrentTimeWithoutLeader > Empire.TimeWithoutLeader)
                if (DowngradeEvent != null)
                    DowngradeEvent(this);
        }

        public override void AddMember(King king)
        {
            Union.RemoveMember(king);
            this._kings.Add(king);
        }

        public override void RemoveMember(King king)
        {
            _kings.Remove(king);
        }

        public override IEnumerable<King> NextMember()
        {
            for (int i = 0; i < _kings.Count; i++)
                yield return _kings[i];
        }

        /// <summary>
        /// собрать налог
        /// </summary>
        public void TakeTax()
        {
            // время отсчета для сбора налогов
            _startTimeOfTaxPeriod = TimeSpan.Zero;

            for (int i = 0; i < _kings.Count; i++)
            {
                // получаем требуемый ресурс
                Resource resource = _kings[i].StartCastle.ResourceStore.GetResource(ResourceTypes.Gold);
                // берем нужное количество ресурса у короля
                int count = _taxRate*resource.CountResource/100;
                if (count > 0)
                {
                    Resource pushedResource = _kings[i].StartCastle.ResourceStore
                        .PushResource(resource.ResourceType, count);
                    // добавляем ресурс в хранилище империи
                    _store.Entity.AddResource(pushedResource);
                    // отправляем сообщение о взимании налога
                    if (TaxEvent != null)
                        TaxEvent(_kings[i], pushedResource);
                }
            }
        }

        /// <summary>
        /// начало голосования
        /// </summary>
        /// <param name="candidate"></param>
        public override void StartVote(King candidate)
        {
            this._isVoteStarted = true;
            this.TimeSinceVoteStarting = TimeSpan.Zero;
            this._ballotBox = new BallotBox(this);
            this._ballotBox.Candidate = candidate;
        }

        /// <summary>
        /// завершение голосования. Генерация события
        /// обработчиком которого является AlianceRoutine
        /// </summary>
        public override void FinishVote()
        {
            this._isVoteStarted = false;
            this.TimeSinceVoteStarting = TimeSpan.Zero;
            if (VoteFinishEvent != null)
                VoteFinishEvent(this);
        }

        /// <summary>
        /// начало процедуры импичмента
        /// </summary>
        public void StartExile()
        {
            this._isImpeachmentStarted = true;
            this.TimeSinceImpeachmentStarting = TimeSpan.Zero;
            this._ballotBox = new BallotBox(this);
            this._ballotBox.Candidate = _leader.Entity;
        }

        /// <summary>
        /// завершение импичмента. Генерация события
        /// обработчиком которого является AlianceRoutine
        /// </summary>
        public void FinishExile()
        {
            this._isImpeachmentStarted = false;
            this.TimeSinceImpeachmentStarting = TimeSpan.Zero;
            if (ImpeachmentFinishedEvent != null)
                ImpeachmentFinishedEvent(this);
        }

        public override void DestroyBallotBox()
        {
            Union.DestroyBallotBox();
        }

        public override void PublishNews(NewsType type, string message)
        {
            Union.PublishNews(type, message);
        }

        public override void PublishNews(Player sender, NewsType type, string message)
        {
            Union.PublishNews(sender, type, message);
        }

        public override void PublishNews(NewsType type, string message, 
            Func<King, bool> expression)
        {
            Union.PublishNews(type, message, expression);
        }

        public override void PublishNews(Player sender, NewsType type, string message, 
            Func<King, bool> expression)
        {
            Union.PublishNews(sender, type, message, expression);
        }

        public void SendMessageToLeader(Player sender, NewsType type, string message)
        {
            Publisher p = new Publisher(PlayerManager);
            p.AddReceiver(_leader.Entity);
            p.PublishNews(sender, type, message);
        }

        #endregion

        #region Attach and Detach Handlers

        private void AttachKing(King entity)
        {
            entity.EmpireId = this.Id;
            entity.Player.Community = this;
        }

        private void DetachKing(King entity)
        {
            entity.EmpireId = null;
            entity.Player.Community = null;
        }

        #endregion

        #region Properties

        //public Tax Tax
        //{
        //    get { return _tax; }
        //    set { _tax = value; }
        //}

        public int TaxRate
        {
            get { return _taxRate; }
            set
            {
                if (value >= 0 && value <= 100)
                    _taxRate = value;
                else throw new InvalidOperationException("Tax rate must be more then 0 and less then 100");
            }
        }

        public bool WithoutLeader
        {
            get { return _withoutLeader; }
            set { _withoutLeader = value; }
        }

        public bool IsImpeachment
        {
            get { return _isImpeachmentStarted; }
            set { _isImpeachmentStarted = value; }
        }

        public override bool IsVoteInProgress
        {
            get { return _isVoteStarted; }
            set { _isVoteStarted = value; }
        }

        public static TimeSpan TimeWithoutLeader
        {
            get { return Empire._timeWithoutLeader; }
        }

        public TimeSpan CurrentTimeWithoutLeader
        {
            get { return _currentTimeWithoutLeader; }
            set { _currentTimeWithoutLeader = value; }
        }

        public static TimeSpan TaxPeriod
        {
            get { return Empire._timePayTaxPeriod; }
        }

        public override bool AllowStartVote
        {
            get { return this._withoutLeader && !this.IsImpeachment && !this.IsVoteInProgress; }
        }

        public bool AllowStartImpeachment
        {
            get { return !this._withoutLeader && !this.IsImpeachment && !this.IsVoteInProgress; }
        }

        public TimeSpan CurrentTimePeriod
        {
            get { return _startTimeOfTaxPeriod; }
            set { _startTimeOfTaxPeriod = value; }
        }

        public override TimeSpan TimeSinceVoteStarting
        {
            get { return Union.TimeSinceVoteStarting; }
            set { Union.TimeSinceVoteStarting = value; }
        }

        public TimeSpan TimeSinceImpeachmentStarting
        {
            get { return _timeSinceImpeachmentStarting; }
            set { _timeSinceImpeachmentStarting = value; }
        }

        public override BallotBox BallotBox
        {
            get { return this._ballotBox; }
        }

        public override PlayerManager PlayerManager
        {
            get { return Union.PlayerManager; }
            set { Union.PlayerManager = value; }
        }

        public override AllianceStatus Status
        {
            get { return AllianceStatus.Empire; }
        }

        public bool IsTakeTax
        {
            get { return _isTakeTax; }
            set { _isTakeTax = value; }
        }

        public Store Store
        {
            get
            {
                return this._store.Entity;
            }
            set
            {
                if (_store.Entity != value)
                {
                    if (value != null)
                    {
                        _storeId = value.Id;
                        _store.Entity = value;
                    }
                }
            }
        }

        public Leader Leader
        {
            get
            {
                return this._leader.Entity;
            }
            set
            {
                if (_leader.Entity != value)
                {
                    if (value != null)
                    {
                        _leaderId = value.Id;
                        _leader.Entity = value;
                    }
                }
            }
        }

        public Union Union
        {
            get
            {
                return this._union.Entity;
            }
            set
            {
                if (_union.Entity != value)
                {
                    if (value != null)
                    {
                        _unionId = value.Id;
                        _union.Entity = value;
                    }
                }
            }
        }

        public override EntitySet<King> Kings
        {
            get { return _kings; }
            set { this._kings.Assign(value); }
        }

        public override int Id
        {
            get
            {
                return this._empireId;
            }
            set
            {
                if (this._empireId != value)
                {
                    this._empireId = value;
                }
            }
        }

        public override int? LevelId
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

        public int UnionId
        {
            get
            {
                return this._unionId;
            }
            set
            {
                if (this._unionId != value)
                {
                    if (this._union.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._unionId = value;
                }
            }
        }

        public int LeaderId
        {
            get
            {
                return _leaderId;
            }
            set
            {
                if (this._leaderId != value)
                {
                    this._leaderId = value;
                }
            }
        }

        public int StoreId
        {
            get
            {
                return _storeId;
            }
            set
            {
                if (this._storeId != value)
                {
                    this._storeId = value;
                }
            }
        }

        /// <summary>
        /// ссылка на уровень
        /// </summary>
        public override Level Level
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
                        value.Emperies.Add(this);
                        _levelId = value.Id;
                        _level.Entity = value;
                    }
                }
            }
        }

        #endregion

        public delegate void EmpireHandler(Empire sender);
        public delegate void TaxHandler(King king, Resource resource);

        public event EmpireHandler DowngradeEvent;
        public new event EmpireHandler VoteFinishEvent;
        public event EmpireHandler ImpeachmentFinishedEvent;
        public event TaxHandler TaxEvent;
    }
}
