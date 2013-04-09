using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Characters;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class BigMapExecutor : IExecutor
    {
        private Game context;
        private BigMapHandler handler;
       
        public BigMapExecutor(Game context)
        {

        }

        public void Execute(ICommand cmd)
        {

        }

        private delegate void BigMapHandler();
    }
}
