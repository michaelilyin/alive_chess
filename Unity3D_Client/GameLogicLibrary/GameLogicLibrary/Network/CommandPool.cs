using AliveChessLibrary.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameLogicLibrary.Network
{
    public class CommandPool
    {
        private readonly Queue<ICommand> _commands;
        private readonly AutoResetEvent _event;

        public CommandPool()
        {
            _commands = new Queue<ICommand>();
            _event = new AutoResetEvent(false);
        }

        public void Enqueue(ICommand command)
        {
            lock (_commands)
                _commands.Enqueue(command);
            _event.Set();
        }

        public ICommand Dequeue()
        {
            ICommand command = null;
            lock (_commands)
                command = _commands.Dequeue();
            return command;
        }

        public int Count
        {
            get { return _commands.Count; }
        }

        public void Wait()
        {
            _event.WaitOne();
        }
    }
}
