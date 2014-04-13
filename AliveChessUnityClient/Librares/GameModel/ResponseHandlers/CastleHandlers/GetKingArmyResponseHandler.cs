using AliveChessLibrary.Commands.CastleCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers.CastleHandlers
{
    class GetKingArmyResponseHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            GetKingArmyResponse response = (GetKingArmyResponse)command;
            GameCore.Instance.World.Player.King.Army.SetUnits(response.Units);
        }
    }
}
