using System;
using System.Windows;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.RegisterCommand;

namespace WP7_Client.LogicLayer.Executors
{
    public class AuthorizeExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            var response = (AuthorizeResponse) command;
            ((App) Application.Current).World.Player = new Player {IsAuthorized = true};
            Deployment.Current.Dispatcher.BeginInvoke(() => ((App)Application.Current).HandleAuthorizeInvocation());
        }
    }
}
