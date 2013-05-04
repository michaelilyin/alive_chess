using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetBuildingsRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetBuildingsRequest; }
        }
    }
}
