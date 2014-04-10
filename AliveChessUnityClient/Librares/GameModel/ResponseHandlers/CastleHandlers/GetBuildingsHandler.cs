using AliveChessLibrary.Commands.CastleCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers.CastleHandlers
{
    class GetBuildingsHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            GetBuildingsResponse responce = (GetBuildingsResponse)command;
            GameCore.Instance.World.Player.King.CurrentCastle.SetBuildings(responce.Buildings);
        }
    }
}
