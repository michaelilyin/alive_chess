using System;
using System.Windows.Forms;
using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.Interaction;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class NewsExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegateWithText handler;
        private KingList h1;
       // private KingList h2;

        public NewsExecutor(Game context)
        {
            

        }

        public void Execute(ICommand cmd)
        {
 

        }

        private delegate void KingList(int id);
    }
}
