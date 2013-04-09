using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class StartNegotiateRequest : ICommand
    {
        [ProtoMember(1)]
        private int opponentId;

        public Command Id
        {
            get { return Command.StartNegotiateRequest; }
        }

        public int OpponentId
        {
            get { return opponentId; }
            set { opponentId = value; }
        }
    }
}
