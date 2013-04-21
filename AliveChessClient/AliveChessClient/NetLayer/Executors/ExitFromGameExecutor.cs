using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.RegisterCommand;

namespace AliveChessClient.NetLayer.Executors
{
    public class ExitFromGameExecutor : IExecutor
    {
        private Game game;
        private StopGameHandler handler;

        public ExitFromGameExecutor(Game game)
        {
            this.game = game;
            this.handler = new StopGameHandler(game.Stop);
        }

        public void Execute(ICommand cmd)
        {
            ExitFromGameResponse exit = (ExitFromGameResponse)cmd;
            game.GameForm.Invoke(handler);
        }

        delegate void StopGameHandler();
    }
}
