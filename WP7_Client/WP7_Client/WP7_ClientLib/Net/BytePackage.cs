using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.Net
{
    public class BytePackage : IBytePackage
    {
        public BytePackage()
        {
            CommandType = string.Empty;
        }

        public int CommandSize { get; set; }

        public string CommandName { get; set; }

        public string CommandType { get; set; }

        public byte[] CommandBody { get; set; }
    }
}
