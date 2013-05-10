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
using AliveChessLibrary.GameObjects.Characters;
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
            this.ShowsNavigationUI = false;
            _castle = GameCore.Instance.Player.King.CurrentCastle;
            _castleCommandController = GameCore.Instance.CastleCommandController;
            _castleCommandController.SendGetCreationRequirementsRequest();
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

        private string _getNameByType(InnerBuildingType type)
        {
            switch (type)
            {
                case InnerBuildingType.Quarters:
                    return "Казармы";
                case InnerBuildingType.TrainingGround:
                    return "Тренировочная площадка";
                case InnerBuildingType.Stabling:
                    return "Конюшня";
                case InnerBuildingType.Workshop:
                    return "Мастерская";
                case InnerBuildingType.Fortress:
                    return "Крепость";
                case InnerBuildingType.Forge:
                    return "Кузница";
                case InnerBuildingType.Hospital:
                    return "Больница";
            }
            return type.ToString();
        }

        private string _getNameByType(ResourceTypes type)
        {
            switch (type)
            {
                case ResourceTypes.Gold:
                    return "Золото";
                case ResourceTypes.Stone:
                    return "Камень";
                case ResourceTypes.Wood:
                    return "Дерево";
                case ResourceTypes.Iron:
                    return "Железо";
                case ResourceTypes.Coal:
                    return "Уголь";
            }
            return type.ToString();
        }

        private string _requirementsToString(CreationRequirements requirements)
        {
            string result = "Требуется:";
            foreach (var item in requirements.Resources)
            {
                result += "\n" + _getNameByType(item.Key) + ": " + item.Value.ToString();
            }
            foreach (var building in requirements.RequiredBuildings)
            {
                result += "\n" + _getNameByType(building);
            }
            result += "\n Время постройки: " + requirements.CreationTime.ToString() + " сек.";
            return result;
        }

        private void _checkBuilding(InnerBuildingType type, Button btn)
        {
            if (_castle.HasBuilding(type))
            {
                btn.Content = "Снести";
                btn.ToolTip = null;
                btn.IsEnabled = true;
            }
            else if (_castle.BuildingManager.HasUnfinishedBuilding(type))
            {
                btn.Content = "Отменить постройку";
                btn.ToolTip = null;
                btn.IsEnabled = true;
            }
            else
            {
                btn.Content = "Построить";
                lock (GameCore.Instance.Player.King.CurrentCastle.BuildingManager.CreationRequirements)
                {
                    CreationRequirements requirements = _castle.BuildingManager.GetCreationRequirements(type);
                    if (requirements == null)
                        return;
                    btn.ToolTip = _requirementsToString(requirements);
                    if (requirements.RequiredBuildings.Any(building => !_castle.HasBuilding(building)))
                    {
                        btn.IsEnabled = false;
                        return;
                    }
                    btn.IsEnabled =
                        GameCore.Instance.Player.King.ResourceStore.HasEnoughResources(requirements.Resources);
                }
            }
        }

        public void UpdateBuildings()
        {
            lock (GameCore.Instance.Player.King.CurrentCastle.InnerBuildings)
            {
                lock (GameCore.Instance.Player.King.CurrentCastle.BuildingManager.BuildingQueue)
                {
                    _checkBuilding(InnerBuildingType.Quarters, BtnQuarters);
                    _checkBuilding(InnerBuildingType.TrainingGround, BtnTrainingGround);
                    _checkBuilding(InnerBuildingType.Stabling, BtnStabling);
                    _checkBuilding(InnerBuildingType.Workshop, BtnWorkshop);
                    _checkBuilding(InnerBuildingType.Fortress, BtnFortress);
                    _checkBuilding(InnerBuildingType.Forge, BtnForge);
                    _checkBuilding(InnerBuildingType.Hospital, BtnHospital);
                }
            }
            //StackPanelBuildings.Visibility = ListBoxBuildings.Items.Count > 0 ? Visibility.Visible : Visibility.Hidden;
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
            StackPanelBuildingQueue.Visibility = ListBoxBuildingQueue.Items.Count > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        public void UpdateUnits()
        {
        }

        private void _buildingBtnClick(InnerBuildingType type)
        {
            lock (_castle.InnerBuildings)
            {
                lock (_castle.BuildingManager.BuildingQueue)
                {
                    if (_castle.HasBuilding(type) ||
                        _castle.BuildingManager.HasUnfinishedBuilding(type))
                    {
                        _castleCommandController.SendDestroyBuildingRequest(type);
                        return;
                    }
                }
            }
            _castleCommandController.SendCreateBuildingRequest(type);
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendLeaveCastleRequest();
        }

        private void BtnQuarters_Click(object sender, RoutedEventArgs e)
        {
            _buildingBtnClick(InnerBuildingType.Quarters);
        }

        private void BtnTrainingGround_Click(object sender, RoutedEventArgs e)
        {
            _buildingBtnClick(InnerBuildingType.TrainingGround);
        }

        private void BtnStabling_Click(object sender, RoutedEventArgs e)
        {
            _buildingBtnClick(InnerBuildingType.Stabling);
        }

        private void BtnWorkshop_Click(object sender, RoutedEventArgs e)
        {
            _buildingBtnClick(InnerBuildingType.Workshop);
        }

        private void BtnRoyalGuardQuarters_Click(object sender, RoutedEventArgs e)
        {
            _buildingBtnClick(InnerBuildingType.Fortress);
        }

        private void BtnForge_Click(object sender, RoutedEventArgs e)
        {
            _buildingBtnClick(InnerBuildingType.Forge);
        }

        private void BtnHospital_Click(object sender, RoutedEventArgs e)
        {
            _buildingBtnClick(InnerBuildingType.Hospital);
        }
    }
}
