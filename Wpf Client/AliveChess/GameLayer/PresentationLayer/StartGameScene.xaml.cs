using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.PresentationLayer
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StartGameScene : GameScene
    {
        public StartGameScene()
        {
            InitializeComponent();

            if(GameCore.Instance.Player.IsAuthorized)
            {
                AllowStartGame();
                label1.Content = "Welcome, player!";
            }
            else
            {
                label1.Content = "Welcome, guest!";
                ForbidStartGame();
            }
        }

        private void OnGame(object sender, RoutedEventArgs e)
        {
            
            MoveTo(new Uri("/GameLayer/PresentationLayer/MapScene.xaml", UriKind.Relative));
        }

        private void OnBattle(object sender, RoutedEventArgs e)
        {
            MoveTo(new Uri("/GameLayer/PresentationLayer/BattleScene.xaml", UriKind.Relative));
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            MoveTo(new Uri("/GameLayer/PresentationLayer/LogInScene.xaml", UriKind.Relative));
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            
        }

        protected override void MoveTo(Uri uri)
        {
            base.MoveTo(uri);
            if (NavigationService != null)
                NavigationService.Navigate(uri, UriKind.Relative);
        }

        private void AllowStartGame()
        {
            BtnGame.IsEnabled = true;
            BtnBattle.IsEnabled = true;
        }

        private void ForbidStartGame()
        {
            BtnGame.IsEnabled = false;
            BtnBattle.IsEnabled = false;
        }
    }
}
