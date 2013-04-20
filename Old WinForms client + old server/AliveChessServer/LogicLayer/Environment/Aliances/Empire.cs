using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Utility;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.Environment.Aliances
{
    /// <summary>
    /// империя
    /// </summary>
    [Table(Name = "dbo.chess_empire")]
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
        private TimeSpan _timeSinceImpeachmentStarting;
        private TimeSpan _currentTimeWithoutLeader;
        private TimeSpan _currentTimePeriod;
        //private static TimeSpan _timeWithoutLeader = TimeSpan.FromDays(5);
        private static TimeSpan _timeWithoutLeader = TimeSpan.FromSeconds(10);
        private static TimeSpan _timePayTaxPeriod = TimeSpan.FromSeconds(10);

        public Empire()
        {
            _level = default(EntityRef<Level>);
            _kings = new EntitySet<King>(AttachKing, DetachKing);
           
            _currentTimePeriod = TimeSpan.Zero;
            _timeSinceImpeachmentStarting = TimeSpan.Zero;
            _currentTimeWithoutLeader = TimeSpan.Zero;
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
            if (this.IsImpeachment)
            {
                this.TimeSinceVoteStarting += TimeSpan.FromMilliseconds(10);
                if (this.TimeSinceVoteStarting >= Union.VoteTime)
                    FinishImpeachment();
            }
            if (this.IsVote)
            {
                this.TimeSinceVoteStarting += TimeSpan.FromMilliseconds(10);
                if (this.TimeSinceVoteStarting >= Union.VoteTime)
                    FinishVote();
            }
            if (this.WithoutLeader)
                this.CurrentTimeWithoutLeader += TimeSpan.FromMilliseconds(10);
            if (this.CurrentTimeWithoutLeader >= Empire.TimeWithoutLeader)
                if (DowngradeEvent != null)
                    DowngradeEvent(this);
        }

        public override void AddMember(King king)
        {
            _kings.Add(king);
            Union.AddMember(king);
        }

        public override void RemoveMember(King king)
        {
            _kings.Remove(king);
            Union.RemoveMember(king);
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
            _kings.ForEach<King>(
                x =>
                {
                    // время отсчета для сбора налогов
                    _currentTimePeriod = TimeSpan.Zero;
                    // получаем требуемый ресурс
                    Resource resource = x.StartCastle.ResourceStore.GetResource(ResourceTypes.Gold);
                    // берем нужное количество ресурса у короля
                    int count = _taxRate * resource.CountResource / 100;
                    if (count > 0)
                    {
                        Resource pushedResource = x.StartCastle.ResourceStore
                            .PushResource(resource.ResourceType, count);
                        // добавляем ресурс в хранилище империи
                        _store.Entity.AddResource(pushedResource);
                        // отправляем сообщение о взимании налога
                        if (TaxEvent != null)
                            TaxEvent(x, pushedResource);
                    }
                });
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
        public void StartImpeachment()
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
        public void FinishImpeachment()
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
        }

        private void DetachKing(King entity)
        {
            entity.EmpireId = null;
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

        public override bool IsVote
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
            get { return this._withoutLeader && !this.IsImpeachment && !this.IsVote; }
        }

        public bool AllowStartImpeachment
        {
            get { return !this._withoutLeader && !this.IsImpeachment && !this.IsVote; }
        }

        public TimeSpan CurrentTimePeriod
        {
            get { return _currentTimePeriod; }
            set { _currentTimePeriod = value; }
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

        public override AlianceStatus Status
        {
            get { return AlianceStatus.Empire; }
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

        [Column(Name = "empire_id", Storage = "_empireId", CanBeNull = false, DbType = Constants.DB_INT,
            IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
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

        [Column(Name = "level_id", Storage = "_levelId", CanBeNull = true, DbType = Constants.DB_INT)]
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

        [Column(Name = "union_id", Storage = "_unionId", CanBeNull = false, DbType = Constants.DB_INT)]
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

        [Column(Name = "king_id", Storage = "_leaderId", CanBeNull = false, DbType = Constants.DB_INT)]
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

        [Column(Name = "store_id", Storage = "_storeId", CanBeNull = false, DbType = Constants.DB_INT)]
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
        [Association(Name = "fk_empire_level", Storage = "_level", ThisKey = "LevelId", IsForeignKey = true)]
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
