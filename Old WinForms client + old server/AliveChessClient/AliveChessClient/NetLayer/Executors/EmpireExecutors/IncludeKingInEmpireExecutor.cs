using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class IncludeKingInEmpireExecutor : IExecutor
    {
        private Game context;

        public IncludeKingInEmpireExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            IncludeKingInEmpireResponse resp = (IncludeKingInEmpireResponse)cmd;
        }
    }
}
