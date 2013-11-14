using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameLogic.CommandHandlers
{
    class MoveKingResponseHandler : CommandHandler
    {
        public MoveKingResponseHandler()
            :base(Command.MoveKingResponse)
        {

        }

        public override void Handle(ICommand command)
        {
            MoveKingResponse res = (MoveKingResponse)command;
        }
    }
}
