using AliveChessLibrary.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public abstract class CommandHandler
    {
        public abstract void Handle(ICommand command);
    }
}
