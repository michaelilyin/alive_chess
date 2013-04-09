namespace AliveChessLibrary.Commands
{
    public interface INonSerializable : ICommand
    {
        byte[] ToBytes();
    }
}
