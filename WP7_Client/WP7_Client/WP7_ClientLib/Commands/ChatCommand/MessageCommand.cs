using ProtoBuf;

namespace AliveChessLibrary.Commands.ChatCommand
{
    [ProtoContract]
    public class MessageCommand : ICommand
    {
        [ProtoMember(1)]
        private string message;
        [ProtoMember(2)]
        private byte msgtype;
        [ProtoMember(3)]
        private int id_getter;
        [ProtoMember(4)]
        private int id_sender;

        public Command Id
        {
            get { return Command.MessageCommand; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public byte MsgType
        {
            get { return msgtype; }
            set { msgtype = value; }
        }
        /// <summary>
        /// -2 если сообщение в канал своего союза
        /// -1 если сообщение в общий канал
        /// любое другое - приватное сообщение пользователю с этим id
        /// </summary>
        public int Id_Getter
        {
            get { return id_getter; }
            set {id_getter=value;}
        }
        public int Id_Sender
        {
            get { return id_sender; }
            set { id_sender = value; }
        }
    }
}
