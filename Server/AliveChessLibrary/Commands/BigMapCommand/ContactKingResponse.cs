using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// взаимодействие с королем
    /// </summary>
    [ProtoContract]
    public class ContactKingResponse : ICommand
    {
        [ProtoMember(1)]
        private Dialog _dialog;

        public ContactKingResponse()
        {
        }

        public ContactKingResponse(Dialog dialog)
        {
            this._dialog = dialog;
        }

        public Command Id
        {
            get { return Command.ContactKingResponse; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public Dialog Dialog
        {
            get { return _dialog; }
            set { _dialog = value; }
        }
    }
}
