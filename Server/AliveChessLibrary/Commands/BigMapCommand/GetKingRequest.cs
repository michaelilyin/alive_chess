using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос короля
    /// </summary>
    [ProtoContract]
    public class GetKingRequest : ICommand
    {
        [ProtoMember(1)]
        private int _kingId;

        public Command Id
        {
            get { return Command.GetKingRequest; }
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
