using System;
using System.Windows.Threading;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.CastleCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.CastleExecutors
{
    public class LeaveCastleExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(AliveChessLibrary.Commands.ICommand command)
        {
            LeaveCastleResponse response = (LeaveCastleResponse)command;
            GameCore.Instance.CastleCommandController.KingOnMap = true;
            GameCore.Instance.BigMapCommandController.KingInCastle = false;
        }

        #endregion
    }
}
