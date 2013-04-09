using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// взаимодействие с замком
    /// </summary>
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

        /// <summary>
        /// Прото-атрибут: 2
        /// </summary>
        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public Dialog Dispute
        {
            get { return _dispute; }
            set { _dispute = value; }
        }
    }
}
