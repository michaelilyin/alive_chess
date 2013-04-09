using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Characters;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class StartNegotiateExecutor : IExecutor
    {
        private Game context;
        private DisputHandler handler;

        public StartNegotiateExecutor(Game context)
        {

        }

        public void Execute(ICommand cmd)
        {

        }

        private delegate void DisputHandler();
        private delegate void StartDisputeHandler(King king);
    }
}
