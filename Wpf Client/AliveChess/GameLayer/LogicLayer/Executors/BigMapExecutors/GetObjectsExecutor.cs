using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class GetObjectsExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            GetObjectsResponse response = (GetObjectsResponse)command;

            GameCore.Instance.World.Map.Resources = CustomConverter.ListToEntitySet(response.Resources);
            GameCore.Instance.World.Map.Kings = CustomConverter.ListToEntitySet(response.Kings);
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
                    else
                    {
                        GameCore.Instance.World.Map.AddCastle(castle);
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
                    else
                    {
                        GameCore.Instance.World.Map.AddMine(mine);
                    }
                }
            }
            GameCore.Instance.BigMapCommandController.DynamicObjectsChanged = true;
            GameCore.Instance.BigMapCommandController.BuildingChanged = true;

            /*MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);
            mapScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<GetObjectsResponse>(mapScene.ShowGetObjectsResult),
                response);*/
            //GameCore.Instance.BigMapCommandController.ReceiveGetObjectsResponse(response);
        }

        #endregion
    }
}
