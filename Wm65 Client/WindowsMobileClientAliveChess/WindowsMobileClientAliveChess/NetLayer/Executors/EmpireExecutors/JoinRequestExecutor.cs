using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class JoinRequestExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegateWithID handler;

        public JoinRequestExecutor(Game context)
        {
            
        }

        public void Execute(ICommand cmd)
        {
 
        }
    }
}
