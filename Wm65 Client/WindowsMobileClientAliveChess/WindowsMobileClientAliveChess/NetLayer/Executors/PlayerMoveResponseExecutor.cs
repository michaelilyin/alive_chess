using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BattleCommand;
//using AliveChessLibrary.Entities.Characters;
//using AliveChessLibrary.Interfaces;

namespace WindowsMobileClientAliveChess.NetLayer.Executors
{
    class PlayerMoveResponseExecutor:IExecutor
    {
        private Game context;
        private MOVE move;
        public PlayerMoveResponseExecutor(Game context)
        {


        }

        public void Execute(ICommand cmd)
        {
            

        }

        public delegate void MOVE(byte [] t, int c, bool step);
    }
}
