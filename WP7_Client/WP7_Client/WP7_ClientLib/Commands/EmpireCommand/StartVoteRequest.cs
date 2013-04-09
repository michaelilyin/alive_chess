using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class StartVoteRequest : ICommand
    {
        [ProtoMember(1)]
        private string _message;

        public Command Id
        {
            get { return Command.StartVoteRequest; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
