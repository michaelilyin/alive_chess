using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetListBuildingsInCastleRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetListBuildingsInCastleRequest; }
        }
    }
}
