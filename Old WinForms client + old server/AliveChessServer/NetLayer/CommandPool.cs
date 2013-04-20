using System.Collections.Generic;
using System.Threading;

namespace AliveChessServer.NetLayer
{
    public class CommandPool
    {
        private Queue<Message> _commands;

        public CommandPool()
        {
            _commands = new Queue<Message>();
        }

        public void Enqueue(Message command)
        {
            Monitor.Enter(_commands);
            _commands.Enqueue(command);
            Monitor.Exit(_commands);
        }

        public Message Dequeue()
        {
            Message command;
            Monitor.Enter(_commands);
            command = _commands.Dequeue();
            Monitor.Exit(_commands);
            return command;
        }

        public int Count
        {
            get { return _commands.Count; }
        }
    }
}
