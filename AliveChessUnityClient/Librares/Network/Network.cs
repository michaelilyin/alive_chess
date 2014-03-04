using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.Net;
using Logger;
using Network.CommandControllers;
using Properties.ConcreateProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Network
{
    public class Network
    {
        public Network()
        {
            ReadNetworkProperties();
            _commands = new CommandPool();
            Messages = new MessagesPool();
            _requestHandler = new RequestHandler(_commands);
            BigMapCommandController = new BigMapCommandController(this);
        }

        #region public

        public bool IsConnected { get { return _socket == null ? false : _socket.Connected; } }
        public event EventHandler OnConnected;
        public BigMapCommandController BigMapCommandController { get; private set; }
        public MessagesPool Messages { get; private set; }

        public void Connect(String login, String pass)
        {
            Log.Message("Start connection");
            _login = login;
            _pass = pass;
            _remoteEndPoint = new IPEndPoint(properties.Server, properties.Port);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
#warning new
                if (!IsConnected)
                    _socket.BeginConnect(_remoteEndPoint, new AsyncCallback(ConnectCallback), _socket);
                else
                {
                    AuthorizeRequest request = new AuthorizeRequest();
                    request.Login = _login;
                    request.Password = _pass;
                    Send(request);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Start connect error!");
            }
        }

        private void ConnectCallback(IAsyncResult res)
        {
            try
            {
                Socket socket = (Socket)res.AsyncState;
                socket.EndConnect(res);
                _requestHandler.Start();
                //if (OnConnected != null)
                //{
                //    OnConnected(_socket, new EventArgs());
                //}
                AuthorizeRequest request = new AuthorizeRequest();
                request.Login = _login;
                request.Password = _pass;
                Send(request);

                StartReceive();
                Log.Message("Connected");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Connection error");
            }
        }


        public void Disconnect()
        {
            BigMapCommandController.StopGameStateUpdate();
            BigMapCommandController.StopObjectsUpdate();
            if (_socket.Connected)
                _socket.Disconnect(true);
            _requestHandler.Stop();
        }

        public void RegisterHandler(Command command, CommandHandler handler)
        {
            _requestHandler.RegisterHandler(command, handler);
        }

        #endregion

        #region private

        private NetworkProperties properties;
        private ConnectionInfo _connection;
        private CommandPool _commands;
        private RequestHandler _requestHandler; 
        private Socket _socket;
        private IPEndPoint _remoteEndPoint;
        private String _login;
        private String _pass;

        private void ReadNetworkProperties()
        {
            properties = Properties.Properties.NetworkProperties;
            Log.Message("Properties loaded");
        }

        #region Recieve

        private void StartReceive()
        {
            Log.Message("Start receive");
            _connection = new ConnectionInfo(_socket);
            _connection.Data = new NetworkDataStream();
            Receive(_connection);
        }

        private void Receive(ConnectionInfo _connection)
        {
            try
            {
                _socket.BeginReceive(_connection.Buffer, 0, _connection.Buffer.Length, SocketFlags.None,
                                               new AsyncCallback(ReceiveCallback), _connection);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Start receive error!");
            }
        }

        private void ReceiveCallback(IAsyncResult res)
        {
            try
            {
                ConnectionInfo _connection = (ConnectionInfo)res.AsyncState;
                int byteCount = _connection.Socket.EndReceive(res);
                byte[] data = new byte[byteCount];
                Array.Copy(_connection.Buffer, data, byteCount);
                _connection.Data.Write(data);
                if (_connection.Data.IsReady)
                {
                    BytePackage package = _connection.Data.Read();
                    if (package != null)
                    {
                        //Log.Message("Not null command");
                        Decode(package);
                    }
                    //else
                        //Log.Message("Null command");
                }
                Receive(_connection);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Receive error!");
            }
        }
        #endregion

        #region Send

        public void Send<T>(T command) where T : ICommand
        {
            try
            {
                Send(ProtoBufferCodec.Encode(command));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Send error");
            }
        }

        public void Send(byte[] data)
        {
            try
            {
                _socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), _socket);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Send error");
            }
        }

        private void SendCallback(IAsyncResult res)
        {
            Socket socket = (Socket)res.AsyncState;
            int byteSent = socket.EndSend(res);
            if (byteSent == 0)
            {
                Log.Error("No bytes sent");
            }
        }

        #endregion

        private void Decode(BytePackage package)
        {
            //Log.Message("Start decode");
            ICommand command = ProtoBufferCodec.Decode(package);
            if (command != null)
            {
                //Log.Message("Push decoded");
                _commands.Enqueue(command);
            }
            else
            {
                Log.Error("Error during deserialization has been occurred");
            }
        }

        #endregion
    }
}
