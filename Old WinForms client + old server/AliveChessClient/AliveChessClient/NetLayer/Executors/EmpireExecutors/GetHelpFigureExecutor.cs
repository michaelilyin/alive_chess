using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class GetHelpFigureExecutor : IExecutor
    {
        private Game context;

        public GetHelpFigureExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            GetHelpFigureResponse resp = (GetHelpFigureResponse)cmd;
        }
    }
}
