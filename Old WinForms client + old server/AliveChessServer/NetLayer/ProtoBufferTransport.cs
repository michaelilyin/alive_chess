using System.Net;
using System.Net.Sockets;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Net;
using AliveChessServer.LogicLayer;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.NetLayer
{
    public class ProtoBufferTransport
    {
        private CommandPool _commandPool;
        private SocketTransport _socketTransport;
        private PlayerManager _playerManager;
        private AliveChessLogger _logger;

        private object _decodeSync = new object();
        private object _disconnectSync = new object();
      
        public ProtoBufferTransport(CommandPool commandPool, AliveChessLogger logger, 
            PlayerManager playerManager)
        {
            this._logger = logger;
            this._commandPool = commandPool;
            this._playerManager = playerManager;
            this._socketTransport = new SocketTransport(commandPool, logger);
        }

        public void Send<T>(Socket socket, T cmd) where T : ICommand
        {
            _socketTransport.Send(ProtoBufferCodec.Encode<T>(cmd), socket);
        }

        /// <summary>
        /// декодирование сообщений в отдельном потоке
        /// </summary>
        public void Decode(object state)
        {
            lock (_decodeSync)
            {
                foreach (ConnectionInfo info in _socketTransport.Connections)
                {
                    if (info.Data.IsReady)
                    {
                        BytePackage data = info.Data.Get();
                        ICommand command = ProtoBufferCodec.Decode(data);
                        if (command != null)
                        {
                            Message msg = new Message();
                            msg.Command = command;
                            msg.Sender = info.Player as Player;
                            msg.RemoteEndPoint = info.Socket.RemoteEndPoint as IPEndPoint;
                            _commandPool.Enqueue(msg);
                        }
                        else _logger.Add("Error has occured while decoding");
                    }
                }
            }
        }

        /// <summary>
        /// отключение игроков в отдельном потоке
        /// </summary>
        public void Disconnect(object state)
        {
            lock (_disconnectSync)
            {
                foreach (ConnectionInfo conn in _socketTransport.Connections)
                {
                    if (!conn.Socket.Connected)
                    {
                        Player player = null;
                        if (conn.Player != null)
                            player = _playerManager.GetPlayerInfoById(conn.Player.Id);
                        if (player != null) _playerManager.LogOutPlayer(player);
                        _socketTransport.RemoveConnection(conn);
                        break;
                    }
                }
            }
        }

        public void Disconnect(Player info)
        {
            //info.Connection.Socket.BeginDisconnect(false,
            //    delegate(IAsyncResult state)
            //    {
            //        Socket sock = (Socket)state.AsyncState;
            //        sock.EndDisconnect(state);
            //    }, info.Connection.Socket);
            //if (!info.Connection.Socket.Connected)
            _socketTransport.RemoveConnection(info.Connection);
        }

        public void Disconnect(ConnectionInfo info)
        {
            _socketTransport.RemoveConnection(info);
        }

        public SocketTransport SocketTransport
        {
            get { return _socketTransport; }
            set { _socketTransport = value; }
        }
    }
}
