using System.Net;
using System.Net.Sockets;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.ErrorCommand;
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

        private PluginPool _pluginPool;

        public ProtoBufferTransport(CommandPool commandPool, AliveChessLogger logger, 
            PlayerManager playerManager, PluginPool pluginPool)
        {
            this._logger = logger;
            this._commandPool = commandPool;
            this._playerManager = playerManager;
            this._socketTransport = new SocketTransport(commandPool, logger);
            this._pluginPool = pluginPool;
        }

        public void Send<T>(Socket socket, T cmd) where T : ICommand
        {
            byte[] binary = ProtoBufferCodec.Encode(cmd);
            _socketTransport.Send(binary, socket);
            _logger.Add(string.Format("Command {0} sent. Size: {1} bytes", cmd.Id, binary.Length));
        }

        public void SendNonSerializedBytes<T>(Socket socket, T command) where T : INonSerializable
        {
            _logger.Add(string.Format("Command {0} sent", command.Id));
            _socketTransport.Send(ProtoBufferCodec.EncodeNonSerialized(command), socket);
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
                        BytePackage data = info.Data.Read();
                        if(data!=null)
                        {
                            if (info.Data.IsProtoduf)
                                ExecuteAsLogic(data, info);
                            else if (info.Data.IsChat)
                                ExecuteAsChat(data, info);
                        }
                    }
                }
            }
        }

        private void ExecuteAsLogic(BytePackage data, ConnectionInfo info)
        {
            ICommand command = ProtoBufferCodec.Decode(data);
            if (command != null)
            {
                Message msg = new Message();
                msg.Command = command;
                msg.Sender = info.Player as Player;
                msg.RemoteEndPoint = info.Socket.RemoteEndPoint as IPEndPoint;

                _commandPool.Enqueue(msg);

                _logger.Add(string.Format("Command {0} received. Size: {1} bytes", command.Id, 
                    data.CommandBody.Length));
            }
            else
            {
                if (ProtoBufferCodec.ErrorLog.Count > 0)
                {
                    ErrorMessage message = new ErrorMessage();
                    message.Message = ProtoBufferCodec.ErrorLog.Dequeue().Message;
                    _socketTransport.Send(ProtoBufferCodec.Encode(message), info.Socket);
                }
                _logger.Add("Error has been occured during decoding");
            }
        }

        private void ExecuteAsChat(BytePackage data, ConnectionInfo info)
        {
            if (_pluginPool.IsChatLoaded)
            {
                ICommand command = ProtoBufferCodec.Decode(data);
                if (command != null)
                {
                    _pluginPool.Chat.Receive(command, info);
                }
                else
                {
                    if (ProtoBufferCodec.ErrorLog.Count > 0)
                    {
                        ErrorMessage message = new ErrorMessage();
                        message.Message = ProtoBufferCodec.ErrorLog.Dequeue().Message;
                        _socketTransport.Send(ProtoBufferCodec.Encode(message), info.Socket);
                    }
                    _logger.Add("Error has been occured during decoding");
                }
            }
            else _logger.Add("Chat plugin hasn't been loaded");
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
                    if (conn.Socket.Poll(0, SelectMode.SelectRead) && conn.Socket.Available == 0)
                    {
                        Player player = null;
                        if (conn.Player != null)
                            player = _playerManager.GetPlayerInfoById(conn.Player.Id);
                        if (player != null) 
                            _playerManager.LogOutPlayer(player);
                        _socketTransport.RemoveConnection(conn);
                        break;
                    }
                }
            }
        }

        public void Disconnect(Player info)
        {
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
