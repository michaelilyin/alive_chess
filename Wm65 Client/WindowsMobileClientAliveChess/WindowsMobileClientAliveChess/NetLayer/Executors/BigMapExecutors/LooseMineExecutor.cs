using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class LooseMineExecutor : IExecutor
    {
        private Game context;
        private MinesHandler handler;

        public LooseMineExecutor(Game context)
        {
       
        }

        public void Execute(ICommand cmd)
        {
            
        }

        public delegate void MinesHandler(int count);
    }
}
