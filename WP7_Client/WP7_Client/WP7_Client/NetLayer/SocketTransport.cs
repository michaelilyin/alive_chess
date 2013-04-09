using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Net;

namespace WP7_Client.NetLayer
{
    public class SocketTransport
    {
        private const Int32 MaxBufferSize = 1024;
        private static ManualResetEvent _operationDone;
        private static ManualResetEvent _receiveDone;
        private readonly CommandPool _commandPool;
        private readonly BackgroundWorker _decodeWorker;
        private readonly ILogger _logger;
        private readonly BackgroundWorker _receiveWorker;
        public SocketError Error;

        public SocketTransport(ILogger logger, CommandPool commands)
        {
            _commandPool = commands;
            _logger = logger;
            new NetworkDataStream();
            _receiveWorker = new BackgroundWorker();
            _decodeWorker = new BackgroundWorker();
            _receiveWorker.DoWork += Receive;
            _operationDone = new ManualResetEvent(false);
            _receiveDone = new ManualResetEvent(false);
        }

        public ConnectionInfo Connection { get; set; }

        public bool Connect(string hostName, int portNumber)
        {
            var result = true;
            var hostEntry = new DnsEndPoint(hostName, portNumber);
            Connection = new ConnectionInfo(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
            var socketEventArg = new SocketAsyncEventArgs {RemoteEndPoint = hostEntry};
            socketEventArg.Completed += delegate(object s, SocketAsyncEventArgs e)
                                            {
                                                if (e.SocketError != SocketError.Success && !((Socket)s).Connected)
                                                {
                                                    result = false;
                                                    _operationDone.Set();
                                                    Error = e.SocketError;
                                                    return;
                                                }
                                                if (!_receiveWorker.IsBusy) _receiveWorker.RunWorkerAsync();
                                                if (!_decodeWorker.IsBusy) _decodeWorker.RunWorkerAsync();
                                                _operationDone.Set();
                                            };
            _operationDone.Reset();
            Connection.Socket.ConnectAsync(socketEventArg);
            _operationDone.WaitOne(30000);
            return result;
        }

        private void Decode(BytePackage package)
        {
            ICommand command = ProtoBufferCodec.Decode(package);
            if (command != null)
            {
                _commandPool.Enqueue(command);
            }
            else
            {
                _logger.Log("ErrLog", "Error during deserialization has been occurred");
            }
        }

        public void Receive(object sender, DoWorkEventArgs e)
        {
            while (Connection.Socket.Connected)
            {
                var socketEventArg = new SocketAsyncEventArgs {RemoteEndPoint = Connection.Socket.RemoteEndPoint};
                socketEventArg.SetBuffer(new Byte[MaxBufferSize], 0, MaxBufferSize);
                socketEventArg.Completed += delegate(object s, SocketAsyncEventArgs ex)
                                                {
                                                    if (ex.SocketError == SocketError.Success)
                                                    {
                                                        int byteData = ex.BytesTransferred;

                                                        if (byteData > 0)
                                                        {
                                                             var data = new byte[byteData];
                                                            Array.Copy(ex.Buffer, data, byteData);

                                                            Connection.Data.Write(data);

                                                            if (Connection.Data.IsReady)
                                                            {
                                                                BytePackage package = Connection.Data.Read();
                                                                if (package != null)
                                                                {
                                                                    Decode(package);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Error = ex.SocketError;
                                                        _logger.Log("An error had been occured while receiving data");
                                                    }
                                                    _receiveDone.Set();
                                                };
                _receiveDone.Reset();
                Connection.Socket.ReceiveAsync(socketEventArg);
                _receiveDone.WaitOne(100);
            }
        }

        public void Send<T>(T command) where T : ICommand
        {
            byte[] data = ProtoBufferCodec.Encode(command);
            if (data != null)
            {
                var socketEventArg = new SocketAsyncEventArgs {RemoteEndPoint = Connection.Socket.RemoteEndPoint};
                socketEventArg.SetBuffer(data, 0, data.Length*sizeof (byte));
                socketEventArg.Completed += delegate(object s, SocketAsyncEventArgs e)
                                                {
                                                    if (e.SocketError != SocketError.Success)
                                                    {
                                                        _logger.Log("An error had been occured while seding data");
                                                        Error = e.SocketError;
                                                    }
                                                };
                Connection.Socket.SendAsync(socketEventArg);
            }
        }

        public bool Connected()
        {
            return Connection != null && Connection.Socket != null && Connection.Socket.Connected;
        }

        public void Stop()
        {
            Connection.Socket.Close();
        }
    }
}