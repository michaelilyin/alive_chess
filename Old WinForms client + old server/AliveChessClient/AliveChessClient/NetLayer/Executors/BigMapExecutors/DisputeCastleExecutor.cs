using AliveChessClient.GameLayer;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class DisputeCastleExecutor : IExecutor
    {
        private Game context;

        public DisputeCastleExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            ContactCastleResponse resp = (ContactCastleResponse)cmd;

            if (resp.Dispute != null)
            {
                context.Dispute = resp.Dispute;
                context.Dispute.Organizator = context.Player.King;
                context.Player.King.State = KingState.Dispute;

                if (!resp.Dispute.YouStep)
                {
                    StartDisputeHandler handler = new StartDisputeHandler(context.GameForm.StartKingInfoDialog);
                    context.GameForm.Invoke(handler, resp.Dispute.Respondent);
                }
                else
                {
                    CastleInfoHandler handler = new CastleInfoHandler(context.GameForm.StartCastleInfoControl);
                    context.GameForm.Invoke(handler, resp.Castle);
                }
            }
            else
            {
                CaptureCastleRequest request = new CaptureCastleRequest();
                request.CastleId = resp.Castle.Id;
                ClientApplication.Instance.Transport.Send<CaptureCastleRequest>(request);
            }
        }

        private delegate void StartDisputeHandler(King king);
        private delegate void CastleInfoHandler(Castle castle);
    }
}
