using System.Collections.Generic;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;
//using AliveChessLibrary.Interfaces;

namespace AliveChessClient.NetLayer.Executors
{
    class ShowArmyKingExecutor : IExecutor
    {
        private Game context;
        private Show show;

        public ShowArmyKingExecutor(Game cont)
        {
            this.context = cont;
            this.show = new Show(context.GameForm.CastleControl.ShowArmC);
        }

        public void Execute(ICommand cmd)
        {
            GetArmyCastleToKingResponse resp = (GetArmyCastleToKingResponse)cmd;
            if (context.GameForm.Created)
            {
                context.GameForm.BattleControl.Invoke(show, resp.Arm);

            }

        }

        public delegate void Show(List<Unit> units);
    }
}
