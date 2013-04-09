using ProtoBuf;

namespace AliveChessLibrary.Commands.ChatCommand
{
    [ProtoContract]
    public class JoinLeaveCommand : ICommand
    {
        [ProtoMember(1)]
        private int id_client;
        [ProtoMember(2)]
        private byte conordisc;

        public Command Id
        {
            get { return Command.JoinLeaveCommand; }
        }

        public int ID_CLIENT
        {
            get { return id_client; }
            set { id_client = value; }
        }
        public byte ConOrDisc
        {
            get { return conordisc; }
            set { conordisc = value; }
        }
    }
}
