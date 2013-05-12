using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetProductionQueueExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public GetProductionQueueExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;

        }
        public void Execute(Message cmd)
        {
            Player player = cmd.Sender;
            //GetProductionQueueRequest request = (GetProductionQueueRequest)cmd.Command;

            GetProductionQueueResponse response = new GetProductionQueueResponse();
            response.BuildingQueue = player.King.CurrentCastle.BuildingManager.GetProductionQueueCopy();
            response.RecruitingQueue = player.King.CurrentCastle.RecruitingManager.GetProductionQueueCopy();
            player.Messenger.SendNetworkMessage(response);
        }
    }
}
