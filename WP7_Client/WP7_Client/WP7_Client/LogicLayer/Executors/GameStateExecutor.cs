using System.Windows;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using WP7_Client.PresentationLayer.ConcreteScreens;

namespace WP7_Client.LogicLayer.Executors
{
    public class GameStateExecutor : IExecutor
    {
        private readonly BigMapScreen _screen;

        public  GameStateExecutor(BigMapScreen screen)
        {
            _screen = screen;
        }

        public void Execute(ICommand command)
        {
            var response = (GetGameStateResponse) command;
            response.King.AttachStartCastle(response.Castle);
            response.King.Resources = response.Resources;
            Deployment.Current.Dispatcher.BeginInvoke(() => _screen.HandleGetGameState(response));

        }
    }
}
