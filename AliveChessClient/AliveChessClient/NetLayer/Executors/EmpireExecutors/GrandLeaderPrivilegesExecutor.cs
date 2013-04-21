using AliveChessClient.GameLayer;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class GrandLeaderPrivilegesExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegateWithText h;
        private AliveChessDelegate handler;

        public GrandLeaderPrivilegesExecutor(Game context)
        {
            this.context = context;
            this.h = new AliveChessDelegateWithText(context.GameForm.BigMapControl.SetText);
            this.handler = new AliveChessDelegate(context.GameForm.AlianceControl.ActivateLeaderButton);
        }

        public void Execute(ICommand cmd)
        {
            GrandLeaderPrivilegesMessage resp = (GrandLeaderPrivilegesMessage)cmd;
            context.BigMap.Invoke(h, "You are new leader");
            context.BigMap.Invoke(handler);

            QueryManager.SendGetAliancesInfo(context.Player);
        }
    }
}
