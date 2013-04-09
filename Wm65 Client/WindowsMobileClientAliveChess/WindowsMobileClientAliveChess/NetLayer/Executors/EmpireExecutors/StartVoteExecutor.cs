using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class StartVoteExecutor : IExecutor
    {
        private Game context;

        public StartVoteExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            StartVoteResponse resp = (StartVoteResponse)cmd;
        }
    }
}
