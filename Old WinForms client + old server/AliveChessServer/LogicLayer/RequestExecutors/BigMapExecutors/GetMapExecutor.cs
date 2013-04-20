using System.Collections.Generic;
using System.Linq;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class GetMapExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
     
        public GetMapExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        #region IExecutor Members

        public void Execute(Message msg)
        {
            GetMapRequest request = (GetMapRequest)msg.Command;

            Player player = msg.Sender;
            Map map = player.Level.Map;
            Level level = _environment.LevelManager.GetLevelById(map.Id);

            // получаем опорные точки
            List<BasePoint> basePoints = map.BasePoints.ToList();
            // получаем стационарные малые объекты
            List<MapPoint> points = level.BigMapRoutine.GetSinglePointObjects(map);
            // получаем стационарные большие объекты
            List<MapSector> sectors = level.BigMapRoutine.GetMultyPointObjects(map);

            // обновление области видимости игрока
            player.UpdateVisibleSpace(_environment.VisibleSpaceManager
                .GetVisibleSpace(player.King, player.IsSuperUser));
            // отправляем карту
            player.Messenger.SendNetworkMessage(
                new GetMapResponse2D(map.Id, map.SizeX, map.SizeY, points, sectors, basePoints));
            player.Ready = true; // игрок готов получать информацию о изменениях на карте
        }

        #endregion
    }
}
