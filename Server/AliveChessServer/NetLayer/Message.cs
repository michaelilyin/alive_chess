using System.Net;
using AliveChessLibrary.Commands;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.NetLayer
{
    public class Message
    {
        private Player sender;
        private ICommand command;
        private IPEndPoint remoteEndPoint;

        public Player Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        public ICommand Command
        {
            get { return command; }
            set { command = value; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return remoteEndPoint; }
            set { remoteEndPoint = value; }
        }
    }
}
