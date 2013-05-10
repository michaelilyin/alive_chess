using System;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    class GetKingExecutor : IExecutor
    {
        #region IExecutor Members

        public void Execute(AliveChessLibrary.Commands.ICommand command)
        {
            GetKingResponse response = (GetKingResponse)command;
            /*GameCore.Instance.Player.AddKing(response.King);
            GameCore.Instance.BigMapCommandController.DynamicObjectsChanged = true;*/
            throw new NotImplementedException();
        }

        #endregion
    }
}
