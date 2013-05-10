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
            lock (GameCore.Instance.Player.King.CurrentCastle.InnerBuildings)
            {
                GameCore.Instance.Player.King.CurrentCastle.InnerBuildings = CustomConverter.ListToEntitySet(response.Buildings);
            }
            GameCore.Instance.CastleCommandController.BuildingsModified = true;
        }

        #endregion
    }
}
