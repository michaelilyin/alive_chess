using System.Threading;
using WindowsMobileClientAliveChess.GameLayer;
using WindowsMobileClientAliveChess.NetLayer.Main;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class CaptureMineExecutor : IExecutor
    {
        private Game context;
        private MinesHandler handler;

        public CaptureMineExecutor(Game context)
        {
            
        }

        public void Execute(ICommand cmd)
        {
            
        }

        public delegate void MinesHandler(int count);
    }
}
