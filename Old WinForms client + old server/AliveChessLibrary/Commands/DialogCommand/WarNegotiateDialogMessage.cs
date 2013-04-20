using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessLibrary.Commands.DialogCommand
{
    public class WarNegotiateDialogMessage : IMessage
    {
        [ProtoMember(1)]
        private DialogState state;
        [ProtoMember(2)]
        private uint disputeId;

        public Command Id
        {
            get { return Command.WarNegotiateDialogMessage; }
        }

        public uint DisputeId
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
