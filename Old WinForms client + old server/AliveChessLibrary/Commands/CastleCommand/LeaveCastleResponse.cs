using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class LeaveCastleResponse : ICommand
    {
        public Command Id
        {
            get { return Command.LeaveCastleResponse; }
        }
    }
}
