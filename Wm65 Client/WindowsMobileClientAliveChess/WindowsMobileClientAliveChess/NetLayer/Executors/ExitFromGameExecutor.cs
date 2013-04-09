using System.Windows.Forms;
using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.RegisterCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors
{
    public class ExitFromGameExecutor : IExecutor
    {
        private Game game;
        private event StopGameHandler handler;

        public ExitFromGameExecutor(Game game)
        {
            this.game = game;
            this.handler += new StopGameHandler(game.Stop);
            this.handler += new StopGameHandler(Application.Exit);
        }

        public void Execute(ICommand cmd)
        {
            ExitFromGameResponse exit = (ExitFromGameResponse)cmd;
            game.GameForm.Invoke(handler);
        }

        delegate void StopGameHandler();
    }
}
