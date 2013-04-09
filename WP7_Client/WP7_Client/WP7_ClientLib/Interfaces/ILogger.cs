namespace AliveChessLibrary.Interfaces
{
    public interface ILogger
    {
        void Log(string filename, string str);

        void Log(string filename, params string[] str);
    }
}
