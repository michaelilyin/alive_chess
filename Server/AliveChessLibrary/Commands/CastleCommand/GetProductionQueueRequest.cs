using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetProductionQueueRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetProductionQueueRequest; }
        }
    }
}
