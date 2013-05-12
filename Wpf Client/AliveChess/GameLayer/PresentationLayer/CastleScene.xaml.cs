using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
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
using AliveChess.Utilities;
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
            if (_castleCommandController.RecruitingQueueModified)
            {
                UpdateRecrutingQueue();
                _castleCommandController.RecruitingQueueModified = false;
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
            foreach (var res in GameCore.Instance.Player.King.ResourceStore.GetResourceListCopy())
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

        private string _requirementsToString(CreationRequirements requirements)
        {
            string result = "Требуется:";
            foreach (var item in requirements.Resources)
            {
                result += "\n" + NameResolver.Instance.GetNameByType(item.Key) + ": " + item.Value.ToString();
            }
            foreach (var building in requirements.RequiredBuildings)
            {
                result += "\n" + NameResolver.Instance.GetNameByType(building);
            }
            result += "\n Время постройки: " + requirements.CreationTime.ToString(CultureInfo.CurrentCulture) + " сек.";
            return result;
        }

        private void _setBuildingControl(InnerBuildingType type, Button btn)
        {
            if (_castle.HasBuilding(type))
            {
                btn.Content = "Снести";
                btn.ToolTip = null;
                btn.IsEnabled = true;
                return;
            }
            if (_castle.BuildingManager.HasInQueue(type))
            {
                btn.Content = "Отменить";
                btn.ToolTip = null;
                btn.IsEnabled = true;
                return;
            }
            btn.Content = "Построить";
            CreationRequirements requirements = _castle.BuildingManager.GetCreationRequirements(type);

            if (requirements == null)
                return;
            btn.ToolTip = _requirementsToString(requirements);

            if (requirements.RequiredBuildings.Any(building => !_castle.HasBuilding(building)))
            {
                btn.IsEnabled = false;
                return;
            }
            btn.IsEnabled = GameCore.Instance.Player.King.ResourceStore.HasEnoughResources(requirements.Resources);
        }

        private void _setUnitControl(UnitType type, Button btnAdd, Button btnCancel, Button btnToKing, Button btnToCastle, Label lblCastleQuantity, Label lblKingQuantity)
        {
            lblCastleQuantity.Content = _castle.Army.GetUnitQuantity(type);
            if (_castle.Army.HasUnits(type))
            {
                btnToKing.IsEnabled = true;
                btnToKing.ToolTip = null;
            }
            else
            {
                btnToKing.IsEnabled = false;
                btnToKing.ToolTip = "В замке нет юнитов этого типа.";
            }
            if (GameCore.Instance.Player.King.CurrentCastle.KingInside)
            {
                lblKingQuantity.Content = GameCore.Instance.Player.King.Army.GetUnitQuantity(type);
                if (GameCore.Instance.Player.King.Army.HasUnits(type))
                {
                    btnToCastle.IsEnabled = true;
                    btnToCastle.ToolTip = null;
                }
                else
                {
                    btnToCastle.IsEnabled = false;
                    btnToCastle.ToolTip = "В армии короля нет юнитов этого типа.";
                }
            }
            else
            {
                btnToCastle.IsEnabled = false;
                btnToKing.IsEnabled = false;
                lblKingQuantity.Content = "0";
                btnToCastle.ToolTip = "Король не в замке.";
                btnToKing.ToolTip = "Король не в замке.";
            }
            if (GameCore.Instance.Player.King.CurrentCastle.RecruitingManager.HasInQueue(type))
            {
                btnCancel.IsEnabled = true;
                btnCancel.ToolTip = "Отменить найм";
            }
            else
            {
                btnCancel.IsEnabled = false;
                btnCancel.ToolTip = "Этот юнит не ожидает найма.";
            }

            CreationRequirements requirements = _castle.RecruitingManager.GetCreationRequirements(type);
            if (requirements == null)
                return;

            btnAdd.ToolTip = _requirementsToString(requirements);
            btnAdd.IsEnabled = GameCore.Instance.Player.King.ResourceStore.HasEnoughResources(requirements.Resources);
        }

        public void UpdateBuildings()
        {
            _setBuildingControl(InnerBuildingType.Quarters, BtnQuarters);
            _setBuildingControl(InnerBuildingType.TrainingGround, BtnTrainingGround);
            _setBuildingControl(InnerBuildingType.Stabling, BtnStabling);
            _setBuildingControl(InnerBuildingType.Workshop, BtnWorkshop);
            _setBuildingControl(InnerBuildingType.Fortress, BtnFortress);
            _setBuildingControl(InnerBuildingType.Forge, BtnForge);
            _setBuildingControl(InnerBuildingType.Hospital, BtnHospital);
            //StackPanelBuildings.Visibility = ListBoxBuildings.Items.Count > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        public void UpdateBuildingQueue()
        {
            ListBoxBuildingQueue.Items.Clear();
            foreach (var item in _castle.BuildingManager.GetProductionQueueCopy())
            {
                ListBoxBuildingQueue.Items.Add(NameResolver.Instance.GetNameByType(item.Type) + " (" + (int)item.TimeToPercent() + "%)");
            }
            //StackPanelBuildingQueue.Visibility = ListBoxBuildingQueue.Items.Count > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        public void UpdateUnits()
        {
            _setUnitControl(UnitType.Pawn, BtnCreatePawn, BtnCancelPawn, BtnPawnToKing, BtnPawnToCastle, LblCastlePawnQuantity, LblKingPawnQuantity);
            _setUnitControl(UnitType.Bishop, BtnCreateBishop, BtnCancelBishop, BtnBishopToKing, BtnBishopToCastle, LblCastleBishopQuantity, LblKingBishopQuantity);
            _setUnitControl(UnitType.Knight, BtnCreateKnight, BtnCancelKnight, BtnKnightToKing, BtnKnightToCastle, LblCastleKnightQuantity, LblKingKnightQuantity);
            _setUnitControl(UnitType.Rook, BtnCreateRook, BtnCancelRook, BtnRookToKing, BtnRookToCastle, LblCastleRookQuantity, LblKingRookQuantity);
            _setUnitControl(UnitType.Queen, BtnCreateQueen, BtnCancelQueen, BtnQueenToKing, BtnQueenToCastle, LblCastleQueenQuantity, LblKingQueenQuantity);
        }

        public void UpdateRecrutingQueue()
        {
            ListBoxRecruitingQueue.Items.Clear();
            foreach (var item in _castle.RecruitingManager.GetProductionQueueCopy())
            {
                ListBoxRecruitingQueue.Items.Add(NameResolver.Instance.GetNameByType(item.Type) + " (" + (int)item.TimeToPercent() + "%)");
            }
        }

        private void _buildingBtnClick(InnerBuildingType type)
        {
            if (_castle.HasBuilding(type) ||
                _castle.BuildingManager.HasInQueue(type))
            {
                _castleCommandController.SendDestroyBuildingRequest(type);
                return;
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

        private void BtnFortress_Click(object sender, RoutedEventArgs e)
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

        private void BtnCreatePawn_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCreateUnitRequest(UnitType.Pawn);
        }

        private void BtnCancelPawn_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCancelUnitRecruitingRequest(UnitType.Pawn);
        }

        private void BtnCreateBishop_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCreateUnitRequest(UnitType.Bishop);
        }

        private void BtnCancelBishop_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCancelUnitRecruitingRequest(UnitType.Bishop);
        }

        private void BtnCreateKnight_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCreateUnitRequest(UnitType.Knight);
        }

        private void BtnCancelKnight_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCancelUnitRecruitingRequest(UnitType.Knight);
        }

        private void BtnCreateRook_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCreateUnitRequest(UnitType.Rook);
        }

        private void BtnCancelRook_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCancelUnitRecruitingRequest(UnitType.Rook);
        }

        private void BtnCreateQueen_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCreateUnitRequest(UnitType.Queen);
        }

        private void BtnCancelQueen_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCancelUnitRecruitingRequest(UnitType.Queen);
        }

        private Dictionary<UnitType, int> _createUnitDictionary(UnitType type)
        {
            Dictionary<UnitType, int> result = new Dictionary<UnitType, int>();
            result[type] = 1;
            return result;
        }

        private void BtnPawnToKing_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCollectUnitsRequest(_createUnitDictionary(UnitType.Pawn));
        }

        private void BtnPawnToCastle_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendLeaveUnitsRequest(_createUnitDictionary(UnitType.Pawn));
        }

        private void BtnBishopToKing_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCollectUnitsRequest(_createUnitDictionary(UnitType.Bishop));
        }

        private void BtnBishopToCastle_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendLeaveUnitsRequest(_createUnitDictionary(UnitType.Bishop));
        }

        private void BtnKnightToKing_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCollectUnitsRequest(_createUnitDictionary(UnitType.Knight));
        }

        private void BtnKnightToCastle_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendLeaveUnitsRequest(_createUnitDictionary(UnitType.Knight));
        }

        private void BtnRookToKing_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCollectUnitsRequest(_createUnitDictionary(UnitType.Rook));
        }

        private void BtnRookToCastle_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendLeaveUnitsRequest(_createUnitDictionary(UnitType.Rook));
        }

        private void BtnQueenToKing_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendCollectUnitsRequest(_createUnitDictionary(UnitType.Queen));
        }

        private void BtnQueenToCastle_Click(object sender, RoutedEventArgs e)
        {
            _castleCommandController.SendLeaveUnitsRequest(_createUnitDictionary(UnitType.Queen));
        }
    }
}
