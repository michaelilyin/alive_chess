using WindowsMobileClientAliveChess.GameLayer;
using WindowsMobileClientAliveChess.GameLayer.AliveChessGraphics;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class TakeResourceExecutor : IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;
        private DrawHandler handler;

        public TakeResourceExecutor(Game context)
        {


        }

        public void Execute(ICommand cmd)
        {


        }

        public delegate void UpdateResourceHandler(object rCount, object rType);
    }
}
