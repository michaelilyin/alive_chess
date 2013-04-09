using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class LooseCastleMessage : ICommand
    {
        [ProtoMember(1)]
        private int castleId;

        public LooseCastleMessage()
        {
        }

        public LooseCastleMessage(int castleId)
        {
            this.castleId = castleId;
        }

        public Command Id
        {
            get { return Command.LooseCastleMessage; }
        }

        public int CastleId
        {
            get { return castleId; }
            set { castleId = value; }
        }
    }
}
