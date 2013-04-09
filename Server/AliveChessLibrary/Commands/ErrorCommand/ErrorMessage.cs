using ProtoBuf;

namespace AliveChessLibrary.Commands.ErrorCommand
{
    [ProtoContract]
    public class ErrorMessage : ICommand
    {
        [ProtoMember(1)]
        private string _message;

        public ErrorMessage()
        {}

        public ErrorMessage(string message)
        {
            this._message = message;
        }

        public Command Id
        {
            get { return Command.ErrorMessage; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
