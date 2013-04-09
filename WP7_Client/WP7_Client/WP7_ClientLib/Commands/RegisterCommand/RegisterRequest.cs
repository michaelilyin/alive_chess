using ProtoBuf;

namespace AliveChessLibrary.Commands.RegisterCommand
{
    [ProtoContract]
    public class RegisterRequest : ICommand
    {
        public Command Id
        {
            get { return Command.RegisterRequest; }
        }

        [ProtoMember(1)]
        public string Login { get; set; }

        [ProtoMember(2)]
        public string Password { get; set; }
    }
}
