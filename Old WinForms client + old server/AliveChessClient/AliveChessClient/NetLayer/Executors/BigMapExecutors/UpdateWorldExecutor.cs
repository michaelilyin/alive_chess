using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class UpdateWorldExecutor : IExecutor
    {
        private Game context;

        public UpdateWorldExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            UpdateWorldMessage update = (UpdateWorldMessage)cmd;
            if (context.Player.Ready)
                context.BigMap.PutNewState(update.MObject);
        }
    }
}
