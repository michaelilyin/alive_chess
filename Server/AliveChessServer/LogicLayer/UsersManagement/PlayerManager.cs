using System;
using System.Collections.Generic;
using AliveChessLibrary;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Mathematic.GeometryUtils;
using AliveChessLibrary.Net;
using AliveChessLibrary.Utility;
using AliveChessServer.DBLayer;
using AliveChessServer.DBLayer.Loaders;
using AliveChessServer.LogicLayer.AI;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Alliances;
using AliveChessServer.NetLayer;
using BehaviorAILibrary;

namespace AliveChessServer.LogicLayer.UsersManagement
{
    public class PlayerManager : IRoutine
    {
        private class PlayerInfo
        {
            private Player _player;
            private King _king;
            private Castle _castle;
            private ConnectionInfo _connection;

            public PlayerInfo(King king, Player player, Castle castle,
                ConnectionInfo connectionInfo)
            {
                this._king = king;
                this._player = player;
                this._castle = castle;
                this._connection = connectionInfo;
            }

            public King King
            {
                get { return _king; }
                set { _king = value; }
            }

            public Player Player
            {
                get { return _player; }
                set { _player = value; }
            }

            public Castle Castle
            {
                get { return _castle; }
                set { _castle = value; }
            }

            public ConnectionInfo Connection
            {
                get { return _connection; }
                set { _connection = value; }
            }
        }

        private readonly ILevelLoader _loader;
        private readonly TimeManager _timeManager;
        private ProtoBufferTransport _transport;
        private AliveChessLogger _logger;
        private readonly LevelRoutine _levelManager;

        private readonly List<Player> _players = new List<Player>();

        private readonly Queue<PlayerInfo> _registerQueue = new Queue<PlayerInfo>();
        private readonly Queue<PlayerInfo> _authorizeQueue = new Queue<PlayerInfo>();

        private readonly object _registerQueueSync = new object();
        private readonly object _authorizeQueueSync = new object();

        public PlayerManager(GameWorld environment, TimeManager timeManager,
            AliveChessLogger logger)
        {
            this._logger = logger;
            this._timeManager = timeManager;
            this._loader = environment.LevelLoader;
            this._levelManager = environment.LevelManager;
        }

        public bool SearchPlayerInDataBase(string login, string password)
        {
            return _loader.FindPlayer(x => x.Login == login && x.Password == password) != null;
        }

        public void LogInPlayer(Player player)
        {
            this._players.Add(player);
        }

        public void LogOutPlayer(Player player)
        {
            player.Ready = false;
            player.King.OutOfGame();
            this._players.Remove(player);
            Level level = (Level)player.Level;
            level.RemovePlayer(player);

            // удаляем союз при выходе одного из игроков (заглушка)
            IAlliance u = level.EmpireManager.GetAlianceByMember(player.King);
            if (u != null)
            {
                if (u.Status == AllianceStatus.Union)
                    level.RemoveUnion(u as Union);
                if (u.Status == AllianceStatus.Empire)
                {
                    Empire e = u as Empire;
                    e.IsTakeTax = false;
                    level.RemoveEmpire(e);
                }
                level.EmpireManager.Remove(u);
            }
        }

        public Leader UpgratePlayerToLeader(Player player)
        {
            player.King = new Leader(player.King);
            return player.King as Leader;
        }

        public void DowngradePlayerToKing(Player player)
        {
            if (player.King is Leader)
                player.King = (player.King as Leader).King;
        }

        public Player GetPlayerInfoById(int playerId)
        {
            int index = _players.FindIndex(x => x.Id == playerId);
            return index >= 0 ? _players[index] : null;
        }

        public Player GetPlayerInfoByKingId(int kingId)
        {
            int index = _players.FindIndex(x => x.King.Id == kingId);
            return index >= 0 ? _players[index] : null;
        }

        public void Register(Identity identity, ConnectionInfo connectionInfo)
        {
            Level level = _levelManager.GetLevelByType(LevelTypes.Easy);
            Castle castle = level.Map.SearchFreeCastle();
            if (castle != null)
            {
                castle.IsAttached = true;

                King king = new King(identity.Login);

                Player player = new Player(level, identity.Login, identity.Password);

                PlayerInfo storage = new PlayerInfo(king, player, castle, connectionInfo);

                lock (_registerQueueSync)
                    _registerQueue.Enqueue(storage);
            }
            else throw new AliveChessException("No free casltes on specified map");
        }

        public void Authorize(Identity identity, ConnectionInfo connectionInfo)
        {
            Player player = _loader.LoadPlayer(identity);
            if (player != null)
            {
                Level level = (Level)player.Level;
                player.Ready = true;
                player.Time = new GameTime();
                this._timeManager.AddTime(player.Time);

                Memory m = new Memory();
                King king = player.King;
                king.Evaluator = new Evaluator(m);

                if (king.Castles.Count > 0)
                {
                    PlayerInfo storage = new PlayerInfo(king, player,
                        king.Castles[0], connectionInfo);

                    lock (_authorizeQueueSync)
                        _authorizeQueue.Enqueue(storage);

                    //не нужно, клиент сам запросит объекты
                    //king.ChangeMapStateEvent  += level.BigMapRoutine.UpdatePointState;
                    //king.UpdateSectorEvent    += level.BigMapRoutine.UpdateSectorState;
                    king.CollectResourceEvent += level.BigMapRoutine.CollectResource;
                    king.CaptureMineEvent += level.BigMapRoutine.CaptureMine;
                }
                else
                {
                    _transport.Send(connectionInfo.Socket, new ErrorMessage("Castle not found"));
                }
            }
            else
            {
                _transport.Send(connectionInfo.Socket, new ErrorMessage("Player doesn't exist"));
            }
        }

        public void Ban(Player player, string message)
        {
            player.Messenger.SendNetworkMessage(new ErrorMessage("You was baned because " + message));
        }

        public Animat AddAnimat(Level level)
        {
            Animat player = new Animat();
            //player.Id = guid.Guid;
            player.Map = level.Map;
            player.LevelId = level.Id;

            return player;
        }

        public BotKing AddBotKing(Animat animat)
        {
            Castle castle = animat.Map.SearchFreeCastle();
            Level level = animat.Level as Level;
            GuidIDPair guid = GuidGenerator.Instance.GeneratePair();
            int x, y;
            if (castle.X != animat.Map.SizeX - 1)
                x = castle.X + 1;
            else x = castle.X - 1;
            if (castle.Y != animat.Map.SizeY - 1)
                y = castle.Y + 1;
            else y = castle.Y - 1;

            InitializeResources(castle);

            Memory m = new Memory();
            King king = new King();
            king.Evaluator = new Evaluator(m);
         
            king.Map = level.Map;
            //king.GameData = _data;
            king.Id = guid.Id;
            king.AttachStartCastle(castle);
            king.MaxSpeed = 1;
            king.MaxForce = 100;
            king.Velocity = new Vector2D(1, 1);
            king.Position = new Vector2D(king.X, king.Y);

            BotKing botKing = new BotKing(animat, king, animat.Teacher);
            animat.AddBot(botKing);

            king.CreateArmy();

            king.AddView(king.Map.GetObject(x, y));

            king.ChangeMapStateEvent    += level.BigMapRoutine.UpdatePointState;
            king.ContactWithCastleEvent += animat.ContactCastle;
            king.ComeInCastleEvent      += animat.ComeInCastle;
            king.CollectResourceEvent   += animat.CollectResource;
      
            return botKing;
        }

        private void InitializeResources(Castle castle)
        {
            GuidIDPair guid;

            ResourceStore resourceStore = new ResourceStore();
            
            //vault.Id = castle.Id;
            //vault.Id = castle.DbId;

            guid = GuidGenerator.Instance.GeneratePair();
            Resource gold = new Resource();
            //gold.Id = guid.Guid;
            //gold.Id = guid.Id;
            gold.Quantity = 100;
            gold.ResourceType = ResourceTypes.Gold;
            resourceStore.AddResource(gold);

            guid = GuidGenerator.Instance.GeneratePair();
            guid = GuidGenerator.Instance.GeneratePair();
            Resource wood = new Resource();
            //wood.Id = guid.Guid;
            //wood.Id = guid.Id;
            wood.Quantity = 50;
            wood.ResourceType = ResourceTypes.Wood;
            resourceStore.AddResource(wood);

            guid = GuidGenerator.Instance.GeneratePair();
            Resource stone = new Resource();
            //gold.Id = guid.Guid;
            //gold.Id = guid.Id;
            stone.Quantity = 10;
            stone.ResourceType = ResourceTypes.Stone;
            resourceStore.AddResource(stone);

            guid = GuidGenerator.Instance.GeneratePair();
            Resource iron = new Resource();
            //wood.Id = guid.Guid;
            //wood.Id = guid.Id;
            iron.Quantity = 5;
            iron.ResourceType = ResourceTypes.Iron;
            resourceStore.AddResource(iron);

            guid = GuidGenerator.Instance.GeneratePair();
            Resource coal = new Resource();
            //wood.Id = guid.Guid;
            //wood.Id = guid.Id;
            coal.Quantity = 15;
            coal.ResourceType = ResourceTypes.Coal;
            resourceStore.AddResource(coal);

            castle.King.ResourceStore = resourceStore;
        }

        public void Update()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].Waiting && !_players[i].King.IsMove &&
                    _players[i].Time.Elapsed > TimeSpan.FromMilliseconds(500))
                {
                    _players[i].Waiting = false;
                }
            }
        }

        public ProtoBufferTransport Transport
        {
            get { return _transport; }
            set { _transport = value; }
        }

        public bool HasNewPlayers
        {
            get { return _registerQueue.Count > 0 || _authorizeQueue.Count > 0; }
        }

        public void HandleNewPlayer()
        {
            if (_registerQueue.Count > 0)
                CompleteRegistration();
            if (_authorizeQueue.Count > 0)
                CompleteAuthorization();
        }

        private void CompleteRegistration()
        {
            lock (_registerQueueSync)
            {
                while (_registerQueue.Count > 0)
                {
                    PlayerInfo storage = _registerQueue.Dequeue();

                    _loader.SavePlayer(storage.Player);

                    storage.King.Map = storage.Player.Level.Map;
                    storage.King.Player = storage.Player;

                    int mapSizeX = storage.Player.Map.SizeX;
                    int mapSizeY = storage.Player.Map.SizeY;

                    int castleWidth = storage.Castle.Width;
                    int castleHeight = storage.Castle.Height;

                    storage.King.X
                        = storage.Castle.X != mapSizeX - 1
                              ? storage.Castle.X - 1
                              : storage.Castle.X + castleWidth + 1;

                    storage.King.Y
                        = storage.Castle.Y + castleHeight != mapSizeY - 1
                              ? storage.Castle.Y + castleHeight + 1
                              : storage.Castle.Y - 1;

                    _loader.SaveKing(storage.King);

                    storage.King.AttachStartCastle(storage.Castle);

                    _loader.CommitAllChanges();

                    RegisterResponse response = new RegisterResponse();
                    response.IsSuccessed = true;
                    Transport.Send(storage.Connection.Socket, response);
                }
            }
        }

        private void CompleteAuthorization()
        {
            lock (_authorizeQueueSync)
            {
                while (_authorizeQueue.Count > 0)
                {
                    PlayerInfo storage = _authorizeQueue.Dequeue();

                    storage.Castle.Map = storage.Player.Map;
                    storage.King.AttachStartCastle(storage.Castle);

                    storage.King.AddView(storage.Player.Map
                                             .GetObject(storage.King.X, storage.King.Y));
                    storage.Player.Map.AddKing(storage.Player.King);

                    storage.Connection.Player = storage.Player;
                    storage.Player.Connection = storage.Connection;
                    storage.Player.Messenger = new Messenger(_transport, storage.Connection);

                    LogInPlayer(storage.Player);
                    storage.Player.IsAuthorized = true;
                    storage.Player.Messenger.SendNetworkMessage(
                        new AuthorizeResponse(true, true, "", storage.Player.King.Id));

                    //_logger.Log(
                    //    storage.Player.Login, "Authorization", storage.King.Id.ToString(),
                    //    storage.Connection.ToString(), storage.King.X + ":" + storage.King.Y,
                    //    storage.King.StartCastle.X + ":" + storage.King.StartCastle.Y);
                }
            }
        }
    }
}
