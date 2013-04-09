using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class ContactCastleRequest : ICommand
    {
        public Command Id
        {
            get { return Command.ContactCastleRequest; }
        }

        [ProtoMember(1)]
        public int CastleId { get; set; }
    }
}
