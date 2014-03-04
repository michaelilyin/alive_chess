using AliveChessLibrary.Commands.BigMapCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers
{
    class GetObjectsHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            GetObjectsResponse response = (GetObjectsResponse)command;
            GameCore.Instance.World.UpdateObjects(response);
        }
    }
}
