using System.Windows.Forms;
using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;

namespace AliveChessClient.NetLayer.Executors.DialogExecutors
{
    public class PeaceMessageExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegateWithText handler;

        public PeaceMessageExecutor(Game context)
        {
            this.context = context;
            handler = delegate(string text)
            {
                MessageBox.Show(text);
            };
        }

        public void Execute(ICommand cmd)
        {
            PeaceDialogMessage msg = (PeaceDialogMessage)cmd;
            context.GameForm.Invoke(handler, "Peace is embeded");
        }
    }
}
