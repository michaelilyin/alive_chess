using AliveChessLibrary.Commands.ErrorCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers
{
    class ErrorMessageHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            ErrorMessage result = (ErrorMessage)command;
            GameCore.Instance.Network.Messages.AddMessage(String.Format("[ERROR][{0}]:{1}", DateTime.Now.ToLocalTime(), result.Message));
            Logger.Log.Error(result.Message);
        }
    }
}
