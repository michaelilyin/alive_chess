using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class JoinToAlianceExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate h;

        public JoinToAlianceExecutor(Game context)
        {
            this.context = context;
            this.h = new AliveChessDelegate(context.GameForm.BigMapControl.ActivateUnionButton);
        }

        public void Execute(ICommand cmd)
        {
            JoinToAlianceResponse resp = (JoinToAlianceResponse)cmd;
            if (resp.Successed)
                context.GameForm.Invoke(h);
        }
    }
}
