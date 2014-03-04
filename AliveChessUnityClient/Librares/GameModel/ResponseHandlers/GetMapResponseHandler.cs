using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers
{
    class GetMapResponseHandler : Network.CommandHandler
    {
        public override void Handle(ICommand command)
        {
            GetMapResponse result = (GetMapResponse)command;
            GameCore.Instance.World.Create(result);
        }
    }
}
