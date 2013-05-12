using System;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors.CastleExecutors
{
    class DestroyBuildingExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            DestroyBuildingResponse response = (DestroyBuildingResponse)command;
            GameCore.Instance.Player.King.CurrentCastle.SetBuildings(response.Buildings);
            GameCore.Instance.CastleCommandController.BuildingsModified = true;
        }

        #endregion
    }
}
