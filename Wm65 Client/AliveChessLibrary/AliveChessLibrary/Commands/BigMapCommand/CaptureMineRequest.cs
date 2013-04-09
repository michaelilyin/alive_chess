using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class CaptureMineRequest : ICommand
    {
        [ProtoMember(1)]
        private int mineId;

        public Command Id
        {
            get { return Command.CaptureMineRequest; }
        }

        public int MineId
        {
            get { return mineId; }
            set { mineId = value; }
        }
    }
}
