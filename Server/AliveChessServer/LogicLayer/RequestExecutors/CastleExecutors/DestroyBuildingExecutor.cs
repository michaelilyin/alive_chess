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
    public class DestroyBuildingExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public DestroyBuildingExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message cmd)
        {
            DestroyBuildingRequest request = (DestroyBuildingRequest)cmd.Command;
            Player player = cmd.Sender;
            King king = cmd.Sender.King;

            if (!king.CurrentCastle.HasBuilding(request.InnerBuildingType) && !king.CurrentCastle.BuildingManager.HasInQueue(request.InnerBuildingType))
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Невозможно разрушить здание: оно еще не было построено."));
                return;
            }

            king.CurrentCastle.BuildingManager.Destroy(request.InnerBuildingType);

            var response = new DestroyBuildingResponse();
            response.Buildings = king.CurrentCastle.GetInnerBuildingListCopy();
            player.Messenger.SendNetworkMessage(response);

        }
    }
}
