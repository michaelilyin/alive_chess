///////////////////////////////////////////////////////////
//  Animat.cs
//  Implementation of the Class Animat
//  Generated by Enterprise Architect
//  Created on:      16-���-2009 0:11:06
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using AliveChessLibrary;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Statistics;
using AliveChessLibrary.Utility;
using BehaviorAILibrary;
using BehaviorAILibrary.DecisionLayer;
using BehaviorAILibrary.DecisionLayer.NeuralNetwork;
//using ConversationAILibrary;
using AliveChessLibrary.Interfaces;

namespace AliveChessServer.LogicLayer.AI
{
    [Table(Name = "dbo.player")]
    public class Animat : IPlayer
    {
        private int _playerId;
        private int _levelId;
        private IList<BotKing> bots;

        private Map _map;
        private CompositeVisibleSpace _sector;
        private IMessenger _messenger;

        private bool _isActive;

        private Statistic _statistics;
        private NeuroTeacher _teacher;
        private EntityRef<ILevel> _level;
        private EntitySet<NeuralNetwork> _networks;
        //private EntitySet<ConversationContext> _conversations;

        private object _botsSync = new object();

        public Animat()
        {
            this.bots = new List<BotKing>();
            this._level = default(EntityRef<ILevel>);
            this._teacher = new NeuroTeacher();
            this._networks = new EntitySet<NeuralNetwork>();
           // this._conversations = new EntitySet<ConversationContext>();

            this._sector = new CompositeVisibleSpace();

            NeuralNetwork network = new NeuralNetwork(7, 15, 8);
            network.LearningRate = 0.1;
            this._networks.Add(network);

            //_messenger = new Messenger(this);
        }

        public void TeachBots()
        {
            //Teacher.Teach(_networks[0]);
            foreach (var botKing in Kings)
                botKing.AttachNeuralNetwork(_networks[0]);
        }

        private void CreateConversation()
        {
            //
        }

        /// <summary>
        /// send message to animat
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="stimulus"></param>
        public void Tell(BotKing receiver, IStimulus stimulus)
        {
            //receiver.FSM.FireUp(stimulus);
        }

        public void AddVisibleSector(IVisibleSpace space)
        {
            _sector.AddSector(space);
        }

        public void RemoveVisibleSector(IVisibleSpace space)
        {
            _sector.RemoveSector(space);
        }

        public void AddBot(BotKing bot)
        {
            lock (_botsSync)
            {
                bots.Add(bot);
                _sector.AddSector(bot.VisibleSpace);
            }
        }

        public void RemoveBot(BotKing bot)
        {
            lock (_botsSync)
            {
                bots.Remove(bot);
                _sector.RemoveSector(bot.VisibleSpace);
            }
        }

        public bool HasKing(int kingId)
        {
            return bots.Exists(x => x.Id == kingId);
        }

        public King GetKing()
        {
            if (bots.Count > 0) return bots[0];
            else throw new AliveChessException("Animat hasn't kings");
        }

        public BotKing GetKingById(int kingId)
        {
            if (HasKing(kingId))
                return bots.Search(x => x.Id == kingId);
            throw new InvalidOperationException("Bot with specified ID not found");
        }

        public void AddKing(King king)
        {
            if (king is BotKing)
            {
                BotKing bot = king as BotKing;
                lock (_botsSync)
                    bots.Add(bot);
            }
            else throw new InvalidCastException("BotKing expected");
        }

        public void RemoveKing(King king)
        {
            if (king is BotKing)
            {
                BotKing bot = king as BotKing;
                lock (_botsSync)
                    bots.Remove(bot);
            }
            else throw new InvalidCastException("BotKing expected");
        }

        public void ComeInCastle(King player, MapSector point)
        {
            Debug.Assert(point.Owner != null);
            Castle castle = player.Map.SearchCastleById(point.Owner.Id);
            if (castle != null)
                player.ComeInCastle(castle);
        }

        public void ContactCastle(King player, MapSector point)
        {
            //Castle castle = player.Map.SearchCastleByPointId(point.Id);
            //if (castle != null)
            //    player.ComeInCastle(castle);
        }

        public void CollectResource(King player, MapPoint point)
        {
            Resource resource = player.Map.SearchResourceById(point.Owner.Id);
            if (resource != null)
            {
                player.Map.RemoveResource(resource);
                player.ResourceStore.AddResourceToStore(resource);
            }
        }

        public bool Ready { get; set; }

        public bool IsSuperUser { get; set; }

        public bool Bot { get { return true; } }

        public IMessenger Messenger
        {
            get { return _messenger; }
            set { _messenger = value; }
        }

        public IList<BotKing> Kings
        {
            get { return this.bots; }
            set { this.bots = value; }
        }

        public NeuroTeacher Teacher
        {
            get { return _teacher; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        public IVisibleSpace VisibleSpace
        {
            get { return _sector; }
        }

        #region Not Supported
       
        public IUser User
        {
            get { throw new NotSupportedException("Animat hasn't user"); }
            set { throw new NotSupportedException("Animat hasn't user"); }
        }

        public ICommunity Community
        {
            get { throw new NotSupportedException("Different bots may belong to different communities"); }
            set { throw new NotSupportedException("Different bots may belong to different communities"); }
        }

        #endregion

        public Statistic Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        //[Column(Name = "animat_id", Storage = "_animatId", CanBeNull = false, DbType = Constants.DB_INT,
        //  IsPrimaryKey = true, IsDbGenerated = true)]
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
                    }
                }
            }
        }

        //[Column(Name = "level_id", Storage = "_levelId", CanBeNull = false, DbType = Constants.DB_INT)]
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
    }//end Animat

}//end namespace BotsLogic