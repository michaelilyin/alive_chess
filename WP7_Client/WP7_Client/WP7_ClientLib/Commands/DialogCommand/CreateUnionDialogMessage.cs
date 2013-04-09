using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.DialogCommand
{
    [ProtoContract]
    public class CreateUnionDialogMessage : IMessage
    {
        [ProtoMember(1)]
        private DialogState type;
        [ProtoMember(2)]
        private int _disputeId;

        public CreateUnionDialogMessage()
        {
        }

        public CreateUnionDialogMessage(int disputeId, DialogState state)
        {
            this.type = state;
            this._disputeId = disputeId;
        }

        public Command Id
        {
            get { return Command.CreateUnionDialogMessage; }
        }

        public int DisputeId
        {
            get { return _disputeId; }
            set { _disputeId = value; }
        }

        public DialogState State
        {
            get { return type; }
            set { type = value; }
        }
    }
}
