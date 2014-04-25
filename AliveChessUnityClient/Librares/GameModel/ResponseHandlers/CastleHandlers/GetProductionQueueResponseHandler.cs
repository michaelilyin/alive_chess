using AliveChessLibrary.Commands.CastleCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers.CastleHandlers
{
    class GetProductionQueueResponseHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            GetProductionQueueResponse response = (GetProductionQueueResponse)command;
            lock (GameCore.Instance.World.Player.King.CurrentCastle.BuildingManager)
            {
                GameCore.Instance.World.Player.King.CurrentCastle.BuildingManager.SetProductionQueue(response.BuildingQueue);
                GameCore.Instance.World.Player.King.CurrentCastle.RecruitingManager.SetProductionQueue(response.RecruitingQueue);
            }
        }
    }
}
