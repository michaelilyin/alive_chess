using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.DialogCommand
{
    [ProtoContract]
    public class JoinEmperiesDialogMessage : IMessage
    {
        [ProtoMember(1)]
        private DialogState type;
        [ProtoMember(2)]
        private int disputeId;

        public JoinEmperiesDialogMessage()
        {
        }

        public JoinEmperiesDialogMessage(int disputeId, DialogState state)
        {
            this.type = state;
            this.disputeId = disputeId;
        }

        public DialogState State
        {
            get { return type; }
            set { type = value; }
        }

        public Command Id
        {
            get { return Command.JoinEmperiesDialogMessage; }
        }

        public int DisputeId
        {
            get { return disputeId; }
            set { disputeId = value; }
        }
    }
}
