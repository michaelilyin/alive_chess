using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetGameStateRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetGameStateRequest; }
        }
    }
}
