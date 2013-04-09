using System;
using ProtoBuf;

namespace AliveChessLibrary.Commands.RegisterCommand
{
    [Serializable, ProtoContract]
    public class AuthorizeResponse : ICommand
    {
        [ProtoMember(1)]
        private bool _isNewPlayer;
        [ProtoMember(2)]
        private bool _isAuthorized;
        [ProtoMember(3)]
        private string _errorMessage;

        public AuthorizeResponse()
        {
        }

        public AuthorizeResponse(bool isNewPlayer, bool isAuthorized,
            string errorMessage)
        {
            this._isNewPlayer = isNewPlayer;
            this._isAuthorized = isAuthorized;
            this._errorMessage = errorMessage;
        }

        public Command Id
        {
            get { return Command.AuthorizeResponse; }
        }

        public bool IsNewPlayer
        {
            get { return _isNewPlayer; }
            set { _isNewPlayer = value; }
        }

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set { _isAuthorized = value; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
    }
}
