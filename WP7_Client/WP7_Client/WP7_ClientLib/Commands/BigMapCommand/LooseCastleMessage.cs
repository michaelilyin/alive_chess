using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// сообщение о потере замка
    /// </summary>
    [ProtoContract]
    public class LooseCastleMessage : ICommand
    {
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

        [ProtoMember(1)]
        public int CastleId
        {
            get { return _castleId; }
            set { _castleId = value; }
        }
    }
}
