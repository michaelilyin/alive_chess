using AliveChessLibrary.Commands.CastleCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers.CastleHandlers
{
    class CreateBuildingResponseHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            CreateBuildingResponse response = (CreateBuildingResponse)command;
            lock (GameCore.Instance.World.Player.King.CurrentCastle.BuildingManager)
            {
                GameCore.Instance.World.Player.King.CurrentCastle.BuildingManager.SetProductionQueue(response.BuildingQueue);
            }
            GameCore.Instance.Network.Messages.AddMessage("Create building started");
        }
    }
}
