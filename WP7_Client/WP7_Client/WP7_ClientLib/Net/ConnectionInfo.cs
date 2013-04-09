using System.Net;
using System.Net.Sockets;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.Net
{
    public sealed class ConnectionInfo : IConnectionInfo
    {
        private Socket socket;
        private byte[] buffer;
        private NetworkDataStream data;
        private IPlayer player;

        public ConnectionInfo(Socket socket)
        {
            this.socket = socket;
            this.data = new NetworkDataStream();
            this.buffer = new byte[2048];
            this.player = null;
        }

        public bool Equals(IPEndPoint other)
        {
            IPEndPoint thisIp = (IPEndPoint)Socket.RemoteEndPoint;
            return thisIp.Address.ToString().Equals(other.Address.ToString()) &&
                thisIp.Port == other.Port;
        }

        public Socket Socket
        {
            get { return socket; }
            set { socket = value; }
        }

        public byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public NetworkDataStream Data
        {
            get { return data; }
            set { data = value; }
        }

        public IPlayer Player
        {
            get { return player; }
            set { player = value; }
        }

        public override string ToString()
        {
            return ((IPEndPoint) socket.RemoteEndPoint).Address + ":" + ((IPEndPoint) socket.RemoteEndPoint).Port;
        }
    }
}
