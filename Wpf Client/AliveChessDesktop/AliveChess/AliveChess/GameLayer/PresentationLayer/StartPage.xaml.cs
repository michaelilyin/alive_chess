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

namespace AliveChess.GameLayer.PresentationLayer
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        private GameCore _core;

        public StartPage()
        {
            InitializeComponent();
            _core = GameCore.Instance;
            //_core = new GameCore(this);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            _core.ConnectToServer();
        }

        public void ConnectCallback(bool isConnected)
        {
            label1.Content = isConnected ? "Done" : "Fail";
            AuthorizeRequest request = new AuthorizeRequest();
            request.Login = "player";
            request.Password = "pw";
            _core.Network.Send(request);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GameLayer/PresentationLayer/BigMapPage.xaml", UriKind.Relative));
        }
    }
}
