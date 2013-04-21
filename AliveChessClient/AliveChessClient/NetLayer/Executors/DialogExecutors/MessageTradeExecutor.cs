using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.NetLayer.Executors.DialogExecutors
{
    public class TradeMsgExecutor : IExecutor
    {
        private Game context;
        private DisputeHandler handler;

        public TradeMsgExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            MarketDialogMessage msg = (MarketDialogMessage)cmd;

            if (msg.State == DialogState.Offer)
            {
                handler = new DisputeHandler(context.GameForm.StartSimpleTradeDialog);
                context.GameForm.Invoke(handler);
            }
            else if (msg.State == DialogState.Refuse)
            {
                handler = new DisputeHandler(context.GameForm.MainDisputControl.RefuseTradeAnswer);
                context.GameForm.Invoke(handler);
                context.Dispute = null;
            }
            else if (msg.State == DialogState.Agree)
            {
                handler = new DisputeHandler(context.GameForm.MainDisputControl.AgreeTradeAnswer);
                context.GameForm.Invoke(handler);
                context.Dispute = null;
            }
        }

        private delegate void DisputeHandler();
    }
}
