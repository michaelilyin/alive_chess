using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using AliveChessLibrary.Commands;
using WindowsMobileClientAliveChess.GameLayer;

namespace WindowsMobileClientAliveChess.NetLayer.Executors
{
    class ErrorMessageExecutor :IExecutor
    {
        private Game game;

        public ErrorMessageExecutor(Game game)
        {
            this.game = game;
        }

        public void Execute(ICommand cmd)
        {
            CrazyMessage error = (CrazyMessage)cmd;
            throw new AliveChessLibrary.AliveChessException(error.Message);
        }
    }
}
