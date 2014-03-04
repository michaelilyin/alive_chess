using AliveChessLibrary.Commands.BigMapCommand;
using Logger;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers
{
    class MoveKingResponseHandler : CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            MoveKingResponse response = (MoveKingResponse)command;
            GameCore.Instance.World.SetPlayerSteps(response);
        }
    }
}
