using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class UpdateWorldMessageExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            //throw new NotImplementedException();
            //HACK: не реализовано

            UpdateWorldMessage response = (UpdateWorldMessage)command;
#warning Не реализовано
            MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);

            /*mapScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<UpdateWorldMessage>(mapScene.ShowGetObjectsResult),
                response);*/
        }


        #endregion
    }
}
