using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class ComeInCastleRequest : ICommand
    {
        [ProtoMember(1)]
        private int castleId;

        public Command Id
        {
            get { return Command.ComeInCastleRequest; }
        }

        public int CastleId
        {
            get { return castleId; }
            set { castleId = value; }
        }
    }
}
