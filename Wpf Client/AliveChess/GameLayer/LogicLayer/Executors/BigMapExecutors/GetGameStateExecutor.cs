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
            GetGameStateResponse response = (GetGameStateResponse) command;
            
            //GameCore.Instance.Player.AddKing(response.King);
            /*GameCore.Instance.Player.King.Castles = response.King.Castles;
            GameCore.Instance.Player.King.AttachStartCastle(response.Castle);
            GameCore.Instance.Player.King.X = response.King.;
            GameCore.Instance.Player.King.AttachStartCastle(response.Castle);*/
            GameCore.Instance.Player.King = response.King;
            GameCore.Instance.Player.King.ResourceStore = new ResourceStore();
            EntitySet<Resource> esr = CustomConverter.ListToEntitySet(response.Resources);
            GameCore.Instance.Player.King.ResourceStore.Resources = esr;

            /*MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);
            if (mapScene != null)
            {
                mapScene.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    new Action<GetGameStateResponse>(mapScene.ShowGetStateResult),
                    response);
            }*/
            GameCore.Instance.BigMapCommandController.ReceiveGetGameStateResponse(response);
        }
    }
}
