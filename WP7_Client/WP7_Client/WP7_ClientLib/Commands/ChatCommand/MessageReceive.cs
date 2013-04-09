using ProtoBuf;

namespace AliveChessLibrary.Commands.ChatCommand
{
    [ProtoContract]
    public class MessageReceiveCommand : ICommand
    {
        [ProtoMember(1)]
        private string messagetext;
        [ProtoMember(2)]
        private byte messagetype;
        [ProtoMember(3)]
        private byte channeltype;
        [ProtoMember(4)]
        private int id_sender;
        [ProtoMember(5)]
        private string login_sender;

        public Command Id
        {
            get { return Command.MessageReceiveCommand; }
        }

        public string Message
        {
            get { return messagetext; }
            set { messagetext = value; }
        }
        /// <summary>
        /// 0 - общее
        /// 1 - приватное
        /// </summary>
        public byte MessageType
        {
            get { return messagetype; }
            set { messagetype = value; }
        }
        /// <summary>
        /// 0- общий канал
        /// 1- канал с ограничениями (администраторский, канал союза)
        /// </summary>
        public byte ChannelType
        {
            get { return channeltype; }
            set { channeltype = value; }
        }
        public int Id_Sender
        {
            get { return id_sender; }
            set { id_sender = value; }
        }
        public string Login_Sender
        {
            get { return login_sender; }
            set { login_sender = value; }
        }
    }
}
