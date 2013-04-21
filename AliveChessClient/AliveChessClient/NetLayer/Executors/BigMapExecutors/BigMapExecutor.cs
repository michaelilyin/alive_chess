using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class BigMapExecutor : IExecutor
    {
        private Game context;
        private BigMapHandler handler;
       
        public BigMapExecutor(Game context)
        {
            this.context = context;
            this.handler = new BigMapHandler(context.GameForm.StartBigMap);
        }

        public void Execute(ICommand cmd)
        {
            BigMapResponse resp = (BigMapResponse)cmd;
            context.Player.King.State = KingState.BigMap;
            context.GameForm.Invoke(handler);

            context.BigMap.UpdateKingVisibleSpace(context.Player.King);
        }

        private delegate void BigMapHandler();
    }
}
