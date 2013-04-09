using AliveChessLibrary.Commands;

namespace WindowsMobileClientAliveChess.NetLayer.Executors
{
    public interface IExecutor
    {
        void Execute(ICommand cmd);
    }
}
