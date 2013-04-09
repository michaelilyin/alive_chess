using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace WindowsMobileClientAliveChess.NetLayer.Executors
{
    class BuildingInCastleResponseExecutor:IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;

        public BuildingInCastleResponseExecutor(Game context)
        {
            
        }

        public void Execute(ICommand cmd)
        {
            
        }

        public delegate void UpdateResourceHandler(IInnerBuilding str);
    }

    }

