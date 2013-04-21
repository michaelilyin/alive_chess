using System.Collections.Generic;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class LooseCastleExecutor : IExecutor
    {
        private Game context;
        private CastlesHandler handler;

        public LooseCastleExecutor(Game context)
        {
            this.context = context;
            this.handler = new CastlesHandler(context.GameForm.BigMapControl.UpdateCastlesCount);
        }

        public void Execute(ICommand cmd)
        {
            LooseCastleMessage resp = cmd as LooseCastleMessage;
            context.Player.King.RemoveCastle(resp.CastleId);

            context.Player.King.RemoveCastle(resp.CastleId);
            
            //TODO: Проверить, не поломалось ли все
            /*if (!context.Player.IsSuperUser)
            {//
                VisibleSpace sector = context.VisibleSpaceManager.GetVisibleSpace(context.Player.King,
                    context.Player.IsSuperUser);
                context.Player.UpdateVisibleSpace(sector);
                List<MapPoint> array = sector.Sector;
                foreach (MapPoint mo in array)
                    mo.Detected = true;
            }*/

            context.GameForm.Invoke(handler, context.Player.King.Castles.Count);
        }

        public delegate void CastlesHandler(int count);
    }
}
