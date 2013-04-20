using ProtoBuf;

namespace AliveChessLibrary.Commands.RegisterCommand
{
    [ProtoContract]
    public class RegisterRequest : ICommand
    {
        [ProtoMember(1)]
        private string login;
        [ProtoMember(2)]
        private string password;

        public Command Id
        {
            get { return Command.RegisterRequest; }
        }

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
