using System.Collections.Generic;
using System.Linq;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.Commands.ErrorCommand;
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

            if (king.CurrentCastle.HasBuilding(request.InnerBuildingType))
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Это здание уже построено."));
                return;
            }

            if (king.CurrentCastle.BuildingManager.HasUnfinishedBuilding(request.InnerBuildingType))
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Это здание уже строится."));
                return;
            }

            CreationRequirements requirements = king.CurrentCastle.BuildingManager.GetCreationRequirements(request.InnerBuildingType);
            if (!king.ResourceStore.HasEnoughResources(requirements.Resources))
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Недостаточно ресурсов."));
                return;
            }

            if (requirements.RequiredBuildings.Any(building => !king.CurrentCastle.HasBuilding(building)))
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Нет необходимых построек."));
                return;
            }

            king.CurrentCastle.BuildingManager.Build(request.InnerBuildingType);

            var response = new CreateBuildingResponse();
            response.BuildingQueue = king.CurrentCastle.BuildingManager.BuildingQueue;
            player.Messenger.SendNetworkMessage(response);

        }
    }
}
