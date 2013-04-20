using ProtoBuf;

namespace AliveChessLibrary.Commands.RegisterCommand
{
    [ProtoContract]
    public class AuthorizeRequest : ICommand
    {
        [ProtoMember(1)]
        private string login;
        [ProtoMember(2)]
        private string password;
        [ProtoMember(3)]
        private bool isSuperUser;

        public Command Id
        {
            get { return Command.AuthorizeRequest; }
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

        public bool IsSuperUser
        {
            get { return isSuperUser; }
            set { isSuperUser = value; }
        }
    }
}
