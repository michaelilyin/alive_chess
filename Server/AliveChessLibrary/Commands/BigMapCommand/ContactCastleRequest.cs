using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос взаимодействия с замком
    /// </summary>
    [ProtoContract]
    public class ContactCastleRequest : ICommand
    {
        [ProtoMember(1)]
        private int _castleId;

        public Command Id
        {
            get { return Command.ContactCastleRequest; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public int CastleId
        {
            get { return _castleId; }
            set { _castleId = value; }
        }
    }
}
