using System;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors.CastleExecutors
{
    class CreateUnitExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            CreateUnitResponse response = (CreateUnitResponse)command;
            GameCore.Instance.Player.King.CurrentCastle.RecruitingManager.SetProductionQueue(response.ProductionQueue);
            GameCore.Instance.CastleCommandController.RecruitingQueueModified = true;
        }

        #endregion
    }
}
