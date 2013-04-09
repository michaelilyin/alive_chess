using System.Collections.Generic;
using System.Threading;
using AliveChessLibrary.Commands;

namespace WindowsMobileClientAliveChess.NetLayer.Main
{
    public class CommandPool
    {
        private Queue<ICommand> commands;

        public CommandPool()
        {
            commands = new Queue<ICommand>();
        }

        public void Enqueue(ICommand command)
        {
            Monitor.Enter(commands);
            commands.Enqueue(command);
            Monitor.Exit(commands);
        }

        public ICommand Dequeue()
        {
            ICommand command;
            Monitor.Enter(commands);
            command = commands.Dequeue();
            Monitor.Exit(commands);
            return command;
        }

        public int Count
        {
            get { return commands.Count; }
        }
    }
}
