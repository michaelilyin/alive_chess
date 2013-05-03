using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class GetMapExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            GetMapResponse response = (GetMapResponse)command;
#warning Создание карты
            GameCore.Instance.World.Create(response);/*
            MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);

            mapScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<GetMapResponse>(mapScene.ShowGetMapResult),
                response);*/
            GameCore.Instance.BigMapCommandController.ReceiveGetMapResponse(response);
        }
    }    
}
