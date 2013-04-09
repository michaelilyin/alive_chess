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
    public class BigMapExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(AliveChessLibrary.Commands.ICommand command)
        {
            BigMapResponse response = (BigMapResponse)command;
            CastleScene castleScene = (CastleScene)GameCore.Instance.WindowContext.Find("SceneCastle", false);

            castleScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<BigMapResponse>(castleScene.ShowBigMapResult),
                response);
        }

        #endregion
    }
}
