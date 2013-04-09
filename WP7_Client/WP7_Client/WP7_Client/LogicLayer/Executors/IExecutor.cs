using AliveChessLibrary.Commands;

namespace WP7_Client.LogicLayer.Executors
{
    public interface IExecutor
    {
        void Execute(ICommand command);
    }
}
