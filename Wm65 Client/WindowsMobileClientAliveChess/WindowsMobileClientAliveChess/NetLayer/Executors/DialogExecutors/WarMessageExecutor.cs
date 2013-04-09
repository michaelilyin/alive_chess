using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.DialogExecutors
{
    public class WarMessageExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegateWithText handler;

        public WarMessageExecutor(Game context)
        {

        }

        public void Execute(ICommand cmd)
        {

        }
    }
}
