using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class CaptureCastleRequest : ICommand
    {
        [ProtoMember(1)]
        private int castleId;

        public Command Id
        {
            get { return Command.CaptureCastleRequest; }
        }

        public int CastleId
        {
            get { return castleId; }
            set { castleId = value; }
        }
    }
}
