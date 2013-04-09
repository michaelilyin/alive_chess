using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// сообщение о потере шахты
    /// </summary>
    [ProtoContract]
    public class LooseMineMessage : ICommand
    {
        private int _mineId;

        public LooseMineMessage()
        {
        }

        public LooseMineMessage(int mineId)
        {
            this._mineId = mineId;
        }

        public Command Id
        {
            get { return Command.LooseMineMessage; }
        }

        [ProtoMember(1)]
        public int MineId
        {
            get { return _mineId; }
            set { _mineId = value; }
        }
    }
}
