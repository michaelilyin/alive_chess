using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.DialogCommand
{
    [ProtoContract]
    public class PayOffDialogMessage : IMessage
    {
        [ProtoMember(1)]
        private DialogState type;
        [ProtoMember(2)]
        private int disputeId;
        [ProtoMember(3)]
        private ResourceTypes resourceType;
        [ProtoMember(4)]
        private int resourceCount;

        public PayOffDialogMessage()
        {
        }

        public PayOffDialogMessage(int disputeId, DialogState state)
        {
            this.type = state;
            this.disputeId = disputeId;
        }

        public Command Id
        {
            get { return Command.PayOffDialogMessage; }
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

        public ResourceTypes ResourceType
        {
            get { return resourceType; }
            set { resourceType = value; }
        }

        public int ResourceCount
        {
            get { return resourceCount; }
            set { resourceCount = value; }
        }
    }
}
