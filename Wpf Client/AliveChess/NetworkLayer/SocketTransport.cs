using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Net;

namespace AliveChess.NetworkLayer
{
    public class SocketTransport
    {
        private readonly ILogger _logger;
        private readonly CommandPool _commands;

        public SocketTransport(ILogger logger, CommandPool commands)
        {
            _logger = logger;
            _commands = commands;
        }

        public void Receive(ConnectionInfo connection)
        {
            try
            {
                connection.Socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                                               ReceiveCallback, connection);
            }
            catch (SocketException ex)
            {
                _logger.Log("ErrLog", ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                _logger.Log("ErrLog", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log("ErrLog", ex.Message);
            }
            catch (SecurityException ex)
            {
                _logger.Log("ErrLog", ex.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            ConnectionInfo connection = (ConnectionInfo)result.AsyncState;
            Socket handler = connection.Socket;
            int byteData = 0;

            byteData = handler.EndReceive(result);

            if (byteData > 0)
            {
                byte[] data = new byte[byteData];
                Array.Copy(connection.Buffer, data, byteData);

                connection.Data.Write(data);

                if (connection.Data.IsReady)
                {
                    BytePackage package = connection.Data.Read();
                    if (package != null)
                    {
                        Decode(package);
                    }
                }

                Receive(connection);
            }
        }

        private void Decode(BytePackage package)
        {
            ICommand command = ProtoBufferCodec.Decode(package);
            if (command != null)
            {
                _commands.Enqueue(command);
            }
            else
            {
                _logger.Log("ErrLog", "Error during deserialization has been occurred");
            }
        }

        public void Send(byte[] data, Socket socket)
        {
             try
            {
                socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), socket);
            }
            catch (SocketException ex)
            {
                _logger.Log("ErrLog", ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                _logger.Log("ErrLog", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log("ErrLog", ex.Message);
            }
            catch (SecurityException ex)
            {
                _logger.Log("ErrLog", ex.Message);
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            Socket handler = (Socket) result.AsyncState;
            int byteSent = handler.EndSend(result);
            if (byteSent == 0)
            {
                _logger.Log("ErrLog", "No bytes sent");
            }
        }
    }
}
