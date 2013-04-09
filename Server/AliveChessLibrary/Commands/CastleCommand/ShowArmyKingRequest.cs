using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class ShowArmyKingRequest : ICommand
    {
        public Command Id
        {
            get { return Command.ShowArmyKingRequest; }
        }
    }
}
