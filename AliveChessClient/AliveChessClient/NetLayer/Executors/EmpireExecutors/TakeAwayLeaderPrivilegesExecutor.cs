using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class TakeAwayLeaderPrivilegesExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate handler;

        public TakeAwayLeaderPrivilegesExecutor(Game context)
        {
            this.context = context;
            handler = delegate()
                          {
                              context.GameForm.AlianceControl.DeactivateLeaderButton();
                          };
        }

        public void Execute(ICommand cmd)
        {
            TakeAwayLeaderPrivilegesMessage resp = (TakeAwayLeaderPrivilegesMessage)cmd;
            context.GameForm.Invoke(handler);
        }
    }
}
