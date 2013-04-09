using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class VoteBallotMessage : ICommand
    {
        [ProtoMember(1)]
        private bool _support;

        public Command Id
        {
            get { return Command.VoteBallotMessage; }
        }

        public bool Support
        {
            get { return _support; }
            set { _support = value; }
        }
    }
}
