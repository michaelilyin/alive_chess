using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetCreationRequirementsExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
      
        public GetCreationRequirementsExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            GetCreationRequirementsRequest request = (GetCreationRequirementsRequest)msg.Command;
            Player player = msg.Sender;
            GetCreationRequirementsResponse response = new GetCreationRequirementsResponse();
            response.BuildingRequirements = player.King.CurrentCastle.BuildingManager.CreationRequirements;
            response.RecruitingRequirements = player.King.CurrentCastle.RecruitingManager.CreationRequirements;
            msg.Sender.Messenger.SendNetworkMessage(response);
        }

    }
}
