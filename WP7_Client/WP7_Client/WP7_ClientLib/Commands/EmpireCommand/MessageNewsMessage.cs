using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class MessageNewsMessage : ICommand
    {
        [ProtoMember(1)]
        private NewsType _news;
        [ProtoMember(2)]
        private string _message;
        [ProtoMember(3)]
        private int _senderId;

        public MessageNewsMessage()
        {
        }

        public MessageNewsMessage(NewsType news, int senderId, string message)
        {
            this._news = news;
            this._senderId = senderId;
            this._message = message;
        }

        public Command Id
        {
            get { return Command.MessageNewsMessage; }
        }

        public NewsType News
        {
            get { return _news; }
            set { _news = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public int SenderId
        {
            get { return _senderId; }
            set { _senderId = value; }
        }
    }
}
