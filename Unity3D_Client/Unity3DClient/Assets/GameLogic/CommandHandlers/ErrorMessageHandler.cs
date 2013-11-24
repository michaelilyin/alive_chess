using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.ErrorCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameLogic.CommandHandlers
{
    class ErrorMessageHandler : CommandHandler
    {
        public ErrorMessageHandler()
            : base(Command.AuthorizeResponse)
        {
        }

        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            ErrorMessage res = (ErrorMessage)command;
            Debug.Log(res.Message);
        }
    }
}
