using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class ContactKingResponse : ICommand
    {
        [ProtoMember(1)]
        private Dialog discussion;

        public ContactKingResponse()
        {
        }

        public ContactKingResponse(Dialog dialog)
        {
            this.discussion = dialog;
        }

        public Command Id
        {
            get { return Command.ContactKingResponse; }
        }

        public Dialog Discussion
        {
            get { return discussion; }
            set { discussion = value; }
        }
    }
}
