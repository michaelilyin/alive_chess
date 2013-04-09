using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.DialogCommand
{
    [ProtoContract]
    public class WarDialogMessage : IMessage
    {
        [ProtoMember(1)]
        private DialogState state;
        [ProtoMember(2)]
        private int disputeId;

        public WarDialogMessage()
        {
        }

        public WarDialogMessage(int disputeId, DialogState state)
        {
            this.state = state;
            this.disputeId = disputeId;
        }

        public Command Id
        {
            get { return Command.WarDialogMessage; }
        }

        public int DisputeId
        {
            get { return disputeId; }
            set { disputeId = value; }
        }

        public DialogState State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
