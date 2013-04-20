using System;
using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.DialogCommand
{
    [Serializable, ProtoContract]
    public class BattleDialogMessage : IMessage
    {
        [ProtoMember(1)]
        private DialogState type;
        [ProtoMember(2)]
        private int disputeId;

        public BattleDialogMessage()
        {
        }

        public BattleDialogMessage(int disputeId, DialogState state)
        {
            this.type = state;
            this.disputeId = disputeId;
        }

        public Command Id
        {
            get { return Command.BattleDialogMessage; }
        }

        public int DisputeId
        {
            get { return disputeId; }
            set { disputeId = value; }
        }

        public DialogState State
        {
            get { return type; }
            set { type = value; }
        }
    }
}
