using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class ContactKingRequest : ICommand
    {
        [ProtoMember(1)]
        private int opponentId;

        public Command Id
        {
            get { return Command.ContactKingRequest; }
        }

        public int OpponentId
        {
            get { return opponentId; }
            set { opponentId = value; }
        }
    }
}
