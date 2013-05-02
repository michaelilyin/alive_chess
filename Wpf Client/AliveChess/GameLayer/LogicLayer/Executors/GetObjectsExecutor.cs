using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChess.GameLayer.LogicLayer.Executors
{
    public class GetObjectsExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            GetObjectsResponse response = (GetObjectsResponse)command;
            var resources = new EntitySet<Resource>();
            if(response.Resources != null)
                resources.AddRange(response.Resources);
            GameCore.Instance.World.Map.Resources = resources;
            var kings = new EntitySet<King>();
            if(response.Kings != null)
                kings.AddRange(response.Kings);
            GameCore.Instance.World.Map.Kings = kings;
            GameCore.Instance.World.Map.Kings.Add(GameCore.Instance.Player.King);

            if (response.Castles != null)
            {
                foreach (var castle in response.Castles)
                {
                    var oldCastle = GameCore.Instance.World.Map.SearchCastleById(castle.Id);
                    if (oldCastle != null)
                    {
                        oldCastle.KingId = castle.KingId;
                    }
                }
            }

            if (response.Mines != null)
            {
                foreach (var mine in response.Mines)
                {
                    var oldMine = GameCore.Instance.World.Map.SearchMineById(mine.Id);
                    if (oldMine != null)
                    {
                        oldMine.KingId = mine.KingId;
                    }
                }
            }

            /*MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);
            mapScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<GetObjectsResponse>(mapScene.ShowGetObjectsResult),
                response);*/
            GameCore.Instance.BigMapRequestSender.ReceiveGetObjectsResponse(response);
        }

        #endregion
    }
}
