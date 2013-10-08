using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// получение краткой информации о всех союзах
    /// </summary>
    [ProtoContract]
    public class GetAliancesInfoRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetAliancesInfoRequest; }
        }
    }
}
