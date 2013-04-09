using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос входа в замок
    /// </summary>
    [ProtoContract]
    public class ComeInCastleRequest : ICommand
    {
        [ProtoMember(1)]
        private int _castleId;

        public Command Id
        {
            get { return Command.ComeInCastleRequest; }
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
