using System;
using System.Net;
using System.Net.Sockets;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.Net
{
    public class ConnectionInfo : IEquatable<IPEndPoint>
    {
        private Object sync;
        private Socket socket;
        private byte[] buffer;
        private NerworkDataStream data;
        private IPlayer player;

        public ConnectionInfo(Socket socket)
        {
            this.socket = socket;
            this.data = new NerworkDataStream();
            this.buffer = new byte[2048];
            this.player = null;
            this.sync = new Object();
        }

        public bool Equals(IPEndPoint other)
        {
            IPEndPoint thisIp = (IPEndPoint)this.Socket.RemoteEndPoint;

            return thisIp.Address.ToString().Equals(other.Address.ToString()) &&
                thisIp.Port == other.Port;
        }

        public Object Sync
        {
            get { return sync; }
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

        public NerworkDataStream Data
        {
            get { return data; }
            set { data = value; }
        }

        public IPlayer Player
        {
            get { return player; }
            set { player = value; }
        }
    }
}
