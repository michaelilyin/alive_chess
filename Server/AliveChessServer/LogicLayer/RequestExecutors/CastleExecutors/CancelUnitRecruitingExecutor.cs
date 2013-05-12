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
    public class CancelUnitRecruitingExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public CancelUnitRecruitingExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message cmd)
        {
            CancelUnitRecruitingRequest request = (CancelUnitRecruitingRequest)cmd.Command;
            Player player = cmd.Sender;
            King king = cmd.Sender.King;

            if (!king.CurrentCastle.RecruitingManager.HasInQueue(request.UnitType))
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Невозможно отменить найм: данного юнита нет в очереди."));
                return;
            }

            king.CurrentCastle.RecruitingManager.Destroy(request.UnitType);

            var response = new CancelUnitRecruitingResponse();
            response.ProductionQueue = king.CurrentCastle.RecruitingManager.GetProductionQueueCopy();
            player.Messenger.SendNetworkMessage(response);

        }
    }
}
