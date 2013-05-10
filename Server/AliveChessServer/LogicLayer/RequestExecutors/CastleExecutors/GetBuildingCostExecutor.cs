using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetBuildingCostExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
      
        public GetBuildingCostExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            GetBuildingCostRequest request = (GetBuildingCostRequest)msg.Command;
            Player player = msg.Sender;
            CreationRequirements requirements = player.King.CurrentCastle.BuildingManager.GetCreationRequirements(request.InnerBuildingType);
            GetBuildingCostResponse response = new GetBuildingCostResponse {BuildingCost = requirements};
            msg.Sender.Messenger.SendNetworkMessage(response);
        }

    }
}
