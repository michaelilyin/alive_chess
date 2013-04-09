using System.Collections.Generic;
using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BattleCommand;
using AliveChessLibrary.GameObjects.Characters;

namespace WindowsMobileClientAliveChess.NetLayer.Executors
{
    class DownloadBattlefildExecutor : IExecutor
    {
        private Game context;
        private Battlefild Bf;
        private Batlecont bat;
        public DownloadBattlefildExecutor(Game context)
        {

        }

        public void Execute(ICommand cmd)
        {
            
        }

        public delegate void Battlefild(IList<Unit> arm, IList<Unit> oppArm, bool course);
        public delegate void Batlecont();
    }
}
