using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class DisputeKingExecutor : IExecutor
    {
        private Game context;
        private DisputHandler handler;

        public DisputeKingExecutor(Game context)
        {
            this.context = context;
            this.handler = new DisputHandler(context.GameForm.StartMainDialog);
        }

        public void Execute(ICommand cmd)
        {
            ContactKingResponse resp = (ContactKingResponse)cmd;

            context.Dispute = resp.Discussion;
            context.Dispute.Organizator = context.Player.King;
            context.Player.King.State = KingState.Dispute;

            if (!resp.Discussion.YouStep)
            {
                StartDisputeHandler handler = new StartDisputeHandler(context.GameForm.StartKingInfoDialog);
                context.GameForm.Invoke(handler, resp.Discussion.Respondent);
            }
            else
            {
                DisputHandler handler = new DisputHandler(context.GameForm.StartMainDialog);
                context.GameForm.Invoke(handler);
            }
        }

        private delegate void DisputHandler();
        private delegate void StartDisputeHandler(King king);
    }
}
