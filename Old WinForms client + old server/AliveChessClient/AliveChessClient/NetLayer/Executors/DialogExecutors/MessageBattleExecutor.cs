using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.NetLayer.Executors.DialogExecutors
{
    public class MessageBattleExecutor : IExecutor
    {
        private Game context;
        private DisputeHandler handler;

        public MessageBattleExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            BattleDialogMessage msg = (BattleDialogMessage)cmd;

            if (msg.State == DialogState.Offer)
            {
                handler = new DisputeHandler(context.GameForm.StartBattleDialog);
                context.GameForm.Invoke(handler);
            }
            else if (msg.State == DialogState.Agree)
            {
                handler = new DisputeHandler(context.GameForm.MainDisputControl.AgreeBattleAnswer);
                context.GameForm.Invoke(handler);
                context.Dispute = null;
            }
        }

        private delegate void DisputeHandler();
    }
}
