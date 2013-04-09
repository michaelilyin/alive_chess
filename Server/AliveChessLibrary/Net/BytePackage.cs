using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.Net
{
    public class BytePackage : IBytePackage
    {
        private string commandName;
        private string commandType;
        private int commandSize;
        private byte[] commandBody;

        public BytePackage()
        {
            commandType = string.Empty;
        }

        public int CommandSize
        {
            get { return commandSize; }
            set { commandSize = value; }
        }

        public string CommandName
        {
            get { return commandName; }
            set { commandName = value; }
        }

        public string CommandType
        {
            get { return commandType; }
            set { commandType = value; }
        }

        public byte[] CommandBody
        {
            get { return commandBody; }
            set { commandBody = value; }
        }
    }
}
