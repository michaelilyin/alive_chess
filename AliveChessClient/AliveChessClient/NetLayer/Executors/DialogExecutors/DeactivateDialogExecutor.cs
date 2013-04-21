using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;

namespace AliveChessClient.NetLayer.Executors.DialogExecutors
{
    public class DeactivateDialogExecutor : IExecutor
    {
        private Game context;
        private DisabledHandler handler;

        public DeactivateDialogExecutor(Game context)
        {
            this.context = context;
            this.handler = new DisabledHandler(context.GameForm.BattleDisputeControl.SetDisabled);
        }

        public void Execute(ICommand cmd)
        {
            DeactivateDialogMessage resp = (DeactivateDialogMessage)cmd;
            context.GameForm.BattleDisputeControl.Invoke(handler);
            //QueryManager.SendBigMapMessage(context.Player);
        }

        private delegate void DisabledHandler();
    }
}
