using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class ShowArmyCastleRequest : ICommand
    {
        public Command Id
        {
            get { return Command.ShowArmyCastleRequest; }
        }
    }
}
