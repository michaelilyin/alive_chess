using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameLogic.CommandHandlers
{
    class CaptureMineHandler : CommandHandler
    {
        public CaptureMineHandler()
            : base(Command.CaptureMineResponse)
        {

        }

        public override void Handle(ICommand command)
        {
            CaptureMineResponse res = (CaptureMineResponse)command;
        }
    }
}
