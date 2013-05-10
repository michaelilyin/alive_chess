using System.Collections.Generic;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetBuildingQueueExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public GetBuildingQueueExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;

        }
        public void Execute(Message cmd)
        {
            Player player = cmd.Sender;
            GetBuildingQueueRequest request = (GetBuildingQueueRequest)cmd.Command;

            GetBuildingQueueResponse response = new GetBuildingQueueResponse();
            response.BuildingQueue = player.King.CurrentCastle.BuildingManager.BuildingQueue;
            player.Messenger.SendNetworkMessage(response);
        }
    }
}
