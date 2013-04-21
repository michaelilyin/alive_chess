using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.NetLayer.Executors.DialogExecutors
{
    public class MessageCapitulateExecutor : IExecutor
    {
        private Game context;
        private DisputeHandler handler;

        public MessageCapitulateExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            CapitulateDialogMessage msg = (CapitulateDialogMessage)cmd;

            if (msg.State == DialogState.Offer)
            {
                handler = new DisputeHandler(context.GameForm.StartSimpleCapitulateDialog);
                context.GameForm.Invoke(handler);
            }
            else if (msg.State == DialogState.Refuse)
            {
                handler = new DisputeHandler(context.GameForm.BattleDisputeControl.RefuseCapitulateAnswer);
                context.GameForm.Invoke(handler);
                context.Negotiate = null;
            }
            else if (msg.State == DialogState.Agree)
            {
                handler = new DisputeHandler(context.GameForm.BattleDisputeControl.AgreeCapitulateAnswer);
                context.GameForm.Invoke(handler);
                context.Negotiate = null;
            }
        }

        private delegate void DisputeHandler();
    }
}
