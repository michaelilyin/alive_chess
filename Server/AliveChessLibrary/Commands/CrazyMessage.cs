using ProtoBuf;

namespace AliveChessLibrary.Commands
{
    [ProtoContract]
    public class CrazyMessage : ICommand
    {
        [ProtoMember(1)]
        private string message;

        public Command Id
        {
            get { return Command.ErrorMessage; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
