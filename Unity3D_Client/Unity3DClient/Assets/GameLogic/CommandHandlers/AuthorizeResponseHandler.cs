using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.RegisterCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameLogic.CommandHandlers
{
    class AuthorizeResponseHandler : CommandHandler
    {
        public AuthorizeResponseHandler(Command id)
            : base(id)
        {

        }

        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            Debug.Log("COOL! I have authorisation command!");
            AuthorizeResponse response = (AuthorizeResponse)command;
            if (response.IsAuthorized)
            {
                GameCore.Instance.OnAuthorize();
            }
        }
    }
}
