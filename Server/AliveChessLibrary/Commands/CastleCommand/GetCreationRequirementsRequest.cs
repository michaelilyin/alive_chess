using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetCreationRequirementsRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetCreationRequirementsRequest; }
        }
    }
}
