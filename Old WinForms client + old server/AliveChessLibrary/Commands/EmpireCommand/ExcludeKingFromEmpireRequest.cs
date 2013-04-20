using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// выгнать короля из империи
    /// </summary>
    [ProtoContract]
    public class ExcludeKingFromEmpireRequest : ICommand
    {
        [ProtoMember(1)]
        private int _kingId;

        public Command Id
        {
            get { return Command.ExcludeKingFromEmpireRequest; }
        }

        public int KingId
        {
            get { return _kingId; }
            set { _kingId = value; }
        }
    }
}
