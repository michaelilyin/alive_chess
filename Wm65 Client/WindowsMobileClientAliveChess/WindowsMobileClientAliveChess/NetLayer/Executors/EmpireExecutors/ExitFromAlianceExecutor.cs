using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class ExitFromAlianceExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate handler;

        public ExitFromAlianceExecutor(Game context)
        {
         }

        public void Execute(ICommand cmd)
        {
            ExitFromAlianceResponse resp = (ExitFromAlianceResponse)cmd;
            context.GameForm.Invoke(handler);
        }
    }
}
