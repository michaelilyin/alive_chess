using System;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Net;
using AliveChessServer.LogicLayer.AI;
using BehaviorAILibrary;

namespace AliveChessServer.NetLayer
{
    public class Messenger : IMessenger
    {
        private Animat _animat;
        private ConnectionInfo _connection;
        private ProtoBufferTransport _transport;

        public Messenger(Animat animat)
        {
            this._animat = animat;
        }

        public Messenger(ProtoBufferTransport transport, ConnectionInfo connection)
        {
            this._connection = connection;
            this._transport = transport;
        }

        public void SendAIMessage<T>(T message) where T : IStimulus
        {
            _animat.Tell((BotKing)message.Receiver, message);
        }

        public void SendNetworkMessage<T>(T messsge) where T : ICommand
        {
            if (_connection != null)
            {
                _transport.Send(_connection.Socket, messsge);
            }
            else throw new InvalidOperationException("Socket is null");
        }

        public void SendNonSerializedMessage<T>(T message) where T : INonSerializable
        {
            if (_connection != null)
            {
                _transport.SendNonSerializedBytes(_connection.Socket, message);
            }
            else throw new InvalidOperationException("Socket is null");
        }
    }
}
