using System;
using System.Windows.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChess.GameLayer.PresentationLayer;

namespace AliveChess.GameLayer.LogicLayer.Executors.CastleExecutors
{
    class GetKingArmyExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(ICommand command)
        {
            GetKingArmyResponse response = (GetKingArmyResponse)command;
            GameCore.Instance.Player.King.Army.SetUnits(response.Units);
            GameCore.Instance.CastleCommandController.UnitsModified = true;
        }

        #endregion
    }
}
