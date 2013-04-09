using System.Collections.Generic;
using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;
//using AliveChessLibrary.Interfaces;

namespace WindowsMobileClientAliveChess.NetLayer.Executors
{
    class ShowArmyKingExecutor : IExecutor
    {
        private Game context;
        private Show show;

        public ShowArmyKingExecutor(Game cont)
        {

        }

        public void Execute(ICommand cmd)
        {

        }

        public delegate void Show(List<Unit> units);
    }
}
