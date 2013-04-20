namespace AliveChessServer.LogicLayer.UsersManagement
{
    public struct Identity
    {
        private string _login;
        private string _password;

        public Identity(string login, string password)
        {
            this._login = login;
            this._password = password;
        }

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
