using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.NetLayer.Executors.DialogExecutors
{
    public class MessagePayOffExecutor : IExecutor
    {
        private Game context;
        private DisputeHandler handler;

        public MessagePayOffExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            PayOffDialogMessage msg = (PayOffDialogMessage)cmd;

            if (msg.State == DialogState.Offer)
            {
                handler = new DisputeHandler(context.GameForm.StartSimplePayOffDialog);
                context.GameForm.Invoke(handler);
            }
            else if (msg.State == DialogState.Refuse)
            {
                handler = new DisputeHandler(context.GameForm.BattleDisputeControl.RefusePayOffAnswer);
                context.GameForm.Invoke(handler);
                context.Dispute = null;
            }
            else if (msg.State == DialogState.Agree)
            {
                handler = new DisputeHandler(context.GameForm.BattleDisputeControl.AgreePayOffAnswer);
                context.GameForm.Invoke(handler);
                context.Dispute = null;
            }
        }

        private delegate void DisputeHandler();
    }
}
