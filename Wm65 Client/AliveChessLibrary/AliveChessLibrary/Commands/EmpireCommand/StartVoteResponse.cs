using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class StartVoteResponse : ICommand
    {
        [ProtoMember(1)]
        private bool _successed;

        public Command Id
        {
            get { return Command.StartVoteResponse; }
        }

        public bool Successed
        {
            get { return _successed; }
            set { _successed = value; }
        }
    }
}
