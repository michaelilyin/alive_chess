using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// вход в замок
    /// </summary>
    [ProtoContract]
    public class ComeInCastleResponse : ICommand
    {
        private int _castleId;

        public ComeInCastleResponse()
        {
        }

        public ComeInCastleResponse(int castleId)
        {
            this._castleId = castleId;
        }

        public Command Id
        {
            get { return Command.ComeInCastleResponse; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        [ProtoMember(1)]
        public int CastleId
        {
            get { return _castleId; }
            set { _castleId = value; }
        }
    }
}
