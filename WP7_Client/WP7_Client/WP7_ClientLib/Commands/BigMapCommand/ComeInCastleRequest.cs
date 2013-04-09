using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class ComeInCastleRequest : ICommand
    {
        public Command Id
        {
            get { return Command.ComeInCastleRequest; }
        }

        [ProtoMember(1)]
        public int CastleId { get; set; }
    }
}
