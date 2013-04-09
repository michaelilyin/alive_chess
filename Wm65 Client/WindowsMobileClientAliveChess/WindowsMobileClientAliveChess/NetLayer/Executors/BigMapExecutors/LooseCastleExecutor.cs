using System.Collections.Generic;
using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class LooseCastleExecutor : IExecutor
    {
        private Game context;
        private CastlesHandler handler;

        public LooseCastleExecutor(Game context)
        {
            
        }

        public void Execute(ICommand cmd)
        {
            
        }

        public delegate void CastlesHandler(int count);
    }
}
