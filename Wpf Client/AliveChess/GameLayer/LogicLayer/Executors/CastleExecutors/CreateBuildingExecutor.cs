using System;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors.CastleExecutors
{
    class CreateBuildingExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            CreateBuildingResponse response = (CreateBuildingResponse)command;
            GameCore.Instance.CastleCommandController.ReceiveCreateBuildingResponce(response);
        }

        #endregion
    }
}
