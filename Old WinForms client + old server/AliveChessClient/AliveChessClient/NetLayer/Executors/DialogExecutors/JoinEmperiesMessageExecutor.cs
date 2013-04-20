using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.NetLayer.Executors.DialogExecutors
{
    public class JoinEmperiesMessageExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegate handler;

        public JoinEmperiesMessageExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            JoinEmperiesDialogMessage msg = (JoinEmperiesDialogMessage)cmd;
            //context.GameForm.Invoke(handler, "Emperies is joined");
            if (msg.State == DialogState.Offer)
            {
                handler = delegate()
                {
                    context.GameForm.StartSimpleJoiningDialog();
                };

                context.GameForm.Invoke(handler);
            }
            else if (msg.State == DialogState.Refuse)
            {
                handler = delegate()
                {
                    context.GameForm.NegotiateControl.RefuseEmperiesJoinAnswer();
                };

                context.GameForm.Invoke(handler);
                context.Negotiate = null;
            }
            else if (msg.State == DialogState.Agree)
            {
                handler = delegate()
                {
                    context.GameForm.NegotiateControl.AgreeEmperiesJoinAnswer();
                };

                context.GameForm.Invoke(handler);
                context.Negotiate = null;
            }
        }
    }
}
