using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class CaptureCastleResponse : ICommand
    {
        [ProtoMember(1)]
        private Castle castle;

        public CaptureCastleResponse()
        {
        }

        public CaptureCastleResponse(Castle castle)
        {
            this.castle = castle;
        }

        public Command Id
        {
            get { return Command.CaptureCastleResponse; }
        }

        public Castle Castle
        {
            get { return castle; }
            set { castle = value; }
        }
    }
}
