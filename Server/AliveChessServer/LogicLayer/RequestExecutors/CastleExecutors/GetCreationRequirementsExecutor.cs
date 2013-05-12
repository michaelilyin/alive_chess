using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
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
            //GetCreationRequirementsRequest request = (GetCreationRequirementsRequest)msg.Command;
            Player player = msg.Sender;
            GetCreationRequirementsResponse response = new GetCreationRequirementsResponse();
            response.BuildingRequirements = new Dictionary<InnerBuildingType, CreationRequirements>();
            foreach (var item in player.King.CurrentCastle.BuildingManager.CreationRequirements)
            {
                response.BuildingRequirements.Add(item.Key, item.Value);
            }
            response.RecruitingRequirements = new Dictionary<UnitType, CreationRequirements>();
            foreach (var item in player.King.CurrentCastle.RecruitingManager.CreationRequirements)
            {
                response.RecruitingRequirements.Add(item.Key, item.Value);
            }
            msg.Sender.Messenger.SendNetworkMessage(response);
        }

    }
}
