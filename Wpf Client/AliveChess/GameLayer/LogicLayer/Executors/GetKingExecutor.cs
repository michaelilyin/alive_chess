using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors
{
    class GetKingExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(AliveChessLibrary.Commands.ICommand command)
        {
            GetKingResponse response = (GetKingResponse)command;
            GameCore.Instance.Player.AddKing(response.King);

            MapScene mapScene = (MapScene)GameCore.Instance.WindowContext.Find("SceneMap", false);

            mapScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<GetKingResponse>(mapScene.ShowGetKingResult),
                response);
        }

        #endregion
    }
}
