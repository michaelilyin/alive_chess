using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class CreateUnionExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate handler;

        public CreateUnionExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            
        }
    }
}
