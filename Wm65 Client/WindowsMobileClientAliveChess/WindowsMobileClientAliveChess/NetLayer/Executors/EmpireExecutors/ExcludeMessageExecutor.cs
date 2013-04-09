using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class ExcludeMessageExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate handler;

        public ExcludeMessageExecutor(Game context)
        {
        
        }

        public void Execute(ICommand cmd)
        {
            ExcludeFromEmpireMessage resp = (ExcludeFromEmpireMessage)cmd;
            context.GameForm.Invoke(handler);
        }
    }
}
