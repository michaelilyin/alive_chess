using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос взаимодействия с королем
    /// </summary>
    [ProtoContract]
    public class ContactKingRequest : ICommand
    {
        private int _kingId;

        public Command Id
        {
            get { return Command.ContactKingRequest; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        [ProtoMember(1)]
        public int KingId
        {
            get { return _kingId; }
            set { _kingId = value; }
        }
    }
}
