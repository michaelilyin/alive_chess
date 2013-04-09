using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors
{
    public class GetObjectsExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            GetObjectsResponse response = (GetObjectsResponse)command;
            MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);

            mapScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<GetObjectsResponse>(mapScene.ShowGetObjectsResult),
                response);
        }

        #endregion
    }
}
