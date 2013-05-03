using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class ComeInCastleExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            ComeInCastleResponse response = (ComeInCastleResponse)command;
            /*MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);

            mapScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<ComeInCastleResponse>(mapScene.ShowComeInCastleResult),
                response);*/
            GameCore.Instance.CastleCommandController.Castle = GameCore.Instance.World.Map.SearchCastleById(response.CastleId);
            GameCore.Instance.BigMapCommandController.ReceiveComeInCastleResponse(response);
        }

        #endregion
    }
}
