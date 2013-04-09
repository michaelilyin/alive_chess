using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class StartImpeachmentExecutor : IExecutor
    {
         private Game context;

         public StartImpeachmentExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            StartImpeachmentResponse resp = (StartImpeachmentResponse)cmd;
        }
    }
}
