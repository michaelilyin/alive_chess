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
        }

        public ConnectionInfo SearchConnection(Socket socket)
        {
            return _connections.Find(x => x.Socket.Equals(socket));
        }

        public int Count { get { return _connections.Count; } }

        public ConnectionInfo this[int index] { get { return _connections[index]; } }
    }
}
