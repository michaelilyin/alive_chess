using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class StartNegotiateResponse : ICommand
    {
        [ProtoMember(1)]
        private Negotiate discussion;

        public StartNegotiateResponse()
        {
        }

        public StartNegotiateResponse(Negotiate negotiate)
        {
            this.discussion = negotiate;
        }

        public Command Id
        {
            get { return Command.StartNegotiateResponse; }
        }

        public Negotiate Negotiate
        {
            get { return discussion; }
            set { discussion = value; }
        }
    }
}
