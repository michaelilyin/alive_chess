using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using AliveChessLibrary.Net;

namespace AliveChessServer.NetLayer
{
    public class ConnectionPool : IEnumerable<ConnectionInfo>
    {
        private SocketTransport _transport;
        private List<ConnectionInfo> _connections;

        public ConnectionPool(SocketTransport transport)
        {
            _transport = transport;
            _connections = new List<ConnectionInfo>();
        }

        public void Add(ConnectionInfo info)
        {
            Monitor.Enter(_connections);
            _connections.Add(info);
            Monitor.Exit(_connections);
        }

        public void Remove(ConnectionInfo info)
        {
            Monitor.Enter(_connections);
            _connections.Remove(info);
            Monitor.Exit(_connections);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ConnectionInfo> GetEnumerator()
        {
            for (int i = 0; i < _connections.Count; i++)
                yield return _connections[i];
            //return new ConnectionEnum(_connections);
        }

        public ConnectionInfo SearchConnection(Socket socket)
        {
            foreach (ConnectionInfo info in _connections)
                if (info.Socket.Equals(socket))
                    return info;
            return null;
        }

        /// <summary>
        /// класс нужен для итерации по ConnectionPool
        /// </summary>
        public class ConnectionEnum : IEnumerator
        {
            private List<ConnectionInfo> connections;
            private int position = -1;

            public ConnectionEnum(List<ConnectionInfo> connections)
            {
                this.connections = connections;
            }

            public object Current
            {
                get { return connections[position]; }
            }

            public bool MoveNext()
            {
                position++;
                return position < connections.Count;
            }

            public void Reset()
            {
                position = -1;
            }
        }
    }
}
