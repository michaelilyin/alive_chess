using AliveChessLibrary.Commands.BigMapCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers
{
    class GetGameStateHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            GetGameStateResponse response = (GetGameStateResponse)command;
            GameCore.Instance.World.UpdateGameState(response);
        }
    }
}
