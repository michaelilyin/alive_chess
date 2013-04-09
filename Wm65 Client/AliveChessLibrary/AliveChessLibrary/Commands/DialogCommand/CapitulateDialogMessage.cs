using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.DialogCommand
{
    [ProtoContract]
    public class CapitulateDialogMessage : IMessage
    {
        [ProtoMember(1)]
        private DialogState type;
        [ProtoMember(2)]
        private int disputeId;

        public CapitulateDialogMessage()
        {
        }

        public CapitulateDialogMessage(int disputeId, DialogState state)
        {
            this.type = state;
            this.disputeId = disputeId;
        }

        public Command Id
        {
            get { return Command.CapitulateDialogMessage; }
        }

        public DialogState State
        {
            get { return type; }
            set { type = value; }
        }
      
        public int DisputeId
        {
            get { return disputeId; }
            set { disputeId = value; }
        }
    }
}
