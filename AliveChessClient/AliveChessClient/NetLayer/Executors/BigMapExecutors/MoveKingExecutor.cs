using System.Collections.Generic;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class MoveKingExecutor : IExecutor
    {
        private Game context;

        public MoveKingExecutor(Game context)
        {
            this.context = context;
        }

        public void Execute(ICommand cmd)
        {
            MoveKingResponse move = (MoveKingResponse)cmd;
            if (move.Steps != null)
            {
                Queue<Position> steps = new Queue<Position>(move.Steps);
                context.Player.King.AddSteps(steps);
            }
            else context.Player.King.IsMove = false;
        }
    }
}
