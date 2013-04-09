using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.DialogCommand
{
    [ProtoContract]
    public class CaptureCastleDialogMessage : IMessage
    {
        [ProtoMember(1)]
        private DialogState type;
        [ProtoMember(2)]
        private int _disputeId;

        public CaptureCastleDialogMessage()
        {
        }

        public CaptureCastleDialogMessage(int disputeId, DialogState state)
        {
            this.type = state;
            this._disputeId = disputeId;
        }

        public Command Id
        {
            get { return Command.CaptureCastleDialogMessage; }
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
