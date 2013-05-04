using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetKingArmyRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetKingArmyRequest; }
        }
    }
}
