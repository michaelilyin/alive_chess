using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class MoveKingRequest : ICommand
    {
        public Command Id
        {
            get { return Command.MoveKingRequest; }
        }

        [ProtoMember(1)]
        public int X { get; set; }

        [ProtoMember(2)]
        public int Y { get; set; }
    }
}
