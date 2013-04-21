using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BattleCommand;
//using AliveChessLibrary.Entities.Characters;
//using AliveChessLibrary.Interfaces;

namespace AliveChessClient.NetLayer.Executors
{
    class MoveUnitResponseExecutor:IExecutor
    {
        private Game context;
        private MOVE move;
        public MoveUnitResponseExecutor(Game context)
        {
            this.context = context;
            this.move = new MOVE(context.GameForm.BattleControl.playerMove);
        }

        public void Execute(ICommand cmd)
        {
            MoveUnitResponse msg = (MoveUnitResponse)cmd;
            

            if (context.GameForm.Created)
            {
                context.GameForm.BattleControl.Invoke(move, msg.Move, msg.Countunit, msg.Step);        
                
            }
        }

        public delegate void MOVE(byte [] t, int c, bool step);
    }
}
