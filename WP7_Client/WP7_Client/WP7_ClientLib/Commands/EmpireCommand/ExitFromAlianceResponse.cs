using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// выход из союза
    /// </summary>
    [ProtoContract]
    public class ExitFromAlianceResponse : ICommand
    {
        [ProtoMember(1)]
        private bool _successed;

        public ExitFromAlianceResponse()
        {
        }

        public ExitFromAlianceResponse(bool successed)
        {
            this._successed = successed;
        }

        public Command Id
        {
            get { return Command.ExitFromAlianceResponse; }
        }

        public bool Successed
        {
            get { return _successed; }
            set { _successed = value; }
        }
    }
}
