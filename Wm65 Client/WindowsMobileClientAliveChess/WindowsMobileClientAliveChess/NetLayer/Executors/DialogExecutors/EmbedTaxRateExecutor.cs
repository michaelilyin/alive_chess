using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.EmpireCommand;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.EmpireExecutors
{
    public class EmbedTaxRateExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegateWithText handler;

        public EmbedTaxRateExecutor(Game context)
        {

        }

        public void Execute(ICommand cmd)
        {
            EmbedTaxRateResponse resp = (EmbedTaxRateResponse)cmd;
            context.GameForm.Invoke(handler, "Tax was changed");
        }
    }
}
