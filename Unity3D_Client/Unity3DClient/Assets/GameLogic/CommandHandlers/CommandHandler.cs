using AliveChessLibrary.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameLogic.CommandHandlers
{
    abstract class CommandHandler
    {
        public Command CommandID { get; private set; }

        public CommandHandler(Command id)
        {
            CommandID = id;
        }

        public abstract void Handle(ICommand command);
    }
}
