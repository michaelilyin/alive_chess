using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class CollectUnitsRequest : ICommand
    {
        public Command Id
        {
            get { return  Command.CollectUnitsRequest;}
        }
    }
}
