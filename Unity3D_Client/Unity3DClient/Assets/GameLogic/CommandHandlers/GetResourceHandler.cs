using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameLogic.CommandHandlers
{
    class GetResourceHandler : CommandHandler
    {
        public GetResourceHandler()
            : base(Command.GetResourceMessage)
        {

        }

        public override void Handle(ICommand command)
        {
            GetResourceMessage res = (GetResourceMessage)command;
        }
    }
}
