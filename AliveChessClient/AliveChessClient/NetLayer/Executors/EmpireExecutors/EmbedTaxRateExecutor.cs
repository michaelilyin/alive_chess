using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Commands.EmpireCommand;

namespace AliveChessClient.NetLayer.Executors.EmpireExecutors
{
    public class EmbedTaxRateExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegateWithText handler;

        public EmbedTaxRateExecutor(Game context)
        {
            this.context = context;
            this.handler = delegate(string text)
            {
                context.GameForm.LeaderControl.TaxLabel.Text = text;
            };
        }

        public void Execute(ICommand cmd)
        {
            EmbedTaxRateResponse resp = (EmbedTaxRateResponse)cmd;
            context.GameForm.Invoke(handler, "Tax was changed");
        }
    }
}
