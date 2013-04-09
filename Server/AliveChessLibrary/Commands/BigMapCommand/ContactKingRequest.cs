using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос взаимодействия с королем
    /// </summary>
    [ProtoContract]
    public class ContactKingRequest : ICommand
    {
        [ProtoMember(1)]
        private int _kingId;

        public Command Id
        {
            get { return Command.ContactKingRequest; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public int KingId
        {
            get { return _kingId; }
            set { _kingId = value; }
        }
    }
}
