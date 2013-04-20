using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class ExcludeMessageExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate handler;

        public ExcludeMessageExecutor(Game context)
        {
            this.context = context;
            this.handler = new AliveChessDelegate(context.GameForm.BigMapControl.DeactivateUnionButton);
        }

        public void Execute(ICommand cmd)
        {
            ExcludeFromEmpireMessage resp = (ExcludeFromEmpireMessage)cmd;
            context.GameForm.Invoke(handler);
        }
    }
}
