using AliveChessLibrary.Commands;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Network
{
    internal class CommandPool
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
            //Log.Message(String.Format("Put command {0}", command.Id));
            lock (_commands)
                _commands.Enqueue(command);
            _event.Set();
            //Log.Message(String.Format("Pool: In queue {0} commands", _commands.Count));
        }

        public ICommand Dequeue()
        {
            ICommand command = null;
            lock (_commands)
                command = _commands.Dequeue();
            //Log.Message(String.Format("Pop command {0}", command.Id));
            return command;
        }

        public int Count
        {
            get { return _commands.Count; }
        }

        public void Wait()
        {
            //Log.Message("Wait new commands");
            _event.WaitOne();
            //Log.Message("Have command");
        }
    }
}
