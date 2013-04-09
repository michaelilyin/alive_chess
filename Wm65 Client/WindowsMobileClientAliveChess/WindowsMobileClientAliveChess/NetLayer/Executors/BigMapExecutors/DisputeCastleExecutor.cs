using WindowsMobileClientAliveChess.GameLayer;
using WindowsMobileClientAliveChess.NetLayer.Main;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class DisputeCastleExecutor : IExecutor
    {
        private Game context;

        public DisputeCastleExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            
        }

        private delegate void StartDisputeHandler(King king);
        private delegate void CastleInfoHandler(Castle castle);
    }
}
