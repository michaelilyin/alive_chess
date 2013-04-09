using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.DialogExecutors
{
    public class JoinEmperiesMessageExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate handler;

        public JoinEmperiesMessageExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            
        }
    }
}
