using ProtoBuf;

namespace AliveChessLibrary.Commands
{
    [ProtoContract]
    public class ErrorMessage : ICommand
    {
        private string _message;

        public Command Id
        {
            get { return Command.Error; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
