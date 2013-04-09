using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// выдать лидерские привилегии
    /// </summary>
    [ProtoContract]
    public class GrandLeaderPrivilegesMessage : ICommand
    {
        public Command Id
        {
            get { return Command.GrandLeaderPrivilegesMessage; }
        }
    }
}
