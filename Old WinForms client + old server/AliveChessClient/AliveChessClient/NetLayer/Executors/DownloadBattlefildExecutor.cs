using System.Collections.Generic;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BattleCommand;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessClient.NetLayer.Executors
{
    class DownloadBattlefildExecutor : IExecutor
    {
        private Game context;
        private Battlefild Bf;
        private Batlecont bat;
        public DownloadBattlefildExecutor(Game context)
        {
            this.context = context;
            this.bat = new Batlecont(context.GameForm.StartBatle);
            this.Bf = new Battlefild(context.GameForm.BattleControl.qwer);
        }

        public void Execute(ICommand cmd)
        {
            DownloadBattlefildResponse msg = (DownloadBattlefildResponse)cmd;


            if (context.GameForm.Created)
            {
                //context.Battle = new AliveChessLibrary.GameObjects.Abstract.Battle();
                //context.Battle.Id = msg.IdBattle;
                //context.Battle.OppId = msg.IdOpponent;

                context.GameForm.BattleControl.Invoke(bat);
                context.Battle = msg.Battle;
                context.GameForm.BattleControl.Invoke(Bf, context.Battle.PlayerArmy,
                    context.Battle.OpponentArmy, false);
            }
        }

        public delegate void Battlefild(IList<Unit> arm, IList<Unit> oppArm, bool course);
        public delegate void Batlecont();
    }
}
