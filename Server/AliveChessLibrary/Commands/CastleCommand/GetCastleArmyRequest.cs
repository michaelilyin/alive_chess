using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetCastleArmyRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetCastleArmyRequest; }
        }
    }
}
