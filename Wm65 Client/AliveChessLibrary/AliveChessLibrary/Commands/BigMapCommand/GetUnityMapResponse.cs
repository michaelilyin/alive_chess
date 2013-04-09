namespace AliveChessLibrary.Commands.BigMapCommand
{
    public class GetUnityMapResponse : INonSerializable
    {
        private byte[] array;

        public GetUnityMapResponse(byte[] array)
        {
            this.array = array;
        }

        public Command Id
        {
            get { return Command.GetUnityMapResponse; }
        }

        public byte[] ToBytes() { return array; }
    }
}
