using AliveChessLibrary.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.GameLogic.Network
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
            Debug.Log(String.Format("Put command {0}", command.Id));
            lock (_commands)
                _commands.Enqueue(command);
            _event.Set();
            Debug.Log(String.Format("Pool: In queue {0} commands", _commands.Count));
        }

        public ICommand Dequeue()
        {
            ICommand command = null;
            lock (_commands)
                command = _commands.Dequeue();
            Debug.Log(String.Format("Pop command {0}", command.Id));
            return command;
        }

        public int Count
        {
            get { return _commands.Count; }
        }

        public void Wait()
        {
            Debug.Log("Start wait");
            _event.WaitOne();
            Debug.Log("End wait");
        }
    }
}
