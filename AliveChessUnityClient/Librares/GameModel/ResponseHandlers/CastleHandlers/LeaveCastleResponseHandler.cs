using AliveChessLibrary.Commands.CastleCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers.CastleHandlers
{
    class LeaveCastleResponseHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            LeaveCastleResponse response = (LeaveCastleResponse)command;
            GameCore.Instance.World.Player.KingInKastle = false;
        }
    }
}
