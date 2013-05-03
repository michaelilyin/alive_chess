using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChess.GameLayer.PresentationLayer
{
    /// <summary>
    /// Interaction logic for CastleScene.xaml
    /// </summary>
    public partial class CastleScene : GameScene
    {
        public CastleScene()
        {
            InitializeComponent();
            GameCore.Instance.CastleCommandController.CastleScene = this;
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(ConnectCallback), true);
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            BigMapRequest request = new BigMapRequest();
            GameCore.Instance.Network.Send(request);
        }

        public void ShowBigMapResult(BigMapResponse response)
        {
            Uri uri = new Uri("/GameLayer/PresentationLayer/MapScene.xaml", UriKind.Relative); 
           
            base.MoveTo(uri);
            if (response.IsAllowed)
            {
                NavigationService.Navigate(uri);
            }
            GameCore.Instance.CastleCommandController.CastleScene = null;
            GameCore.Instance.CastleCommandController.Castle = null;
        }

        public void ConnectCallback(bool isConnected)
        {
            //try
            //{
            //    NavigationService.RemoveBackEntry();
            //}
            //catch (Exception) { }
            //GetMapRequest request = new GetMapRequest();
            //GameCore.Instance.Network.Send(request);
            GameCore.Instance.CastleCommandController.SendGetListBuildingsInCastleRequest();
        }

        public void ShowGetListBuildingsInCastleResult()
        {
        }
    }
}
