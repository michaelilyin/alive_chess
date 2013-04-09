using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class ComeInCastleResponse : ICommand
    {
        [ProtoMember(1)]
        private int castleId;

        public ComeInCastleResponse()
        {
        }

        public ComeInCastleResponse(int castleId)
        {
            this.castleId = castleId;
        }

        public Command Id
        {
            get { return Command.ComeInCastleResponse; }
        }

        public int CastleId
        {
            get { return castleId; }
            set { castleId = value; }
        }
    }
}
