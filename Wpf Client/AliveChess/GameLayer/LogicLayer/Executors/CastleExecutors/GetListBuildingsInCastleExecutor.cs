using System;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors.CastleExecutors
{
    class GetListBuildingsInCastleExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            GetListBuildingsInCastleResponse response = (GetListBuildingsInCastleResponse)command;
            GameCore.Instance.CastleCommandController.ReceiveGetListBuildingsInCastleResponce(response);
        }

        #endregion
    }
}
