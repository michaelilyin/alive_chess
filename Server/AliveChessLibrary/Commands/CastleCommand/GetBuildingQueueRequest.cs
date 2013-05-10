using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetBuildingQueueRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetBuildingQueueRequest; }
        }
    }
}
