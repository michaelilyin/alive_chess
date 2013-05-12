using System.Data.Linq;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class GetGameStateExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            GetGameStateResponse response = (GetGameStateResponse)command;

            //GameCore.Instance.Player.AddKing(response.King);
            /*GameCore.Instance.Player.King.Castles = response.King.Castles;
            GameCore.Instance.Player.King.AttachStartCastle(response.Castle);
            GameCore.Instance.Player.King.X = response.King.;
            GameCore.Instance.Player.King.AttachStartCastle(response.Castle);*/
            if (GameCore.Instance.Player.King == null)
            {
                GameCore.Instance.Player.King = response.King;
            }
            else
            {
                lock (GameCore.Instance.Player.King)
                {
                    GameCore.Instance.Player.King.X = response.King.X;
                    GameCore.Instance.Player.King.Y = response.King.Y;
                    GameCore.Instance.Player.King.Experience = response.King.Experience;
                    GameCore.Instance.Player.King.MilitaryRank = response.King.MilitaryRank;
                }

            }
            if (GameCore.Instance.Player.King.ResourceStore == null)
                GameCore.Instance.Player.King.ResourceStore = new ResourceStore();
            GameCore.Instance.Player.King.ResourceStore.SetResources(response.Resources);

            /*MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);
            if (mapScene != null)
            {
                mapScene.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    new Action<GetGameStateResponse>(mapScene.ShowGetStateResult),
                    response);
            }*/
            AliveChessLibrary.DebugConsole.WriteLine(this, "Received");
            GameCore.Instance.BigMapCommandController.ResourcesModified = true;
        }
    }
}
