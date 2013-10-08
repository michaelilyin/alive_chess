using AliveChessLibrary.Commands;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GameLogicLibrary.Network
{
    public class NetworkManager
    {
        private ConnectionInfo _connection;
        private IPEndPoint _remoteEndPoint;
        private readonly SocketTransport _transport;
        private readonly CommandPool _commands;
        private readonly ILogger _logger;

        private const int PORT = 22000;

        public NetworkManager(ILogger logger, CommandPool commands)
        {
            this._logger = logger;
            this._commands = commands;
            this._transport = new SocketTransport(logger, commands);
        }

        public void Connect(IPAddress address)
        {
            _remoteEndPoint = new IPEndPoint(address, PORT);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _connection = new ConnectionInfo(socket);
            socket.BeginConnect(_remoteEndPoint, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            Socket handler = (Socket)result.AsyncState;
            handler.EndConnect(result);
            if (OnConnect != null)
            {
                StartReceive();
                OnConnect();
            }
        }

        public void StartReceive()
        {
            _transport.Receive(_connection);
        }

        public void Send<T>(T command) where T : ICommand
        {
            _transport.Send(ProtoBufferCodec.Encode(command), _connection.Socket);
        }

        public bool IsConnected
        {
            get { return _connection.Socket.Connected; }
        }

        public delegate void ConnectHandler();

        public event ConnectHandler OnConnect;
    }
}
