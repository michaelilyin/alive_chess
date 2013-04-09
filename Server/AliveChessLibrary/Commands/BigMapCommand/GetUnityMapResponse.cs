namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// получение карты для Unity
    /// </summary>
    public class GetUnityMapResponse : INonSerializable
    {
        private byte[] _array;

        public GetUnityMapResponse(byte[] array)
        {
            this._array = array;
        }

        public Command Id
        {
            get { return Command.GetUnityMapResponse; }
        }

        public byte[] ToBytes() { return _array; }
    }
}
