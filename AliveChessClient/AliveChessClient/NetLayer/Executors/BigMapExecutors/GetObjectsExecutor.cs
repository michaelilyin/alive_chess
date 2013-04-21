using System.Collections.Generic;
using System.Diagnostics;
using AliveChessClient.GameLayer;
using AliveChessClient.GameLayer.AliveChessGraphics;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class GetObjectsExecutor : IExecutor
    {
        private Game game;
        private DrawHandler handler;

        public GetObjectsExecutor(Game game)
        {
            this.game = game;
            this.handler = new DrawHandler(game.BigMap.GraphicManager.Draw);
        }

        #region IExecutor Members

        public void Execute(ICommand cmd)
        {
            GetObjectsResponse resp = (GetObjectsResponse)cmd;

            game.BigMap.GraphicManager.CalculateBounds();

            if (!game.Player.IsSuperUser)
            {//TODO: Проверить
                VisibleSpace sector = game.VisibleSpaceManager.GetVisibleSpace(game.Player.King,
                    game.Player.IsSuperUser);

                game.Player.UpdateVisibleSpace(sector);
                List<MapPoint> array = sector.Sector;
                foreach (MapPoint mo in array)
                    mo.Detected = true;
            }

            if (!game.Player.IsSuperUser)
                game.RefreshMap();

            if (resp.Objects!=null)
            {
                foreach (MapPoint mo in resp.Objects)
                {
                    if (!game.ContainsEntity(mo))
                    {
                        //game.PassEntityToModel(mo);
                        mo.Detected = true;

                        if (game.Map[mo.X, mo.Y] == null)
                            Debugger.Break();

                        mo.ObjectUnderThis = game.Map[mo.X, mo.Y];

                        if (mo.ObjectUnderThis == null)
                            Debugger.Break();

                        game.Map.SetObject(mo);
                    }
                }
            }

            game.GameForm.Invoke(handler);
        }

        #endregion
    }
}
