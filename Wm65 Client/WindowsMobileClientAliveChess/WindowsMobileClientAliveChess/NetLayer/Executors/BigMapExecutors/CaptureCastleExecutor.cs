using System.Collections.Generic;
using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Resources;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class CaptureCastleExecutor : IExecutor
    {
        private Game context;
        private CastleHandler handler;

        public CaptureCastleExecutor(Game context)
        {
            
        }

        public void Execute(ICommand cmd)
        {
            
        }

        public delegate void CastleHandler(int count);
    }
}
