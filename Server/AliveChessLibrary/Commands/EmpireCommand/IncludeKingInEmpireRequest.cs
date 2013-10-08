using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// принять игрока в империю
    /// </summary>
    [ProtoContract]
    public class IncludeKingInEmpireRequest : ICommand
    {
        [ProtoMember(1)]
        private int _kingId;

        public Command Id
        {
            get { return Command.IncludeKingInEmpireRequest; }
        }

        public int KingId
        {
            get { return _kingId; }
            set { _kingId = value; }
        }
    }
}
