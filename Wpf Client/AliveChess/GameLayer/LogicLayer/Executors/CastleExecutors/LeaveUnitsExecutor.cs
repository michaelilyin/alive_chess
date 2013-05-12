using System;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors.CastleExecutors
{
    class LeaveUnitsExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            LeaveUnitsResponse response = (LeaveUnitsResponse)command;
            GameCore.Instance.Player.King.CurrentCastle.Army.SetUnits(response.CastleArmy);
            GameCore.Instance.Player.King.Army.SetUnits(response.KingArmy);
            GameCore.Instance.CastleCommandController.UnitsModified = true;
        }

        #endregion
    }
}
