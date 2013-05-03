using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class MoveKingExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            MoveKingResponse response = (MoveKingResponse)command;

            /*MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);

            mapScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<MoveKingResponse>(mapScene.ShowMoveKingResult),
                response);*/
            GameCore.Instance.BigMapCommandController.ReceiveMoveKingResponse(response);
        }
    }
}
