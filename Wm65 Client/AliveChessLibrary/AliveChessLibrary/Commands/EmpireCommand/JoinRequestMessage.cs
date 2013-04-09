using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class JoinRequestMessage : ICommand
    {
        [ProtoMember(1)]
        private King _candidate;

        public JoinRequestMessage()
        {
        }

        public JoinRequestMessage(King candidate)
        {
            this._candidate = candidate;
        }

        public Command Id
        {
            get { return Command.JoinRequestMessage; }
        }

        public King Candidate
        {
            get { return _candidate; }
            set { _candidate = value; }
        }
    }
}
