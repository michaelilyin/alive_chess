using WindowsMobileClientAliveChess.GameLayer;
using WindowsMobileClientAliveChess.GameLayer.AliveChessGraphics;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors
{
    public class LeaveCastleExecutor : IExecutor
    {
        private Game context;
        private LeaveCastleHandler handler;
        private DrawHandler draw;

        public LeaveCastleExecutor(Game context)
        {

        }

        public void Execute(ICommand cmd)
        {

        }

        public delegate void LeaveCastleHandler();
    }
}
