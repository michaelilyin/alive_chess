using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class JoinRequestExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegateWithID handler;

        public JoinRequestExecutor(Game context)
        {
            this.context = context;
            handler = new AliveChessDelegateWithID(context.GameForm.LeaderControl.AddCandidate);
        }

        public void Execute(ICommand cmd)
        {
            JoinRequestMessage resp = (JoinRequestMessage)cmd;
            context.BigMap.Invoke(handler, resp.Candidate.Id);
        }
    }
}
