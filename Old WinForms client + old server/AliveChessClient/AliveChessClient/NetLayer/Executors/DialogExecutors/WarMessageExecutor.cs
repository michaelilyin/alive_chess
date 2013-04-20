using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.DialogCommand;

namespace AliveChessClient.NetLayer.Executors.DialogExecutors
{
    public class WarMessageExecutor : IExecutor
    {
        private Game context;
        private AliveChessDelegateWithText handler;

        public WarMessageExecutor(Game context)
        {
            this.context = context;
            handler = delegate(string text)
            {
                context.BigMap.SetText(text);
            };
        }

        public void Execute(ICommand cmd)
        {
            WarDialogMessage msg = (WarDialogMessage)cmd;
            context.BigMap.Invoke(handler, "War is started");
        }
    }
}
