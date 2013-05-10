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
using AliveChess.GameLayer.LogicLayer;
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

        private CastleCommandController _castleCommandController;

        DispatcherTimer timerUpdate = new DispatcherTimer();

        private Castle _castle;

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

        public CastleScene()
        {
            InitializeComponent();
            _castle = GameCore.Instance.Player.King.CurrentCastle;
            _castleCommandController = GameCore.Instance.CastleCommandController;
            _castleCommandController.SendGetBuildingsRequest();
            /*Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(ConnectCallback), true);*/
            timerUpdate.Tick += new EventHandler(timerUpdate_Tick);
            timerUpdate.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timerUpdate.Start();
            _castleCommandController.StartUpdate();
            GameCore.Instance.BigMapCommandController.StartGameStateUpdate();
        }


        void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (_castleCommandController.KingOnMap)
            {
                ExitCastle();
                return;
            }
            if (GameCore.Instance.BigMapCommandController.ResourcesModified)
            {
                UpdateResources();
                GameCore.Instance.BigMapCommandController.ResourcesModified = false;
            }
            if (_castleCommandController.BuildingsModified)
            {
                UpdateBuildings();
                _castleCommandController.BuildingsModified = false;
            }
            if (_castleCommandController.UnitsModified)
            {
                UpdateUnits();
                _castleCommandController.UnitsModified = false;
            }
            if (_castleCommandController.BuildingQueueModified)
            {
                UpdateBuildingQueue();
                _castleCommandController.BuildingQueueModified = false;
            }
        }

        public void ExitCastle()
        {
            timerUpdate.Stop();
            GameCore.Instance.BigMapCommandController.StopGameStateUpdate();
            _castleCommandController.StopUpdate();
            Uri uri = new Uri("/GameLayer/PresentationLayer/MapScene.xaml", UriKind.Relative);
            base.MoveTo(uri);
            if ((NavigationService != null))
                NavigationService.Navigate(uri);
        }

        //public void ConnectCallback(bool isConnected)
        //{
        //    //try
        //    //{
        //    //    NavigationService.RemoveBackEntry();
        //    //}
        //    //catch (Exception) { }
        //    //GetMapRequest request = new GetMapRequest();
        //    //GameCore.Instance.Network.Send(request);
        //    GameCore.Instance.CastleCommandController.SendGetBuildingsRequest();
        //}

        public void UpdateResources()
        {
            lock (GameCore.Instance.Player.King.ResourceStore)
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
        }

        public void UpdateBuildings()
        {
            ListBoxBuildings.Items.Clear();
            lock (GameCore.Instance.Player.King.CurrentCastle.InnerBuildings)
            {
                foreach (var building in _castle.InnerBuildings)
                {
                    ListBoxBuildings.Items.Add(building.InnerBuildingType.ToString());
                }
            }
        }

        public void UpdateBuildingQueue()
        {
            ListBoxBuildingQueue.Items.Clear();
            lock (GameCore.Instance.Player.King.CurrentCastle.BuildingManager.BuildingQueue)
            {
                foreach (var item in _castle.BuildingManager.BuildingQueue)
                {
                    ListBoxBuildingQueue.Items.Add(item.Type.ToString() + " (" + (int)item.TimeToPercent() + "%)");
                }
            }
        }

        public void UpdateUnits()
        {
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendLeaveCastleRequest();
        }

        private void btnBuild_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCreateBuildingRequest(InnerBuildingType.Stabling);
        }

        private void btnDestroy_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendDestroyBuildingRequest(InnerBuildingType.Stabling);
        }
    }
}
