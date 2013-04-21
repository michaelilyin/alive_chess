using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class CreateUnionExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate handler;

        public CreateUnionExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            CreateUnionDialogMessage msg = (CreateUnionDialogMessage)cmd;

            if (msg.State == DialogState.Offer)
            {
                handler = new AliveChessDelegate(context.GameForm.StartEmpireDialog);
                context.GameForm.Invoke(handler);
            }
            else if (msg.State == DialogState.Refuse)
            {
                handler = new AliveChessDelegate(context.GameForm.MainDisputControl.RefuseEmpireAnswer);
                context.GameForm.Invoke(handler);
                context.Dispute = null;
            }
            else if (msg.State == DialogState.Agree)
            {
                handler = new AliveChessDelegate(context.GameForm.MainDisputControl.AgreeEmpireAnswer);
                context.GameForm.Invoke(handler);
                context.Dispute = null;
            }
        }
    }
}
