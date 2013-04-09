using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.DialogExecutors
{
    public class TradeMsgExecutor : IExecutor
    {
        private Game context;
        private DisputeHandler handler;

        public TradeMsgExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {

        }

        private delegate void DisputeHandler();
    }
}
