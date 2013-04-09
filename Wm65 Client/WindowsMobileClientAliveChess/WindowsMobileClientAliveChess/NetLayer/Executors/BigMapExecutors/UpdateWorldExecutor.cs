using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class UpdateWorldExecutor : IExecutor
    {
        private Game context;

        public UpdateWorldExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            
        }
    }
}
