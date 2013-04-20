using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class MoveKingRequest : ICommand
    {
        [ProtoMember(1)]
        private int x;
        [ProtoMember(2)]
        private int y;

        public Command Id
        {
            get { return Command.MoveKingRequest; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}
