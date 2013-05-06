using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Effects;
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
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChess.GameLayer.PresentationLayer
{
    /// <summary>
    /// Interaction logic for CastleScene.xaml
    /// </summary>
    public partial class CastleScene : GameScene
    {
        private Rectangle[,] rectArrGround;
        private Rectangle[,] rectArrBuildings;

        DropShadowEffect _enemyLighting = new DropShadowEffect();
        DropShadowEffect _playerLighting = new DropShadowEffect();
        DropShadowEffect _selectionLighting = new DropShadowEffect();

        private Castle _castle;

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

        public CastleScene()
        {
            InitializeComponent();
            _castle = GameCore.Instance.CastleCommandController.Castle;
            GameCore.Instance.CastleCommandController.CastleScene = this;
            GameCore.Instance.BigMapCommandController.CastleScene = this;
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
            GameCore.Instance.CastleCommandController.SendGetBuildingsRequest();
        }

        public void ShowGetGameStateResult()
        {
            foreach (var res in GameCore.Instance.Player.King.ResourceStore.Resources)
            {
                switch (res.ResourceType)
                {
                    case ResourceTypes.Gold:
                        LabelGoldQuantity.Content = res.Quantity.ToString();
                        break;
                    case ResourceTypes.Stone:
                        LabelStoneQuantity.Content = res.Quantity.ToString();
                        break;
                    case ResourceTypes.Wood:
                        LabelWoodQuantity.Content = res.Quantity.ToString();
                        break;
                    case ResourceTypes.Iron:
                        LabelIronQuantity.Content = res.Quantity.ToString();
                        break;
                    case ResourceTypes.Coal:
                        LabelCoalQuantity.Content = res.Quantity.ToString();
                        break;
                }
            }
        }

        public void ShowGetBuildingsResult()
        {
            ListBoxBuildings.Items.Clear();
            foreach (var building in _castle.InnerBuildings)
            {
                ListBoxBuildings.Items.Add(building.InnerBuildingType.ToString());
            }
        }

        private void btnBuild_Click(object sender, RoutedEventArgs e)
        {
            GameCore.Instance.CastleCommandController.SendCreateBuildingRequest(InnerBuildingType.Stabling);
        }
    }
}
