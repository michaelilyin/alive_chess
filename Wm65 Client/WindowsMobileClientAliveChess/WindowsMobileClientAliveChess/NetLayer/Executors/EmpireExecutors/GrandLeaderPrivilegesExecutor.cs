using WindowsMobileClientAliveChess.GameLayer;
using WindowsMobileClientAliveChess.NetLayer.Main;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class GrandLeaderPrivilegesExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegateWithText h;
        private AliveChessDelegate handler;

        public GrandLeaderPrivilegesExecutor(Game context)
        {
            
        }

        public void Execute(ICommand cmd)
        {

        }
    }
}
