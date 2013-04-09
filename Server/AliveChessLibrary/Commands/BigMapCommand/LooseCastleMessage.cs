using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// сообщение о потере замка
    /// </summary>
    [ProtoContract]
    public class LooseCastleMessage : ICommand
    {
        [ProtoMember(1)]
        private int _castleId;

        public LooseCastleMessage()
        {
        }

        public LooseCastleMessage(int castleId)
        {
            this._castleId = castleId;
        }

        public Command Id
        {
            get { return Command.LooseCastleMessage; }
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
