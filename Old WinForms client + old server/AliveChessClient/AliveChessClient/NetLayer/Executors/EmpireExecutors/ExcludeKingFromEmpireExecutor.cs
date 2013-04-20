using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class ExcludeKingFromEmpireExecutor : IExecutor
    {
        private Game context;

        public ExcludeKingFromEmpireExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            ExcludeKingFromEmpireResponse resp = (ExcludeKingFromEmpireResponse)cmd;
        }
    }
}
