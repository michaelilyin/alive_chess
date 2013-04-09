using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class LeaveCastleRequest : ICommand
    {
        public Command Id
        {
            get { return Command.LeaveCastleRequest; }
        }
    }
}
