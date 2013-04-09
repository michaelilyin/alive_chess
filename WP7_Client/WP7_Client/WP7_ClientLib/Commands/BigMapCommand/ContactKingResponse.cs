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
        [ProtoMember(1)]
        public Dialog Dialog
        {
            get { return _dialog; }
            set { _dialog = value; }
        }
    }
}
