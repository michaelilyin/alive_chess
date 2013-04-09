using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class TakeAwayLeaderPrivilegesMessage : ICommand
    {
        public Command Id
        {
            get { return Command.TakeAwayLeaderPrivilegesMessage; }
        }
    }
}
