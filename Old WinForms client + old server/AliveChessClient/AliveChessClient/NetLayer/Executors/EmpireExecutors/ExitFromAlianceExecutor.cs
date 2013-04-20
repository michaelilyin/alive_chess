using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class ExitFromAlianceExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate handler;

        public ExitFromAlianceExecutor(Game context)
        {
            this.context = context;
            this.handler = new AliveChessDelegate(context.GameForm.BigMapControl.DeactivateUnionButton);
        }

        public void Execute(ICommand cmd)
        {
            ExitFromAlianceResponse resp = (ExitFromAlianceResponse)cmd;
            context.GameForm.Invoke(handler);
        }
    }
}
