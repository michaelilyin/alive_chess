using System;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors.CastleExecutors
{
    class GetCastleArmyExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            GetCastleArmyResponse response = (GetCastleArmyResponse)command;
            GameCore.Instance.Player.King.CurrentCastle.Army.SetUnits(response.Units);
            GameCore.Instance.CastleCommandController.UnitsModified = true;
        }

        #endregion
    }
}
