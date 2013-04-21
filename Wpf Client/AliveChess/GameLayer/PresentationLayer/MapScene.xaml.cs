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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AliveChess.GameLayer.LogicLayer;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;


namespace AliveChess.GameLayer.PresentationLayer
{
    /// <summary>
    /// Interaction logic for MapScene.xaml
    /// </summary>
    public partial class MapScene : GameScene
    {
        private GameWorld _world = GameCore.Instance.World;
        private Player _player = GameCore.Instance.Player;

        private Rectangle[,] rectArrLandscape = new Rectangle[50, 50];
        private Rectangle[,] rectArrLandscapeObjects = new Rectangle[50, 50];
        private Rectangle[,] rectArrGameObjects = new Rectangle[50, 50];

        private Point _kingPosition;
        private Point _castlePosition;

        private const int width = 100;
        private const int height = 100;
        private bool _rectIsInit = false;

        private bool _kingIsFocused = false;
        private bool _kingInCastle = false;

        DispatcherTimer timerMove = new DispatcherTimer();
        private Point _nextStep;
        private List<Position> _steps;
        private int _stepCount;


        private ImageBrush _brushKing;
        private ImageBrush _brushCastle;
        private ImageBrush[] _landscapeBrushes = new ImageBrush[4];
        private ImageBrush[] _singleObjectBrushes = new ImageBrush[2];
        private ImageBrush[] _mineBrushes = new ImageBrush[5];
        private ImageBrush[] _resourceBrushes = new ImageBrush[5];

        public MapScene()
        {
            //if (!rectIsInit)
            //{
            InitializeComponent();
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(ConnectCallback), true);
            ResponceComplete.responceComplete += new ResponceCompleteDelegate(ResponceComplete_responceComplete);
            timerMove.Tick += new EventHandler(timerMove_Tick);
            timerMove.Interval = new TimeSpan(0, 0, 0, 0, 200);

            InitBrushes();
            InitRectangles();
            //}
            //MyEvent += new MyDelegate(MapScene_MyEvent);
        }

        void timerMove_Tick(object sender, EventArgs e)
        {
            timerMove.Stop();

            if (_nextStep != null)
            {
                DoMove(new Position((int)(_nextStep.X), (int)(_nextStep.Y)));
                _stepCount--;
                if (_stepCount > 0)
                {
                    _nextStep = new Point(_steps[_steps.Count - _stepCount].X, _steps[_steps.Count - _stepCount].Y);
                    timerMove.Start();
                }
                else
                {
                    _steps = null;
                    _stepCount = 0;
                }
            }
        }

        void MapScene_MyEvent()
        {
            //canvas1_MouseLeftButtonDown(this, new MouseButtonEventArgs(Mouse.PrimaryDevice, 1, MouseButton.Left));
        }
        private delegate void MyDelegate();
        private event MyDelegate MyEvent;

        void ResponceComplete_responceComplete()
        {
            if (MyEvent != null)
                MyEvent();
        }

        public void InitBrushes()
        {
            BitmapImage bmKing = new BitmapImage(new Uri(@"Resources\king.png", UriKind.RelativeOrAbsolute));
            _brushKing = new ImageBrush(bmKing);

            BitmapImage bmCastle = new BitmapImage(new Uri(@"Resources\castle.png", UriKind.RelativeOrAbsolute));
            _brushCastle = new ImageBrush(bmCastle);

            BitmapImage bmGrass = new BitmapImage(new Uri(@"Resources\grass.png", UriKind.RelativeOrAbsolute));
            BitmapImage bmSnow = new BitmapImage(new Uri(@"Resources\snow.png", UriKind.RelativeOrAbsolute));
            BitmapImage bmGround = new BitmapImage(new Uri(@"Resources\ground.png", UriKind.RelativeOrAbsolute));
            BitmapImage bmNone = new BitmapImage(new Uri(@"Resources\none.png", UriKind.RelativeOrAbsolute));
            _landscapeBrushes[(int)AliveChessLibrary.GameObjects.Landscapes.LandscapeTypes.Grass] = new ImageBrush(bmGrass);
            _landscapeBrushes[(int)AliveChessLibrary.GameObjects.Landscapes.LandscapeTypes.Snow] = new ImageBrush(bmSnow);
            _landscapeBrushes[(int)AliveChessLibrary.GameObjects.Landscapes.LandscapeTypes.Ground] = new ImageBrush(bmGround);
            _landscapeBrushes[(int)AliveChessLibrary.GameObjects.Landscapes.LandscapeTypes.None] = new ImageBrush(bmNone);

            //Одиночное дерево
            BitmapImage bmTree = new BitmapImage(new Uri(@"Resources\tree.png", UriKind.RelativeOrAbsolute));
            //Скала (непроходимая)
            BitmapImage bmRock = new BitmapImage(new Uri(@"Resources\rock.png", UriKind.RelativeOrAbsolute));
            _singleObjectBrushes[(int)AliveChessLibrary.GameObjects.Objects.SingleObjectType.Obstacle] =
                new ImageBrush(bmRock);
            _singleObjectBrushes[(int)AliveChessLibrary.GameObjects.Objects.SingleObjectType.Tree] =
                new ImageBrush(bmTree);

            BitmapImage bmGoldMine = new BitmapImage(new Uri(@"Resources\goldmine.png", UriKind.RelativeOrAbsolute));
            BitmapImage bmCoalMine = new BitmapImage(new Uri(@"Resources\coalmine.png", UriKind.RelativeOrAbsolute));
            BitmapImage bmIronMine = new BitmapImage(new Uri(@"Resources\ironmine.png", UriKind.RelativeOrAbsolute));
            BitmapImage bmQuarry = new BitmapImage(new Uri(@"Resources\quarry.png", UriKind.RelativeOrAbsolute));
            BitmapImage bmSawMill = new BitmapImage(new Uri(@"Resources\sawmill.png", UriKind.RelativeOrAbsolute));
            _mineBrushes[(int)AliveChessLibrary.GameObjects.Resources.ResourceTypes.Gold] = new ImageBrush(bmGoldMine);
            _mineBrushes[(int)AliveChessLibrary.GameObjects.Resources.ResourceTypes.Coal] = new ImageBrush(bmCoalMine);
            _mineBrushes[(int)AliveChessLibrary.GameObjects.Resources.ResourceTypes.Iron] = new ImageBrush(bmIronMine);
            _mineBrushes[(int)AliveChessLibrary.GameObjects.Resources.ResourceTypes.Stone] = new ImageBrush(bmQuarry);
            _mineBrushes[(int)AliveChessLibrary.GameObjects.Resources.ResourceTypes.Wood] = new ImageBrush(bmSawMill);

            BitmapImage bmGold = new BitmapImage(new Uri(@"Resources\gold.png", UriKind.RelativeOrAbsolute));
            BitmapImage bmCoal = new BitmapImage(new Uri(@"Resources\coal.png", UriKind.RelativeOrAbsolute));
            BitmapImage bmIron = new BitmapImage(new Uri(@"Resources\iron.png", UriKind.RelativeOrAbsolute));
            BitmapImage bmStone = new BitmapImage(new Uri(@"Resources\stone.png", UriKind.RelativeOrAbsolute));
            BitmapImage bmWood = new BitmapImage(new Uri(@"Resources\wood.png", UriKind.RelativeOrAbsolute));
            _resourceBrushes[(int)AliveChessLibrary.GameObjects.Resources.ResourceTypes.Gold] = new ImageBrush(bmGold);
            _resourceBrushes[(int)AliveChessLibrary.GameObjects.Resources.ResourceTypes.Coal] = new ImageBrush(bmCoal);
            _resourceBrushes[(int)AliveChessLibrary.GameObjects.Resources.ResourceTypes.Iron] = new ImageBrush(bmIron);
            _resourceBrushes[(int)AliveChessLibrary.GameObjects.Resources.ResourceTypes.Stone] = new ImageBrush(bmStone);
            _resourceBrushes[(int)AliveChessLibrary.GameObjects.Resources.ResourceTypes.Wood] = new ImageBrush(bmWood);
        }

        private void InitRectangles()
        {
            ImageBrush brush0 = _landscapeBrushes[0];
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Height = height;
                    r.Width = width;
                    r.Fill = brush0; //TODO: Узнать тип местности(трава, грязь, снег) и нарисовать его, а не просто траву
                    TranslateTransform t = new TranslateTransform();
                    t.X = i * width;
                    t.Y = j * height;
                    r.RenderTransform = t;

                    rectArrLandscape[i, j] = r;

                }
                for (int k = 0; k < 50; k++)
                {
                    Rectangle r = new Rectangle();
                    r.Height = height;
                    r.Width = width;
                    r.Fill = Brushes.Transparent;
                    TranslateTransform t = new TranslateTransform();
                    t.X = i * width;
                    t.Y = k * height;
                    r.RenderTransform = t;

                    rectArrGameObjects[i, k] = r;

                }
                for (int k = 0; k < 50; k++)
                {
                    Rectangle r = new Rectangle();
                    r.Height = height;
                    r.Width = width;
                    r.Fill = Brushes.Transparent;
                    TranslateTransform t = new TranslateTransform();
                    t.X = i * width;
                    t.Y = k * height;
                    r.RenderTransform = t;

                    rectArrLandscapeObjects[i, k] = r;

                }
            }
            _rectIsInit = true;
        }

        private void AddRectanglesToCanvas()
        {
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    canvasMap.Children.Add(rectArrLandscape[i, j]);
                    canvasMapObjects.Children.Add(rectArrLandscapeObjects[i, j]);
                    canvasGame.Children.Add(rectArrGameObjects[i, j]);
                }
            }
        }

        public void ConnectCallback(bool isConnected)
        {
            try
            {
                NavigationService.RemoveBackEntry();
            }
            catch (Exception) { }
            GetMapRequest request = new GetMapRequest();
            GameCore.Instance.Network.Send(request);
        }

        public void ShowGetMapResult()
        {
            //InitRectangles();


            foreach (var obj in _world.Map.BasePoints)
            {
                rectArrLandscape[obj.X, obj.Y].Fill = _landscapeBrushes[(int)obj.LandscapeType];
            }
            foreach (var obj in _world.Map.SingleObjects)
            {
                rectArrLandscapeObjects[obj.X, obj.Y].Fill = _singleObjectBrushes[(int)obj.SingleObjectType];
            }
            foreach (var obj in _world.Map.Resources)
            {
                rectArrGameObjects[obj.X, obj.Y].Fill = _resourceBrushes[(int)obj.ResourceType];
            }

            foreach (var obj in _world.Map.Mines)
            {
                rectArrGameObjects[obj.X, obj.Y].Fill = _mineBrushes[(int)obj.MineType];
            }
            AddRectanglesToCanvas();
            //Получение короля и замка:
            GetGameState();
            //GetKing();

        }

        public void ShowGetMapResult(GetMapResponse responce)
        {
            ShowGetMapResult();
        }

        private void GetKing()
        {

#warning Отправка запросов серверу
            GetKingRequest request = new GetKingRequest();
            GameCore.Instance.Network.Send(request);
        }

        private void GetGameState()
        {
            //HACK: Отображение отправки GetGameStateRequest
            System.Windows.MessageBox.Show("new GetGameStateRequest");
            GetGameStateRequest request = new GetGameStateRequest();
            GameCore.Instance.Network.Send(request);
        }

        #region Выделение
        DropShadowEffect active = new DropShadowEffect();
        int lastX = 0;
        int lastY = 0;
        private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_rectIsInit)
            {
                active.BlurRadius = 10;
                active.Color = Colors.White;
                active.ShadowDepth = 5;
                Point mousePosition = e.GetPosition((IInputElement)sender);
                //индексы для ячейки карты в массиве ячеек rectArr
                int x = (int)(mousePosition.X / width);
                int y = (int)(mousePosition.Y / height);

                rectArrLandscape[lastX, lastY].Effect = null;

                rectArrLandscape[x, y].Effect = active;
                lastX = x;
                lastY = y;
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(ConnectCallback), true);
            }
        }
        #endregion

        public void ShowGetStateResult(GetGameStateResponse response)
        {

            //TODO: разобраться, зачем нужны следующие 2 строки
            rectArrGameObjects[response.Castle.X, response.Castle.Y].Fill = _brushCastle;
            _castlePosition = new Point(response.Castle.X, response.Castle.Y);

            foreach (AliveChessLibrary.GameObjects.Buildings.Castle castle in response.King.Castles)
            {
                rectArrGameObjects[castle.X, castle.Y].Fill = _brushCastle;
                _castlePosition = new Point(castle.X, castle.Y);
            }

            //MessageBox.Show(response.Resources[0].ResourceType.ToString() + ": " + response.Resources[0].CountResource +
            //                " " + response.Resources[1].ResourceType.ToString() + ": " +
            //                response.Resources[1].CountResource + " " + response.Resources[2].ResourceType.ToString() +
            //                ": " + response.Resources[2].CountResource + " " +
            //                response.Resources[3].ResourceType.ToString() + ": " + response.Resources[3].CountResource +
            //                " " + response.Resources[4].ResourceType.ToString() + ": " +
            //                response.Resources[4].CountResource);

            rectArrGameObjects[response.King.X, response.King.Y].Fill = _brushKing;
            _kingPosition = new Point(response.King.X, response.King.Y);

        }

        private void KingToFocus()
        {
            scrollViewer1.ScrollToHorizontalOffset(_kingPosition.X * width + width / 2 - scrollViewer1.ActualWidth / 2/* - scrollViewer1.ContentHorizontalOffset / 2/* - scrollViewer1.ContentHorizontalOffset / 2*/);
            scrollViewer1.ScrollToVerticalOffset(_kingPosition.Y * width + width / 2 - scrollViewer1.ActualHeight / 2);
        }

        private void canvasGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_kingIsFocused)
            {
                Point mousePosition = e.GetPosition((IInputElement)sender);
                //индексы для ячейки карты в массиве ячеек rectArr
                int x = (int)(mousePosition.X / width);
                int y = (int)(mousePosition.Y / height);

                if (IsGameObject(x, y) == GameObjects.Castle)
                {
                    ComeInCastleRequest request = new ComeInCastleRequest();
                    request.CastleId = GameCore.Instance.Player.GetKingList()[0].Castles[0].Id;
                    GameCore.Instance.Network.Send(request);
                }
                else
                {
                    MoveKing(new Point(x, y));
                }
            }
            else
                if (_rectIsInit)
                {
                    active.BlurRadius = 10;
                    active.Color = Colors.Red;
                    active.ShadowDepth = 5;
                    Point mousePosition = e.GetPosition((IInputElement)sender);
                    //индексы для ячейки карты в массиве ячеек rectArr
                    int x = (int)(mousePosition.X / width);
                    int y = (int)(mousePosition.Y / height);

                    rectArrGameObjects[lastX, lastY].Effect = null;

                    rectArrGameObjects[x, y].Effect = active;
                    lastX = x;
                    lastY = y;
                    if (IsGameObject(x, y) == GameObjects.King)
                    {
                        _kingIsFocused = true;
                    }
                }
        }

        private void MoveKing(Point kingDest)
        {
            MoveKingRequest request = new MoveKingRequest();
            request.X = (int)kingDest.X;
            request.Y = (int)kingDest.Y;
            GameCore.Instance.Network.Send(request);
        }

        public void ShowGetKingResult(GetKingResponse response)
        {
            KingToFocus();
            rectArrLandscape[response.King.X, response.King.Y].Fill = _brushKing;
            _kingPosition = new Point(response.King.X, response.King.Y);
            //scrollViewer1.ScrollToHorizontalOffset(1);

        }

        private GameObjects? IsGameObject(double x, double y)
        {

            if (_kingPosition == (new Point(x, y)))
            {
                return GameObjects.King;
            }
            else if (_castlePosition == (new Point(x, y)))
            {
                return GameObjects.Castle;
            }
            else
                return null;
        }

        /*static BitmapImage bmKing = new BitmapImage(new Uri(@"Resources\king.png", UriKind.RelativeOrAbsolute));
        ImageBrush brushKing = new ImageBrush(bmKing);*/

        public void ShowMoveKingResult(MoveKingResponse response)
        {

            if (response.Steps != null)
            {
                //foreach (var pos in response.Steps)
                //{
                //    DoMove(brushKing, pos);
                //    KingToFocus();
                //}
                _steps = response.Steps;
                _nextStep = new Point();
                _nextStep.X = _steps[0].X;
                _nextStep.Y = _steps[0].Y;
                _stepCount = _steps.Count;
                timerMove.Start();
            }
            _kingIsFocused = false;
            //rectArrGame[response.King.X, response.King.Y].Fill = brushKing;
        }

        public void DoMove(Position pos)
        {
            rectArrGameObjects[(int)_kingPosition.X, (int)_kingPosition.Y].Fill = Brushes.Transparent;
            rectArrGameObjects[pos.X, pos.Y].Fill = _brushKing;
            _kingPosition.X = pos.X;
            _kingPosition.Y = pos.Y;
            //if (_kingIsFocused)
            KingToFocus();
            canvasGame.InvalidateVisual();
            stackPanel.InvalidateVisual();
            rectArrGameObjects[pos.X, pos.Y].InvalidateVisual();
            //System.Threading.Thread.SpinWait(100);
            //System.Threading.Thread.Sleep(100);

        }

        /// <summary>
        /// Отображение объектов в зоне видимости
        /// </summary>
        /// <param name="response"></param>
        public void ShowGetObjectsResult(GetObjectsResponse response)
        {
            foreach (var item in response.Resources)
            {
                if (!(collapsedResource.Contains(item.Id)))
                {
                    rectArrGameObjects[item.X, item.Y].Fill = _resourceBrushes[(int)item.ResourceType];
                }
            }
            foreach (var obj in _world.Map.Mines)
            {
                rectArrGameObjects[obj.X, obj.Y].Fill = _mineBrushes[(int)obj.MineType];
            }
        }

        public void ShowGetResourceMessageResult(GetResourceMessage message)
        {
            Resource res = GetResource(message.Resource.ResourceType);
            if (res != null)
                res.CountResource += message.Resource.CountResource;
            message.Resource.ChangeMapStateEvent += ChangeMapStateEventHandler;
            message.Resource.Disappear();
            //UpdateWorldMessage updateMessage = new UpdateWorldMessage(message.Resource, /**/, UpdateType.ResourceDisappear);
        }

        private Resource GetResource(ResourceTypes type)
        {
            foreach (Resource res in _player.GetKingList().First().Resources)
            {
                if (res.ResourceType == type)
                    return res;
            }
            return null;
        }

        public void ShowComeInCastleResult(ComeInCastleResponse response)
        {
            Uri uri = new Uri("/GameLayer/PresentationLayer/CastleScene.xaml",
                                                       UriKind.Relative);
            base.MoveTo(uri);
            if ((response.CastleId == _player.GetKingList().First().Castles[0].Id) && (NavigationService != null))
                NavigationService.Navigate(uri);
        }

        private void SceneMap_Loaded(object sender, RoutedEventArgs e)
        {
            if (_rectIsInit == false)
            {
                ConnectCallback(true);
            }
        }

        List<int> collapsedResource = new List<int>();

        private void ChangeMapStateEventHandler(IMapObject sender, UpdateWorldEventArgs eventArgs)
        {
            //rectArrGame[sender.X, sender.Y].Fill = Brushes.Transparent;
            collapsedResource.Add(sender.Id);
        }
    }

    public enum GameObjects
    {
        Castle, King
    }


}
