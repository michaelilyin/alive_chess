using System.Collections.Generic;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetBuildingsExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public GetBuildingsExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;

        }
        public void Execute(Message cmd)
        {
            Player player = cmd.Sender;
            GetBuildingsRequest request = (GetBuildingsRequest)cmd.Command;

            GetBuildingsResponse response = new GetBuildingsResponse();
            response.Buildings = new List<InnerBuilding>();
            foreach (var building in player.King.CurrentCastle.InnerBuildings)
            {
                response.Buildings.Add(building);
            }
            player.Messenger.SendNetworkMessage(response);
        }
    }
}
