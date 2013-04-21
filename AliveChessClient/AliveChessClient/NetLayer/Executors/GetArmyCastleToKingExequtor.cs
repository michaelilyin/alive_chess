using System.Collections.Generic;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessClient.NetLayer.Executors
{
    class GetArmyCastleToKingExequtor:IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;

        public GetArmyCastleToKingExequtor(Game context)
        {
            this.context = context;
            this.resourceHandler = new UpdateResourceHandler(context.GameForm.CastleControl.getArmy);
        }

        public void Execute(ICommand cmd)
        {
            GetArmyCastleToKingResponse msg = (GetArmyCastleToKingResponse)cmd;
            

            if (context.GameForm.Created)   
            {
                    context.GameForm.CastleControl.Invoke(resourceHandler, msg.Arm, 2);
            }
        }

        public delegate void UpdateResourceHandler(IList<Unit> un, int i);
    }
}
