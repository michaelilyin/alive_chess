using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BattleCommand;
//using AliveChessLibrary.Entities.Characters;
//using AliveChessLibrary.Interfaces;

namespace AliveChessClient.NetLayer.Executors
{
    class PlayerMoveResponseExecutor:IExecutor
    {
        private Game context;
        private MOVE move;
        public PlayerMoveResponseExecutor(Game context)
        {
            this.context = context;
            this.move = new MOVE(context.GameForm.BattleControl.playerMove);
        }

        public void Execute(ICommand cmd)
        {
            PlayerMoveResponse msg = (PlayerMoveResponse)cmd;
            

            if (context.GameForm.Created)
            {
                context.GameForm.BattleControl.Invoke(move, msg.Move, msg.Countunit, msg.Step);        
                
            }
        }

        public delegate void MOVE(byte [] t, int c, bool step);
    }
}
