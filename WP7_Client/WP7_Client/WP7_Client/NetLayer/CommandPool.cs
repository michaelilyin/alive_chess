using System.Collections.Generic;
using System.Threading;
using AliveChessLibrary.Commands;

namespace WP7_Client.NetLayer
{
    public class CommandPool
    {
        private readonly Queue<ICommand> _commands;

        public CommandPool()
        {
            _commands = new Queue<ICommand>();
        }

        public void Enqueue(ICommand command)
        {
            lock (_commands)
                _commands.Enqueue(command);
        }

        public ICommand Dequeue()
        {
            ICommand command;
            lock (_commands)
                command = _commands.Dequeue();
            return command;
        }

        public int Count
        {
            get { return _commands.Count; }
        }
    }
}
