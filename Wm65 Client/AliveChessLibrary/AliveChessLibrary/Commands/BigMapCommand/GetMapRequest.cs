using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetMapRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetMapRequest; }
        }
    }
}
