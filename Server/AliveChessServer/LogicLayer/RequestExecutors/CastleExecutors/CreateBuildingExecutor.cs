using System.Collections.Generic;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class CreateBuildingExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public CreateBuildingExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message cmd)
        {
            CreateBuildingRequest request = (CreateBuildingRequest)cmd.Command;
            Player player = cmd.Sender;
            King king = cmd.Sender.King;
#warning Переделать
            InnerBuilding building = new InnerBuilding();
            building.InnerBuildingType = request.Type;
            king.CurrentCastle.AddBuilding(building);

            var response = new CreateBuildingResponse();
            response.Buildings = new List<InnerBuilding>();
            foreach (var b in king.CurrentCastle.InnerBuildings)
            {
                response.Buildings.Add(b);
            }
            player.Messenger.SendNetworkMessage(response);

        }
    }
}
