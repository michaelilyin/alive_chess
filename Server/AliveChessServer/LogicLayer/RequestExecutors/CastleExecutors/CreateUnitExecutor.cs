using System.Linq;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessServer.DBLayer;
using System.Collections.Generic;
using System.Data.Linq;
 

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class CreateUnitExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public CreateUnitExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message cmd)
        {
            Player player = cmd.Sender;
            CreateUnitRequest request = (CreateUnitRequest)cmd.Command;
            CreationRequirements requirements = player.King.CurrentCastle.RecruitingManager.GetCreationRequirements(request.UnitType);

            if (requirements.RequiredBuildings.Any(building => !player.King.CurrentCastle.HasBuilding(building)))
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Нет необходимых построек."));
                return;
            }
            if (!player.King.ResourceStore.HasEnoughResources(requirements.Resources))
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Недостаточно ресурсов"));
            }
            player.King.CurrentCastle.RecruitingManager.Build(request.UnitType);
            var response = new CreateUnitResponse();
            response.ProductionQueue = player.King.CurrentCastle.RecruitingManager.GetProductionQueueCopy();
            player.Messenger.SendNetworkMessage(response);

        }
    }
}
