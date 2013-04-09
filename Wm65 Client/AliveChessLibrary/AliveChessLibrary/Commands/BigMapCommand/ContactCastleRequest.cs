using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class ContactCastleRequest : ICommand
    {
        [ProtoMember(1)]
        private int castleId;

        public Command Id
        {
            get { return Command.ContactCastleRequest; }
        }

        public int CastleId
        {
            get { return castleId; }
            set { castleId = value; }
        }
    }
}
