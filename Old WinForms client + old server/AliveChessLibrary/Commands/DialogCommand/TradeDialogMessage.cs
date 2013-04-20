using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessLibrary.Commands.DialogCommand
{
    [Serializable, ProtoContract]
    public class TradeDialogMessage : ICommand
    {
        [ProtoMember(1)]
        private DialogState type;
        [ProtoMember(2)]
        private long disputeId;

        public DialogState Type
        {
            get { return type; }
            set { type = value; }
        }

        public Command Id
        {
            get { return Command.TradeDialogMessage; }
        }

        public long DisputeId
        {
            get { return disputeId; }
            set { disputeId = value; }
        }

    }
}
