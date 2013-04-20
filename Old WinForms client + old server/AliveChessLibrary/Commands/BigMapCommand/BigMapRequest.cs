using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class BigMapRequest : ICommand
    {
        public Command Id
        {
            get { return Command.BigMapRequest; }
        }
    }
}
