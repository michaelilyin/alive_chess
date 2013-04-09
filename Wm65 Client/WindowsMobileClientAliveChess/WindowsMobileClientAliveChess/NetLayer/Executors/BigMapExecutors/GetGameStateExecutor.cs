using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands;
using WindowsMobileClientAliveChess.GameLayer;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    class GetGameStateExecutor : IExecutor
    {
        private Game game;

        public GetGameStateExecutor(Game game)
        {
            this.game = game;
        }

        public void Execute(ICommand cmd)
        {
            GetGameStateResponse state = (GetGameStateResponse)cmd;
            Monitor.Enter(this.game);
            game.Player.King = state.King;
            game.AddCastle(state.Castle);
            game.SetInitRes(state.StartResources);
            game.Player.King.Name = game.Player.Login;
            game.Ready = true;
            game.HandleReady();
            Monitor.Exit(this.game);
        }
    }
}
