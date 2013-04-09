using ProtoBuf;

namespace AliveChessLibrary.Commands.ErrorCommand
{
    [ProtoContract]
    public class ErrorMessage : ICommand
    {
        public ErrorMessage()
        {}

        public ErrorMessage(string message)
        {
            this.Message = message;
        }

        public Command Id
        {
            get { return Command.ErrorMessage; }
        }

        [ProtoMember(1)]
        public string Message { get; set; }
    }
}
