using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.RegisterCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers
{
    class AuthorizeResponseHandler : Network.CommandHandler
    {
        public override void Handle(ICommand command)
        {
            AuthorizeResponse result = (AuthorizeResponse)command;
            if (result.IsAuthorized)
            {
                GameCore.Instance.IsAuthorized = true;
                Logger.Log.Message("Authorization successful");
            }
        }
    }
}
