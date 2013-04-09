using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class StartImpeachmentRequest : ICommand
    {
        [ProtoMember(1)]
        private string _message;

        public Command Id
        {
            get { return Command.StartImpeachmentRequest; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
