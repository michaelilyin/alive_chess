using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// сообщение королю о факте его исключения из империи
    /// </summary>
    [ProtoContract]
    public class ExcludeFromEmpireMessage : ICommand
    {
        public Command Id
        {
            get { return Command.ExcludeFromEmpireMessage; }
        }
    }
}
