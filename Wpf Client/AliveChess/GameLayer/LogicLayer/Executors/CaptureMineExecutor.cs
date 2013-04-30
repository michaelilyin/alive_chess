using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors
{
    public class CaptureMineRequestExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
//TODO: При необходимости реализовать обработчик
            CaptureMineResponse response = (CaptureMineResponse) command;
            GameCore.Instance.World.Map.SearchMineById(response.Mine.Id).KingId = response.Mine.KingId;
        }
    }
}
