using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetArmyCastleToKingRequest : ICommand
    {
        public Command Id
        {
            get { return  Command.GetArmyCastleToKingRequest;}
        }
    }
}
