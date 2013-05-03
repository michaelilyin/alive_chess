using System;
using System.Windows.Threading;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class BigMapExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(AliveChessLibrary.Commands.ICommand command)
        {
            BigMapResponse response = (BigMapResponse)command;
            CastleScene castleScene = (CastleScene)GameCore.Instance.WindowContext.Find("SceneCastle", false);
#warning Переделать под контроллер
            castleScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<BigMapResponse>(castleScene.ShowBigMapResult),
                response);
        }

        #endregion
    }
}
