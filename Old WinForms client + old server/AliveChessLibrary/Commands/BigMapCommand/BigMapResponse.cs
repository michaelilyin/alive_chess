using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class BigMapResponse : ICommand
    {
        [ProtoMember(1)]
        private bool isAllowed;

        public BigMapResponse()
        {
        }

        public BigMapResponse(bool isAllowed)
        {
            this.isAllowed = isAllowed;
        }

        public Command Id
        {
            get { return Command.BigMapResponse; }
        }

        public bool IsAllowed
        {
            get { return isAllowed; }
            set { isAllowed = value; }
        }
    }
}
