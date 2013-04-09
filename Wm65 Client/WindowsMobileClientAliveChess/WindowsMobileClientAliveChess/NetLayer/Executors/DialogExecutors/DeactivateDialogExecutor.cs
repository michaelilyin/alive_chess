using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.DialogExecutors
{
    public class DeactivateDialogExecutor : IExecutor
    {
        private Game context;
        private DisabledHandler handler;

        public DeactivateDialogExecutor(Game context)
        {
            

        }

        public void Execute(ICommand cmd)
        {

        }

        private delegate void DisabledHandler();
    }
}
