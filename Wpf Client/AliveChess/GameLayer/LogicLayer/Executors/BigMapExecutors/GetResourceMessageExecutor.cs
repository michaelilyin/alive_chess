using System;
using System.Windows.Threading;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    class GetResourceMessageExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(AliveChessLibrary.Commands.ICommand command)
        {
            GetResourceMessage message = (GetResourceMessage)command;
            //Не реализовано, нет необходимости, т.к. есть GetGameState
            GameCore.Instance.BigMapCommandController.ResourcesModified = true;
        }

        #endregion
    }
}
