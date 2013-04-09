using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
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
