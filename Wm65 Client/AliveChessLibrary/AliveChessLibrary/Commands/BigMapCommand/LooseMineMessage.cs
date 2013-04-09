using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class LooseMineMessage : ICommand
    {
        [ProtoMember(1)]
        private int mineId;

        public LooseMineMessage()
        {
        }

        public LooseMineMessage(int mineId)
        {
            this.mineId = mineId;
        }

        public Command Id
        {
            get { return Command.LooseMineMessage; }
        }

        public int MineId
        {
            get { return mineId; }
            set { mineId = value; }
        }
    }
}
