using AliveChessLibrary.Commands;

namespace AliveChessClient.NetLayer.Executors
{
    public interface IExecutor
    {
        void Execute(ICommand cmd);
    }
}
