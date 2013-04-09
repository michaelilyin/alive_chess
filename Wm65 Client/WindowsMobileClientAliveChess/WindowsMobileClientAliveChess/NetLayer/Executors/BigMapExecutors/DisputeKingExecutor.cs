using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Characters;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class DisputeKingExecutor : IExecutor
    {
        private Game context;
        private DisputHandler handler;

        public DisputeKingExecutor(Game context)
        {
        }

        public void Execute(ICommand cmd)
        {

        }

        private delegate void DisputHandler();
        private delegate void StartDisputeHandler(King king);
    }
}
