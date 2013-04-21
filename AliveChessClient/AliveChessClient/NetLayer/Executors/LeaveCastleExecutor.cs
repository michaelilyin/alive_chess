using AliveChessClient.GameLayer;
using AliveChessClient.GameLayer.AliveChessGraphics;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;

namespace AliveChessClient.NetLayer.Executors
{
    public class LeaveCastleExecutor : IExecutor
    {
        private Game context;
        private LeaveCastleHandler handler;
        private DrawHandler draw;

        public LeaveCastleExecutor(Game context)
        {
            this.context = context;
            this.handler = new LeaveCastleHandler(context.GameForm.StartBigMap);
            this.draw = new DrawHandler(context.BigMap.GraphicManager.Draw);
        }

        public void Execute(ICommand cmd)
        {
            LeaveCastleResponse resp = (LeaveCastleResponse)cmd;
            context.Player.King.LeaveCastle();

            context.GameForm.Invoke(handler);

            context.BigMap.UpdateKingVisibleSpace(context.Player.King);

            context.GameForm.Invoke(draw);
        }

        public delegate void LeaveCastleHandler();
    }
}
