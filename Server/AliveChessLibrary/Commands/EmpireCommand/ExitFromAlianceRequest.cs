using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// выход из союза
    /// </summary>
    [ProtoContract]
    public class ExitFromAlianceRequest : ICommand
    {
        public Command Id
        {
            get { return Command.ExitFromAlianceRequest; }
        }
    }
}
