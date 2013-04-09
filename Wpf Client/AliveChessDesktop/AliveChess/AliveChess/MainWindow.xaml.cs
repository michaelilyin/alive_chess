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

namespace AliveChess
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameCore _core;

        public MainWindow()
        {
            InitializeComponent();
            _core = GameCore.Instance;
        }

        private void OnGame(object sender, RoutedEventArgs e)
        {
            //MoveTo(new Uri("/GameLayer/PresentationLayer/MapScene.xaml", UriKind.Relative));
        }

        private void OnBattle(object sender, RoutedEventArgs e)
        {
            //MoveTo(new Uri("/GameLayer/PresentationLayer/BattleScene.xaml", UriKind.Relative));
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            //MoveTo(new Uri("/GameLayer/PresentationLayer/LogInScene.xaml", UriKind.Relative));
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {

        }

        //protected override void MoveTo(Uri uri)
        //{
        //    base.MoveTo(uri);
        //    if (NavigationService != null)
        //        NavigationService.Navigate(uri, UriKind.Relative);
        //}

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
