using System.Collections.Generic;
using System.Diagnostics;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class GetObjectsExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private VisibleSpaceManager _vsManager;

        public GetObjectsExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            //Debug.Assert((_vsManager = gameLogic.Environment.BigMapRoutine.VisibleSpaceManager) != null);
        }

        public void Execute(Message msg)
        {
            GetObjectsRequest request = (GetObjectsRequest)msg.Command;

            List<MapPoint> objects = null;
            List<MapSector> sectors = null;

            Player player = msg.Sender;
            Level level = _environment.LevelManager.GetLevelById(player.LevelId);
            this._vsManager = level.BigMapRoutine.VisibleSpaceManager;

            // получаем область видимости игрока)
            if (!request.ForConcreteObserver)
            {
                // получение объектов из области видимости
                objects = level.BigMapRoutine.VisibleSpaceManager
                    .GetObjectsInVisibleSpace(player.VisibleSpace, player.IsSuperUser);

                // получение секторов из области видимости
                sectors = level.BigMapRoutine.VisibleSpaceManager
                    .GetSectorsInVisibleSpace(player.VisibleSpace, player.IsSuperUser);
            }
            else // получаем область видимости конкретного объекта
            {
                // получение объекта на карте
                IObserver observer = player.Level.Map
                    .SearchObserverById(request.ObserverId, request.ObserverType);

                // обновление области видимости объекта
                observer.UpdateVisibleSpace(_vsManager.GetVisibleSpace(observer));

                if (!player.IsSuperUser)
                    player.UpdateVisibleSpace(_vsManager.GetVisibleSpace(player.King, false));

                // получение объектов из области видимости
                objects = level.BigMapRoutine.VisibleSpaceManager
                    .GetObjectsInVisibleSpace(observer.VisibleSpace, player.IsSuperUser);

                // получение секторов из области видимости
                sectors = level.BigMapRoutine.VisibleSpaceManager
                    .GetSectorsInVisibleSpace(observer.VisibleSpace, player.IsSuperUser);
            }

            if (objects != null || sectors != null)
                player.Messenger.SendNetworkMessage(new GetObjectsResponse(objects, sectors));
        }
    }
}
