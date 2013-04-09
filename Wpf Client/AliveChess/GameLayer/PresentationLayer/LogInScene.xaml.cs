using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AliveChessLibrary.Commands.RegisterCommand;

namespace AliveChess.GameLayer.PresentationLayer
{
    /// <summary>
    /// Interaction logic for LogInScene.xaml
    /// </summary>
    public partial class LogInScene : GameScene
    {
        public LogInScene()
        {
            InitializeComponent();

            GameCore.Instance.Network.OnConnect += Network_OnConnect;
        }

        void Network_OnConnect()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(ConnectCallback), true);
        }

        public void ConnectCallback(bool isConnected)
        {
            BtnConnect.IsEnabled = false;
            BtnConnect.Content = "Connected";

            AuthorizeRequest request = new AuthorizeRequest();
            request.Login = textBox1.Text;
            request.Password = passwordBox1.Password;
            GameCore.Instance.Network.Send(request);
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            GameCore.Instance.ConnectToServer();
        }

        public void ShowAuthorizationResult(AuthorizeResponse response)
        {
            if (response.IsAuthorized)
            {
                if (NavigationService != null)
                    NavigationService.Navigate(new Uri("/GameLayer/PresentationLayer/StartGameScene.xaml",
                                                       UriKind.Relative));
            }
            else
            {
                label1.Content = response.ErrorMessage;
            }
        }
    }
}
