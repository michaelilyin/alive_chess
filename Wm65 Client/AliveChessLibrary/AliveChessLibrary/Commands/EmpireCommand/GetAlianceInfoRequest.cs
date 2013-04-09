using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// получение информации о союзе
    /// </summary>
    [ProtoContract]
    public class GetAlianceInfoRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetAlianceInfoRequest; }
        }
    }
}
