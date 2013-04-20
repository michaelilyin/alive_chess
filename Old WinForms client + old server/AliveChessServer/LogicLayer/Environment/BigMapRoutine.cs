using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Threading;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Objects;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.Environment
{
    public class BigMapRoutine : IRoutine
    {
        private Map _map;
        private GameData _context;
        private PlayerManager _playerManager;
        private VisibleSpaceManager _vsManager;
       
        private object _mapSync = new object();

        public BigMapRoutine(Level level, GameData data)
        {
            Debug.Assert(level.Map != null);

            this._map = level.Map;
            this._context = data;
            this._vsManager = new VisibleSpaceManager(data);
        }

        public void DoLogic(GameTime time)
        {
            foreach (King king in _map.NextKing())
                king.Update();

            foreach (Mine mine in _map.NextMine())
                mine.DoWork(time.Time);
        }

        /// <summary>
        /// получение объектов занимающих одну ячейку
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public List<MapPoint> GetSinglePointObjects(Map map)
        {
            List<MapPoint> result = new List<MapPoint>();
            EntitySet<SingleObject> obj = map.SingleObjects;
            foreach (SingleObject o in obj) result.Add(o.ViewOnMap);
            return result;
        }

        /// <summary>
        ///  получение объектов занимающих несколько ячеек
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public List<MapSector> GetMultyPointObjects(Map map)
        {
            List<MapSector> result = new List<MapSector>();
            foreach (Mine o in map.Mines) result.Add(o.ViewOnMap);
            foreach (Castle o in map.Castles) result.Add(o.ViewOnMap);
            foreach (MultyObject o in map.MultyObjects) result.Add(o.ViewOnMap);
            return result;
        }

        /// <summary>
        /// отправка сообщения об изменения состояния карты
        /// </summary>
        /// <param name="map"></param>
        /// <param name="king"></param>
        /// <param name="mObject"></param>
        public void SendMapStateExchange(King king, Map map, MapPoint mObject)
        {
            EntitySet<King> kings = map.Kings;
            foreach (King k in map.NextKing())
            {
                if (!k.Equals(king.Id) && k.Player.VisibleSpace.Contains(mObject.X, mObject.Y))
                {
                    IPlayer player = k.Player;
                    if (!player.Bot && player.Ready)
                        player.Messenger.SendNetworkMessage(new UpdateWorldMessage(mObject));
                }
            }
        }

        /// <summary>
        /// отправка области видимости при достижении королем 
        /// пункта назначения
        /// </summary>
        /// <param name="player"></param>
        public void SendUpdateSectorAction(King king)
        {
            // получение информации об игроке
            IPlayer player = king.Player;

            // обновление области видимости объекта
            king.UpdateVisibleSpace(_vsManager.GetVisibleSpace(king));

            // обновление области видимости игрока
            if (!king.Player.IsSuperUser) 
                player.UpdateVisibleSpace(_vsManager.GetVisibleSpace(king, false));

            // получение объектов из области видимости
            List<MapPoint> objects = _vsManager.GetObjectsInVisibleSpace(king.VisibleSpace, 
                king.Player.IsSuperUser);

            // получение секторов из области видимости
            List<MapSector> sectors = _vsManager.GetSectorsInVisibleSpace(king.VisibleSpace, 
                king.Player.IsSuperUser);

            // отправляем область видимости
            player.Messenger.SendNetworkMessage(new GetObjectsResponse(objects, sectors));
        }

        #region Executors

        /// <summary>
        /// захват шахты королем
        /// </summary>
        /// <param name="player"></param>
        /// <param name="objMine"></param>
        public void SendCaptureMineAction(King player, MapSector objMine)
        {
            Map map = player.Map;
            Mine mine = map.SearchMineByPointId(objMine.Id);

            if ((mine.Player == null) || (mine.King.Id != player.Id && !mine.King.Sleep))
            {
                if (mine.Player != null && mine.Player.Id != player.Id)
                {
                    mine.King.Player.Messenger.SendNetworkMessage(new LooseMineMessage(mine.Id));
                    mine.King.RemoveMine(mine);
                }

                player.AddMine(mine);
                mine.King.Player = player.Player;

                player.Player.Messenger.SendNetworkMessage(new CaptureMineResponse(mine));

                mine.Activation();
            }
        }

        //public void SendComeInCastleAction(King player, ILocation objCastle)
        //{
        //    Castle castle = player.Map.SearchCastleById(objCastle.Id);
        //    if (castle.Player != null && castle.Player.Id == player.Id)
        //    {
        //        int castleId = castle.Id;
        //        player.ComeInCastle(castle);
        //        player.Player.Messenger.SendNetworkMessage(new ComeInCastleResponse(castleId));
        //    }
        //}

        public void SendCollectResourceAction(King player, MapPoint resource)
        {
            Map map = player.Map;
            Resource r = map.SearchResourceByPointId(resource.Id);
            if (r != null)
            {
                map.RemoveResource(r);
                player.Player.Messenger.SendNetworkMessage(new GetResourceMessage(r, false));
            }
        }

        #endregion

        public PlayerManager PlayerManager
        {
            get { return _playerManager; }
            set { _playerManager = value; }
        }

        public VisibleSpaceManager VisibleSpaceManager
        {
            get { return _vsManager; }
        }
    }
}
