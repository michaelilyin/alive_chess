using WindowsMobileClientAliveChess.GameLayer;
using WindowsMobileClientAliveChess.GameLayer.AliveChessGraphics;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class GetResourceExecutor : IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;
        private DrawHandler handler;

        public GetResourceExecutor(Game context)
        {

        }

        public void Execute(ICommand cmd)
        {
           
        }

        public delegate void UpdateResourceHandler(object rCount, object rType);
    }
}
