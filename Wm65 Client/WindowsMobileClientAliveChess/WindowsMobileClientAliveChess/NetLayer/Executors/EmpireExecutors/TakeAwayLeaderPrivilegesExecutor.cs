using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class TakeAwayLeaderPrivilegesExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate handler;

        public TakeAwayLeaderPrivilegesExecutor(Game context)
        {


        }

        public void Execute(ICommand cmd)
        {
            TakeAwayLeaderPrivilegesMessage resp = (TakeAwayLeaderPrivilegesMessage)cmd;
            context.GameForm.Invoke(handler);
        }
    }
}
