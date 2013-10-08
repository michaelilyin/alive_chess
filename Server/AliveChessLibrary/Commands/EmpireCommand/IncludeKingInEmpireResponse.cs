using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// принять игрока в империю
    /// </summary>
    [ProtoContract]
    public class IncludeKingInEmpireResponse : ICommand
    {
        [ProtoMember(1)]
        private bool _successed;

        public IncludeKingInEmpireResponse()
        {
        }

        public IncludeKingInEmpireResponse(bool successed)
        {
            this._successed = successed;
        }

        public Command Id
        {
            get { return Command.IncludeKingInEmpireResponse; }
        }

        public bool Successed
        {
            get { return _successed; }
            set { _successed = value; }
        }
    }
}
