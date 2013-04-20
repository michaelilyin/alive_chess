using System;
using System.Collections.Generic;
using System.Diagnostics;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Mathematic.GeometryUtils;
using AliveChessLibrary.Utility;
using AliveChessServer.DBLayer;
using AliveChessServer.DBLayer.Loaders;
using AliveChessServer.LogicLayer.AI;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Aliances;

namespace AliveChessServer.LogicLayer.UsersManagement
{
    public class PlayerManager
    {
        private GameData _data;
        private LevelRoutine _levelManager;
        private GameWorld _environment;
        private List<Player> _players;
        private DataBaseMapManager _dbLoader;

        public PlayerManager(GameWorld environment)
        {
            Debug.Assert(environment.DbLoader != null);

            this._environment = environment;
            this._levelManager = environment.LevelManager;
            this._data = _levelManager.GameData;
            this._dbLoader = environment.DbLoader;
            this._players = new List<Player>();
        }

        public bool SearchPlayerInDataBase(string login, string password)
        {
            return false;
            //return _dbLoader.CheckPlayer(login, password);
        }

        public void LogInPlayer(Player player)
        {
            _players.Add(player);
            //info.Level.AddPlayer(info);
        }

        public void LogOutPlayer(Player player)
        {
            player.Ready = false;
            player.King.OutOfGame();
            this._players.Remove(player);
            Level level = player.Level as Level;
            level.RemovePlayer(player);

            // удаляем союз при выходе одного из игроков (заглушка)
            IAliance u = level.EmpireManager.GetAlianceByMember(player.King);
            if (u != null)
            {
                if (u.Status == AlianceStatus.Union)
                    level.RemoveUnion(u as Union);
                if (u.Status == AlianceStatus.Empire)
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
            return index > -1 ? _players[index] : null;
        }

        public Player GetPlayerInfoByKingId(int kingId)
        {
            int index = _players.FindIndex(x => x.King.Id == kingId);
            return index > -1 ? _players[index] : null;
        }

        public List<Player> GetPlayersByLambda(Func<Player, bool> predicate)
        {
            List<Player> result = new List<Player>();
            _players.ForEach(
                x =>
                {
                    if (predicate(x))
                        result.Add(x);
                });
            return result;
        }

        public Level LoadLevel(Player player)
        {
            return null;
            //return _dbLoader.LoadLevel(player);
        }

        /// <summary>
        /// поиск игрока в режиме online
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private Player Search(Identity identity)
        {
            return _players.FindElement(
                x => x.Login == identity.Login && x.Password == identity.Password
                );
        }

        /// <summary>
        /// регистрация игрока на сервере включает:
        /// 1 - создание игрока
        /// 2 - создание короля
        /// 3 - создание хранилища ресурсов для короля
        /// 4 - создание представления короля на карте
        /// 5 - инициализация созданных обхектов
        /// </summary>
        /// <param name="username">имя пользователя</param>
        /// <param name="password">пароль</param>
        public void Register(string username, string password)
        {
            King king = new King();
            Player player = new Player();
            ResourceStore vault = new ResourceStore();
            Level level = _levelManager.EasyLevel;

            GuidIDPair guid = GuidGenerator.Instance.GeneratePair();

            //player.Id = guid;
            player.LevelId = level.Id;
            player.Login = username;
            player.Password = password;
            player.Map = level.Map;
            player.King = king;

            //vault.Id = player.Id;
            Castle castle = level.Map.SearchFreeCastle();

           // king.DbId = player.Id;
            king.Id = guid.Id;
            king.Name = player.Login;
            king.Experience = 0;
            king.MilitaryRank = 0;
            castle.ResourceStore = vault;
            king.Map = level.Map;
            king.AttachStartCastle(castle);
            king.GameData = _data;

            int x, y;
            if (castle.X != level.Map.SizeX - 1)
                x = castle.X + 1;
            else x = castle.X - 1;
            if (castle.Y != level.Map.SizeY - 1)
                y = castle.Y + 1;
            else y = castle.Y - 1;

            ImageInfo img = new ImageInfo();
            img.ImageId = 8;

            MapPoint view = King.CreateView(x, y, img, _data);

            //_environment.DbLoader.Context.Players.InsertOnSubmit(player);
            //_environment.DbLoader.Context.ResourceStores.InsertOnSubmit(vault);
            //_environment.DbLoader.Context.MapPoints.InsertOnSubmit(view);

            //_environment.DbLoader.Context.SubmitChanges();
        }

        /// <summary>
        /// авторизация в игре для зарегистрированных пользователей
        /// </summary>
        /// <param name="username">имя пользователя</param>
        /// <param name="password">пароль</param>
        /// <returns></returns>
        public Player AuthorizeInGame(Identity identity, Level level)
        {
           // Register(username, password);
            if (Search(identity) != null)
                return null;
            /*----------------------FAKE------------------------------*/
            GuidIDPair k_id = GuidGenerator.Instance.GeneratePair();
            GuidIDPair mp_id = GuidGenerator.Instance.GeneratePair();

            Player player = new Player();
           // player.Id = guid.Guid;
            player.Ready = true;
            player.Map = level.Map;
            player.LevelId = level.Id;

            Castle castle = player.Map.SearchFreeCastle();

            int x, y;
            if (castle.X != player.Map.SizeX - 1)
                x = castle.X + 1;
            else x = castle.X - 1;
            if (castle.Y != player.Map.SizeY - 1)
                y = castle.Y + 1;
            else y = castle.Y - 1;

            InitializeResources(castle);

            ImageInfo img = new ImageInfo();
            img.ImageId = 8;

            MapPoint view = King.CreateView(x, y, img, _data);
            view.Id = mp_id.Id;

            King king = new King(view);
            king.GameData = _data;
            king.Id = k_id.Id;
            //king.DbId = player.Id;
            player.King = king;
            king.Player = player;
            king.AttachStartCastle(castle);
            king.Map = player.Map;
   
            king.CreateArmy(delegate() { return GuidGenerator.Instance.GeneratePair(); });
            /*-----------------------------------------------------------*/

            //Player player = _dbLoader.LoadPlayer(username, password);
            //King king = player.King;
            //player.Map = king.Map;
            //player.InGame = true;
            //king.GameData = _levelManager.Context;
            //king.StartCastle = king.Castles.First();

            king.AddView(king.ViewOnMap);

            king.ChangeMapStateEvent += // обработка события изменения сектора карты
                new King.ChangeMapStateHandler(level.BigMapRoutine.SendMapStateExchange);
            //king.ContactWithCastleEvent += new King.ContactWithCastleHandler(_environment.BigMapRoutine.SendContactWithCastleAction);
            king.CaptureMineEvent += new King.CaptureMineHandler(level.BigMapRoutine.SendCaptureMineAction);
            //king.ComeInCastleEvent += new King.ComeInCastleHandler(_environment.BigMapRoutine.SendComeInCastleAction);
            //king.ContactWithKingEvent += new King.ContactWithKingHandler(_environment.BigMapRoutine.SendContactWithKingAction);
            king.CollectResourceEvent += new King.CollectResourceHandler(level.BigMapRoutine.SendCollectResourceAction);
            king.UpdateSectorEvent += new King.UpdateSectorHandler(level.BigMapRoutine.SendUpdateSectorAction);

            king.Map.AddKing(king);

            return player;
        }

        public Animat AddAnimat(Level level)
        {
            GuidIDPair guid = GuidGenerator.Instance.GeneratePair();

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

            ImageInfo img = new ImageInfo();
            img.ImageId = 8;

            MapPoint view = King.CreateView(x, y, img, _data);

            BotKing king = new BotKing(animat.Map, view, _data, animat.Teacher);
            king.GameData = _data;
            king.Id = guid.Id;
            king.ViewOnMap.Id = guid.Id;
            //king.DbId = animat.Id;
            animat.AddBot(king);
            king.Player = animat;
            king.AttachStartCastle(castle);
            king.MaxSpeed = 1;
            king.MaxForce = 100;
            king.Velocity = new Vector2D(1, 1);
            king.Position = new Vector2D(king.X, king.Y);

            king.CreateArmy(delegate() { return GuidGenerator.Instance.GeneratePair(); });

            king.AddView(king.ViewOnMap);

            king.ChangeMapStateEvent += // обработка события изменения сектора карты
                new King.ChangeMapStateHandler(level.BigMapRoutine.SendMapStateExchange);
            king.ContactWithCastleEvent += new King.ContactWithCastleHandler(animat.ContactCastle);
            //king.CaptureMineEvent += new King.CaptureMineHandler(level.MRoutine.SendCaptureMineAction);
            king.ComeInCastleEvent += new King.ComeInCastleHandler(animat.ComeInCastle);
            //king.ContactWithKingEvent += new King.ContactWithKingHandler(_environment.BigMapRoutine.SendContactWithKingAction);
            king.CollectResourceEvent += new King.CollectResourceHandler(animat.CollectResource);
            //king.UpdateSectorEvent += new King.UpdateSectorHandler(level.MRoutine.SendUpdateSectorAction);

            //king.Map.AddKing(king);

            return king;
        }

        private void InitializeResources(Castle castle)
        {
            GuidIDPair guid;

            ResourceStore vault = new ResourceStore();
            
            //vault.Id = castle.Id;
            //vault.Id = castle.DbId;

            guid = GuidGenerator.Instance.GeneratePair();
            Resource gold = new Resource();
            //gold.Id = guid.Guid;
            //gold.Id = guid.Id;
            gold.CountResource = 100;
            gold.ResourceType = ResourceTypes.Gold;
            vault.AddResourceToRepository(gold);

            guid = GuidGenerator.Instance.GeneratePair();
            guid = GuidGenerator.Instance.GeneratePair();
            Resource wood = new Resource();
            //wood.Id = guid.Guid;
            //wood.Id = guid.Id;
            wood.CountResource = 50;
            wood.ResourceType = ResourceTypes.Wood;
            vault.AddResourceToRepository(wood);

            castle.ResourceStore = vault;
        }

        private void InitializeFigures(Castle castle)
        {
            
        }

        public List<Player> Players
        {
            get { return _players; }
        }
    }
}
