using System.Collections.Generic;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessClient.NetLayer.Executors
{
    class HireUnitExecutor:IExecutor
    {
        private Game context;
        private UpdateResourceHandler resourceHandler;

        public HireUnitExecutor(Game context)
        {
            this.context = context;
            this.resourceHandler = new UpdateResourceHandler(context.GameForm.CastleControl.getArmy);
        }

        public void Execute(ICommand cmd)
        {
            BuyFigureResponse msg = (BuyFigureResponse)cmd;
            

            if (context.GameForm.Created)
            {
                    context.GameForm.CastleControl.Invoke(resourceHandler, msg.Units, 1);
            }
        }

        public delegate void UpdateResourceHandler(IList<Unit> un, int i);

    }
}
