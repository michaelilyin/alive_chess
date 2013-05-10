using System;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors.BigMapExecutors
{
    public class MoveKingExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            MoveKingResponse response = (MoveKingResponse)command;
            
            //Не реализовано, нет необходимости, т.к. есть GetObjects

            GameCore.Instance.BigMapCommandController.DynamicObjectsModified = true;
        }
    }
}
