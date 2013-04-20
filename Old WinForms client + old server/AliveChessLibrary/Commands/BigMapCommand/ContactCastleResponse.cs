using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class ContactCastleResponse : ICommand
    {
        [ProtoMember(1)]
        private Dialog _dispute;
        [ProtoMember(2)]
        private Castle _castle;

        public ContactCastleResponse()
        {
        }

        public ContactCastleResponse(Castle castle, Dialog dialog)
        {
            this._castle = castle;
            this._dispute = dialog;
        }

        public Command Id
        {
            get { return Command.ContactCastleResponse; }
        }

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

        public Dialog Dispute
        {
            get { return _dispute; }
            set { _dispute = value; }
        }
    }
}
