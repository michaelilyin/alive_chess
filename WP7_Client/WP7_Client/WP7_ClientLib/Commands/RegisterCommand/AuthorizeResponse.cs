using System;
using ProtoBuf;

namespace AliveChessLibrary.Commands.RegisterCommand
{
    [ProtoContract]
    public class AuthorizeResponse : ICommand
    {
        public AuthorizeResponse()
        {
        }

        public AuthorizeResponse(bool isNewPlayer, bool isAuthorized,
            string errorMessage)
        {
            this.IsNewPlayer = isNewPlayer;
            this.IsAuthorized = isAuthorized;
            this.ErrorMessage = errorMessage;
        }

        public Command Id
        {
            get { return Command.AuthorizeResponse; }
        }

        [ProtoMember(1)]
        public bool IsNewPlayer { get; set; }

        [ProtoMember(2)]
        public bool IsAuthorized { get; set; }

        [ProtoMember(3)]
        public string ErrorMessage { get; set; }
    }
}
