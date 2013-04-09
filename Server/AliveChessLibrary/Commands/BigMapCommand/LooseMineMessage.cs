using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// сообщение о потере шахты
    /// </summary>
    [ProtoContract]
    public class LooseMineMessage : ICommand
    {
        [ProtoMember(1)]
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

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public int MineId
        {
            get { return _mineId; }
            set { _mineId = value; }
        }
    }
}
