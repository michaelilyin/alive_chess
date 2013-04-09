using ProtoBuf;

namespace AliveChessLibrary.Commands.RegisterCommand
{
    [ProtoContract]
    public class AuthorizeRequest : ICommand
    {
        public Command Id
        {
            get { return Command.AuthorizeRequest; }
        }

        [ProtoMember(1)]
        public string Login { get; set; }

        [ProtoMember(2)]
        public string Password { get; set; }
    }
}
