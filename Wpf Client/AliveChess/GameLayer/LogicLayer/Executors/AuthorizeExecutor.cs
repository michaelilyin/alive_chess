using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.RegisterCommand;

namespace AliveChess.GameLayer.LogicLayer.Executors
{
    public class AuthorizeExecutor : IExecutor
    {
        public void Execute(ICommand command)
        {
            AuthorizeResponse response = (AuthorizeResponse) command;
            LogInScene logInScene = (LogInScene)GameCore.Instance.WindowContext.Find("SceneLogIn", false);

            GameCore.Instance.Player.IsAuthorized = true;

            logInScene.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action<AuthorizeResponse>(logInScene.ShowAuthorizationResult), 
                response);
        }
    }
}
