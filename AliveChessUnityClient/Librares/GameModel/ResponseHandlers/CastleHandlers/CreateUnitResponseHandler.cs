using AliveChessLibrary.Commands.CastleCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers.CastleHandlers
{
    class CreateUnitResponseHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            CreateUnitResponse response = (CreateUnitResponse)command;
            lock (GameCore.Instance.World.Player.King.CurrentCastle.BuildingManager)
            {
                GameCore.Instance.World.Player.King.CurrentCastle.RecruitingManager.SetProductionQueue(response.ProductionQueue);
            }
            GameCore.Instance.Network.Messages.AddMessage("Recruting start");
        }
    }
}
