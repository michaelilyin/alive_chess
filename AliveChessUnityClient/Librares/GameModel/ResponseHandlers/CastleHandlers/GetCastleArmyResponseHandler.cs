using AliveChessLibrary.Commands.CastleCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers.CastleHandlers
{
    class GetCastleArmyResponseHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            GetCastleArmyResponse response = (GetCastleArmyResponse)command;
            GameCore.Instance.World.Player.King.CurrentCastle.Army.SetUnits(response.Units);
        }
    }
}
