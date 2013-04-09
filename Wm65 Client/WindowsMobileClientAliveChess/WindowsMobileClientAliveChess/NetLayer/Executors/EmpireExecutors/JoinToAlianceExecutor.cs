using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class JoinToAlianceExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate h;

        public JoinToAlianceExecutor(Game context)
        {

        }

        public void Execute(ICommand cmd)
        {
            JoinToAlianceResponse resp = (JoinToAlianceResponse)cmd;
            if (resp.Successed)
                context.GameForm.Invoke(h);
        }
    }
}
