using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors
{
    public interface IExecutor
    {
        void Execute(Message msg);
    }
}
