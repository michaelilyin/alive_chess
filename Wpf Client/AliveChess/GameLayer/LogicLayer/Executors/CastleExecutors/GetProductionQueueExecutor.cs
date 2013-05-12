using System;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors.CastleExecutors
{
    class GetProductionQueueExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            GetProductionQueueResponse response = (GetProductionQueueResponse)command;
            GameCore.Instance.Player.King.CurrentCastle.BuildingManager.SetProductionQueue(response.BuildingQueue);
            GameCore.Instance.CastleCommandController.BuildingQueueModified = true;
            GameCore.Instance.Player.King.CurrentCastle.RecruitingManager.SetProductionQueue(response.RecruitingQueue);
            GameCore.Instance.CastleCommandController.RecruitingQueueModified = true;
        }

        #endregion
    }
}
