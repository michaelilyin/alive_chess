using System.Collections.Generic;
using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class MoveKingExecutor : IExecutor
    {
        private Game context;

        public MoveKingExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {

        }
    }
}
