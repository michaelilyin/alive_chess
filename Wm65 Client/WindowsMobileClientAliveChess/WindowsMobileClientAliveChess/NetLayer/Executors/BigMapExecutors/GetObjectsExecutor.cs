using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using WindowsMobileClientAliveChess.GameLayer;
using WindowsMobileClientAliveChess.GameLayer.AliveChessGraphics;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Characters;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class GetObjectsExecutor : IExecutor
    {
        private Game game;
        private DrawHandler handler;

        public GetObjectsExecutor(Game game)
        {
            this.game = game;
        }

        #region IExecutor Members

        public void Execute(ICommand cmd)
        {
            GetObjectsResponse objects = (GetObjectsResponse)cmd;
            Monitor.Enter(this.game);
            if (objects.Kings != null)
            {
                foreach (King k in objects.Kings)
                {
                    game.Map.AddKing(k);
                    game.Map.SetObject(Map.CreatePoint(k.X, k.Y, PointTypes.King));
                }
            }
            if (objects.Resources != null)
            {
                foreach (Resource res in objects.Resources)
                {
                    game.Map.AddResource(res);
                    game.Map.SetObject(Map.CreatePoint(res.X, res.Y, PointTypes.Resource));
                }
            }
            Monitor.Exit(this.game);
            game.BigMap.Invoke(new DrawHandler(game.BigMap.Draw));
        }

        #endregion
    }
}
