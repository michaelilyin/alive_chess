using System.Windows;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.Commands;

namespace WP7_Client.LogicLayer.Executors
{
    public class ErrorMessageExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            var msg = (ErrorMessage)command;
            Deployment.Current.Dispatcher.BeginInvoke(() => ((App) Application.Current).HandleError(msg.Message));
        }
    }
}
