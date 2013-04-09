using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace WindowsMobileClientAliveChess.NetLayer.Executors
{
    class GetRecBuildingsExequtor : IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;

        public GetRecBuildingsExequtor(Game context)
        {
        }

        public void Execute(ICommand cmd)
        {
             
        }

        public delegate void UpdateResourceHandler(ResBuild res);
        //public delegate void UpdateResourceHandler(ResourceTypes t, int count);
    }
}
