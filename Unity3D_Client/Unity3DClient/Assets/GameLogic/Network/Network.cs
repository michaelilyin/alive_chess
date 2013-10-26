using AliveChessLibrary.Commands;
using AliveChessLibrary.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using UnityEngine;

namespace Assets.GameLogic.Network
{
    public static class Network
    {
        private static int PORT = 22000;

        private static ConnectionInfo _connection;
        private static IPEndPoint _remoteEndPoint;
        private static Socket _socket;

        public static CommandPool Commands { get; private set; }
        public static event EventHandler OnConnected;

        static Network()
        {
            Commands = new CommandPool();
        }

        #region Connect

        public static void Connect(IPAddress address)
        {
            _remoteEndPoint = new IPEndPoint(address, PORT);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _socket.BeginConnect(_remoteEndPoint, new AsyncCallback(ConnectCallback), _socket);
            }
            catch (Exception ex)
            {
                Debug.Log("BeginConnect error:" + ex.Message);
            }
        }

        private static void ConnectCallback(IAsyncResult res)
        {
            try
            {
                Socket socket = (Socket)res.AsyncState;
                socket.EndConnect(res);
                if (OnConnected != null)
                {
                    OnConnected(_socket, new EventArgs());
                }
                //ClientNetwork.Sending();
                Receive();
            }
            catch (Exception error)
            {
                Debug.Log("ConnectCallback error: " + error.Message);
            }
        }

        #endregion

        #region Recieve
        public static void Receive()
        {
            try
            {
                ConnectionInfo ci = new ConnectionInfo(_socket);
                ci.Data = new NetworkDataStream();
                _socket.BeginReceive(ci.Buffer, 0, ci.Buffer.Length, SocketFlags.None,
                                               new AsyncCallback(ReceiveCallback), ci);
            }
            catch (Exception ex)
            {
                Debug.Log("BeginReceive error: " + ex.Message);
            }
        }

        public static void ReceiveCallback(IAsyncResult res)
        {
            try
            {
                ConnectionInfo ci = (ConnectionInfo)res.AsyncState;
                int byteCount = ci.Socket.EndReceive(res);
                Debug.Log("End recieve completed!");
                byte[] data = new byte[byteCount];
                Array.Copy(ci.Buffer, data, byteCount);
                ci.Data.Write(data);

                if (ci.Data.IsReady)
                {
                    BytePackage package = ci.Data.Read();
                    if (package != null)
                    {
                        Decode(package);
                    }
                }
                Receive();

                //BytePackage bp;
                //if ((bp = ci.Data.Read()) != null)
                //{
                //    if (bp.CommandName != "GetUnityMapResponse")
                //        ClientNetwork.receiveCommandQueue.Enqueue(ByteManager.Decode(bp));
                //    else
                //        ClientNetwork.Map = bp.CommandBody;
                //    ClientNetwork.Receive();
                //}
                //else
                //    ci.Socket.BeginReceive(ci.Buffer, 0, ci.Buffer.Length, 0, new AsyncCallback(ClientNetwork.ReceiveCallback), ci);
            }
            catch (Exception error)
            {
                Debug.Log("ReceiveCallback error: " + error.Message + "\n" + error.StackTrace);
            }
        }
        #endregion

        #region Send

        public static void Send<T>(T command) where T : ICommand
        {
            try
            {
                Send(ProtoBufferCodec.Encode(command));
            }
            catch (Exception ex)
            {
                Debug.Log("Send error: " + ex.Message);
            }
        }

        public static void Send(byte[] data)
        {
            try
            {
                _socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), _socket);
            }
            catch (Exception ex)
            {
                Debug.Log("Send error: " + ex.Message);
            }
        }

        private static void SendCallback(IAsyncResult res)
        {
            Socket socket = (Socket)res.AsyncState;
            int byteSent = socket.EndSend(res);
            if (byteSent == 0)
            {
                Debug.Log("No bytes sent");
            }
        }

        #endregion

        private static void Decode(BytePackage package)
        {
            ICommand command = ProtoBufferCodec.Decode(package);
            if (command != null)
            {
                Commands.Enqueue(command);
            }
            else
            {
                Debug.Log("Error during deserialization has been occurred");
            }
        }

        public static void Disconnect()
        {
            _socket.Disconnect(true);
        }
    }
}
