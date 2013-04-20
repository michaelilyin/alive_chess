using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class StartNegotiateExecutor : IExecutor
    {
        private Game context;
        private DisputHandler handler;

        public StartNegotiateExecutor(Game context)
        {
            this.context = context;
            this.handler = new DisputHandler(context.GameForm.StartNegotiateDialog);
        }

        public void Execute(ICommand cmd)
        {
            StartNegotiateResponse resp = (StartNegotiateResponse)cmd;

            context.Negotiate = resp.Negotiate;
            context.Negotiate.Organizator = context.Player.King;
            context.Player.King.State = KingState.Negotiate;

            if (!resp.Negotiate.YouStep)
            {
                StartDisputeHandler handler = new StartDisputeHandler(context.GameForm.StartKingInfoDialog);
                context.GameForm.Invoke(handler, resp.Negotiate.Respondent);
            }
            else
            {
                DisputHandler handler = new DisputHandler(context.GameForm.StartNegotiateDialog);
                context.GameForm.Invoke(handler);
            }
        }

        private delegate void DisputHandler();
        private delegate void StartDisputeHandler(King king);
    }
}
