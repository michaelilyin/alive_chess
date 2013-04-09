using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class JoinToAlianceResponse : ICommand
    {
        [ProtoMember(1)]
        private bool _successed;

        public JoinToAlianceResponse()
        {
        }

        public JoinToAlianceResponse(bool successed)
        {
            this._successed = successed;
        }

        public Command Id
        {
            get { return Command.JoinToAlianceResponse; }
        }

        public bool Successed
        {
            get { return _successed; }
            set { _successed = value; }
        }
    }
}
