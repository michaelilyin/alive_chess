using System.Windows;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;

namespace WP7_Client.LogicLayer.Executors
{
    public class GetMapExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            var response = (GetMapResponse)command;
            ((App)Application.Current).World.Create(response);
            Deployment.Current.Dispatcher.BeginInvoke(() => ((App) Application.Current).OnGetBigMap());
        }
    }   
}
