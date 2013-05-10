using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class BigMapExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(AliveChessLibrary.Commands.ICommand command)
        {
            BigMapResponse response = (BigMapResponse)command;
            if (response.IsAllowed)
            {
                GameCore.Instance.BigMapCommandController.KingInCastle = false;
                /*GameCore.Instance.CastleCommandController.KingOnMap = true;*/
            }
        }

        #endregion
    }
}
