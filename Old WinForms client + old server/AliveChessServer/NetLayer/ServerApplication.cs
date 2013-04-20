using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AliveChessServer.LogicLayer;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.NetLayer
{
    public class ServerApplication
    {
        private Socket _acceptSocket;
        private IPAddress _ipAddress;
        private IPEndPoint _ipEndPoint;

        private AliveChessLogger _logger;

        private ProtoBufferTransport _transport; 
        private CommandPool _commands;

        private bool _isStarted = false;

        private Timer _decodeTimer;
        private Timer _disconnectTimer;

        private const int port = 22000;

        public ServerApplication(MainForm form, PlayerManager playerManager, CommandPool commands,
            AliveChessLogger logger)
        {
            Debug.Assert(playerManager != null);
            Debug.Assert(commands != null);
            Debug.Assert(logger != null);

            this._logger = logger;
            this._commands = commands;

            //_ipAddress = IPAddress.Parse("5.64.64.149");
            _ipAddress = IPAddress.Parse("127.0.0.1");
            _ipEndPoint = new IPEndPoint(_ipAddress, port);
            _acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _transport = new ProtoBufferTransport(commands, logger, playerManager);
        }

        public void Start()
        {
            if (!_isStarted)
            {
                StartAccept();
                _decodeTimer = new Timer(new TimerCallback(_transport.Decode), null, 0, 10);
                _disconnectTimer = new Timer(new TimerCallback(_transport.Disconnect), null, 0, 50);
                _isStarted = true;
            }
        }

        private void StartAccept()
        {
            _acceptSocket.Bind(_ipEndPoint);
            _acceptSocket.Listen(10);

            _acceptSocket.BeginAccept(new AsyncCallback(AcceptCallback), _acceptSocket);
        }

        private void AcceptCallback(IAsyncResult result)
        {
            Socket listener = (Socket)result.AsyncState;
            Socket handler = listener.EndAccept(result);
            _transport.SocketTransport.AddConnection(handler);

            _acceptSocket.BeginAccept(new AsyncCallback(AcceptCallback), _acceptSocket);
        }

        public ProtoBufferTransport Transport
        {
            get { return _transport; }
        }
    }
}
