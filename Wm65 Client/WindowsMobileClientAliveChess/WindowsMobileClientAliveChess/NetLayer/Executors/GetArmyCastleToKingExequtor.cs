using System.Collections.Generic;
using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;

namespace WindowsMobileClientAliveChess.NetLayer.Executors
{
    class GetArmyCastleToKingExequtor:IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;

        public GetArmyCastleToKingExequtor(Game context)
        {

        }

        public void Execute(ICommand cmd)
        {
            
        }

        public delegate void UpdateResourceHandler(IList<Unit> un, int i);
    }
}
