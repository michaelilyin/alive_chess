using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос захвата замка
    /// </summary>
    [ProtoContract]
    public class CaptureCastleRequest : ICommand
    {
        [ProtoMember(1)]
        private int _castleId;

        /// <summary>
        /// Идентификатор:8
        /// </summary>
        public Command Id
        {
            get { return Command.CaptureCastleRequest; }
        }

        /// <summary>
        /// Идентификатор замка. Прото-атрибут: 1
        /// </summary>
        public int CastleId
        {
            get { return _castleId; }
            set { _castleId = value; }
        }
    }
}
