using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors
{
    public class GameStateExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            GetGameStateResponse response = (GetGameStateResponse) command;
            
            GameCore.Instance.Player.AddKing(response.King);
            response.King.AttachStartCastle(response.Castle);
            response.King.Resources = response.Resources;

            //
        }
    }
}
