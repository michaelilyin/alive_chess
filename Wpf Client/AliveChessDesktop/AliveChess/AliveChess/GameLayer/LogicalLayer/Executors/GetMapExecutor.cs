using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors
{
    public class GetMapExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            GetMapResponse response = (GetMapResponse) command;
            GameCore.Instance.World.Create(response);

            //
        }
    }
}
