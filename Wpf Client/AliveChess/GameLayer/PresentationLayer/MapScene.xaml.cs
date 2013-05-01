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
using AliveChess.Utilities;
using AliveChessLibrary;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
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

        private Rectangle[,] rectArrGround = new Rectangle[50, 50];
        private Rectangle[,] rectArrLandscape = new Rectangle[50, 50];
        private Rectangle[,] rectArrBuildings = new Rectangle[50, 50];
        private Rectangle[,] rectArrDynamicObjects = new Rectangle[50, 50];

        private const int width = 50;
        private const int height = 50;
        private bool _rectIsInit = false;

        private bool _kingIsFocused = false;
        private bool _kingInCastle = false;

        DispatcherTimer timerUpdate = new DispatcherTimer();


        private ImageBrush _brushKing;
        private ImageBrush _brushCastle;
        private ImageBrush[] _groundBrushes = new ImageBrush[4];
        private ImageBrush[] _landscapeBrushes = new ImageBrush[2];
        private ImageBrush[] _mineBrushes = new ImageBrush[5];
        private ImageBrush[] _resourceBrushes = new ImageBrush[5];
        DropShadowEffect _enemyLighting = new DropShadowEffect();
        DropShadowEffect _playerLighting = new DropShadowEffect();
        DropShadowEffect _selectionLighting = new DropShadowEffect();

        public MapScene()
        {
            //if (!rectIsInit)
            //{
            InitializeComponent();
            canvasGround.Width = width * rectArrGround.GetLength(0);
            canvasGround.Height = height * rectArrGround.GetLength(1);
            canvasLandscape.Width = width * rectArrLandscape.GetLength(0);
            canvasLandscape.Height = height * rectArrLandscape.GetLength(1);
            canvasBuildings.Width = width * rectArrBuildings.GetLength(0);
            canvasBuildings.Height = height * rectArrBuildings.GetLength(1);
            canvasDynamicObjects.Width = width * rectArrDynamicObjects.GetLength(0);
            canvasDynamicObjects.Height = height * rectArrDynamicObjects.GetLength(1);
            /*scrollViewerMap.Width = width * rectArrGround.GetLength(0);
            scrollViewerMap.Height = height * rectArrGround.GetLength(1);*/
            InitBrushes();
            InitRectangles();
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(ConnectCallback), true);
            ResponceComplete.responceComplete += new ResponceCompleteDelegate(ResponceComplete_responceComplete);
            timerUpdate.Tick += new EventHandler(timerUpdate_Tick);
            timerUpdate.Interval = new TimeSpan(0, 0, 0, 0, 20);

            //}
            //MyEvent += new MyDelegate(MapScene_MyEvent);
        }

        /// <summary>
        /// Обновление состояния игры (ресурсы, замки, короли)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerUpdate_Tick(object sender, EventArgs e)
        {
            GetGameState();
            GetObjectsRequest r = new GetObjectsRequest();
            GameCore.Instance.Network.Send(r);
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
            ImageBrush grass = new ImageBrush(bmGrass);
            grass.Stretch = Stretch.UniformToFill;
            _groundBrushes[(int)LandscapeTypes.Grass] = grass;
            _groundBrushes[(int)LandscapeTypes.Snow] = new ImageBrush(bmSnow);
            _groundBrushes[(int)LandscapeTypes.Ground] = new ImageBrush(bmGround);
            _groundBrushes[(int)LandscapeTypes.None] = new ImageBrush(bmNone);

            //Одиночное дерево
            BitmapImage bmTree = new BitmapImage(new Uri(@"Resources\tree.png", UriKind.RelativeOrAbsolute));
            //Скала (непроходимая)
            BitmapImage bmRock = new BitmapImage(new Uri(@"Resources\rock.png", UriKind.RelativeOrAbsolute));
            _landscapeBrushes[(int)AliveChessLibrary.GameObjects.Objects.SingleObjectType.Obstacle] =
                new ImageBrush(bmRock);
            _landscapeBrushes[(int)AliveChessLibrary.GameObjects.Objects.SingleObjectType.Tree] =
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

            _enemyLighting.BlurRadius = 10;
            _enemyLighting.ShadowDepth = 5;
            _enemyLighting.Color = Colors.OrangeRed;
            _playerLighting.BlurRadius = 10;
            _playerLighting.ShadowDepth = 5;
            _playerLighting.Color = Colors.Honeydew;
            _selectionLighting.BlurRadius = 10;
            _selectionLighting.ShadowDepth = 5;
            _selectionLighting.Color = Colors.LawnGreen;
        }

        private void InitRectangles()
        {
            ImageBrush brush0 = _groundBrushes[0];
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    /*Rectangle r = new Rectangle();
                    r.Height = height + 1;
                    r.Width = width + 1;
                    r.Fill = brush0;
                    TranslateTransform t = new TranslateTransform();
                    t.X = i * width;
                    t.Y = j * height;
                    r.RenderTransform = t;*/
                    rectArrGround[i, j] = null;

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
                    rectArrLandscape[i, k] = r;
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
                    rectArrBuildings[i, k] = r;
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
                    rectArrDynamicObjects[i, k] = r;
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
                    if (rectArrGround[i, j] != null)
                        canvasGround.Children.Add(rectArrGround[i, j]);
                    canvasLandscape.Children.Add(rectArrLandscape[i, j]);
                    canvasBuildings.Children.Add(rectArrBuildings[i, j]);
                    canvasDynamicObjects.Children.Add(rectArrDynamicObjects[i, j]);
                }
            }
            foreach (var castle in this._world.Map.Castles)
            {
                if (castle.KingId == _player.King.Id)
                {
                    /*Ellipse e = new Ellipse();
                    e.Width = width/3;
                    e.Height = height/3;
                    e.Fill = new RadialGradientBrush(Colors.GreenYellow, Colors.LightGreen);
                    TranslateTransform t = new TranslateTransform();
                    t.X = (castle.X + castle.Width) * width - e.Width;
                    t.Y = castle.Y * height;
                    e.RenderTransform = t;
                    canvasDynamicObjects.Children.Add(e);*/
                    DropShadowEffect effect = new DropShadowEffect();
                    effect.Color = Colors.Honeydew;
                    effect.BlurRadius = 10;
                    effect.ShadowDepth = 5;
                    rectArrBuildings[castle.X, castle.Y].Effect = effect;
                }
            }
        }

        public void ClearRectangles(Rectangle[,] rectangles)
        {
            for (int i = 0; i < rectangles.GetLength(0); i++)
            {
                for (int j = 0; j < rectangles.GetLength(1); j++)
                {
                    if (rectangles[i, j] != null)
                        rectangles[i, j].Fill = Brushes.Transparent;
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
            DrawAll();
            //Получение короля и замка:
            GetGameState();
            //TODO: Вероятно, нужно для нормального возврата из замка
            //GetKing();
            timerUpdate.Start();

        }

        private MapPoint GetPoint(int x, int y)
        {
            return this._world.Map[x, y];
        }

        private Rectangle _createRectangle(int X, int Y, LandscapeTypes type)
        {
            Rectangle r = new Rectangle();
            r.Height = height + 1;
            r.Width = width + 1;
            r.Fill = _groundBrushes[(int)type];
            TranslateTransform t = new TranslateTransform();
            t.X = X * width;
            t.Y = Y * height;
            r.RenderTransform = t;
            return r;
        }

        public void DrawGround()
        {
            foreach (var basePoint in _world.Map.BasePoints)
            {

                rectArrGround[basePoint.X, basePoint.Y] = _createRectangle(basePoint.X, basePoint.Y, basePoint.LandscapeType);
            }

            foreach (var basePoint in _world.Map.BasePoints)
            {
                Queue<MapPoint> cells = new Queue<MapPoint>();
                cells.Enqueue(GetPoint(basePoint.X, basePoint.Y));

                while (cells.Count > 0)
                {
                    MapPoint landscape = cells.Dequeue();

                    if (landscape.X > 0 && landscape.X < _world.Map.SizeX && rectArrGround[landscape.X - 1, landscape.Y] == null)
                    {
                        cells.Enqueue(GetPoint(landscape.X - 1, landscape.Y));
                        rectArrGround[landscape.X - 1, landscape.Y] = _createRectangle(landscape.X - 1, landscape.Y, basePoint.LandscapeType);
                    }
                    if (landscape.X < _world.Map.SizeX - 1 && rectArrGround[landscape.X + 1, landscape.Y] == null)
                    {
                        cells.Enqueue(GetPoint(landscape.X + 1, landscape.Y));
                        rectArrGround[landscape.X + 1, landscape.Y] = _createRectangle(landscape.X + 1, landscape.Y, basePoint.LandscapeType);
                    }
                    if (landscape.Y > 0 && landscape.Y < _world.Map.SizeY && rectArrGround[landscape.X, landscape.Y - 1] == null)
                    {
                        cells.Enqueue(GetPoint(landscape.X, landscape.Y - 1));
                        rectArrGround[landscape.X, landscape.Y - 1] = _createRectangle(landscape.X, landscape.Y - 1, basePoint.LandscapeType);
                    }
                    if (landscape.Y < _world.Map.SizeY - 1 && rectArrGround[landscape.X, landscape.Y + 1] == null)
                    {
                        cells.Enqueue(GetPoint(landscape.X, landscape.Y + 1));
                        rectArrGround[landscape.X, landscape.Y + 1] = _createRectangle(landscape.X, landscape.Y + 1, basePoint.LandscapeType);
                    }
                }
            }
        }

        public void DrawLandscape()
        {
            foreach (var obj in _world.Map.SingleObjects)
            {
                rectArrLandscape[obj.X, obj.Y].Fill = _landscapeBrushes[(int)obj.SingleObjectType];
            }
        }

        public void DrawBuildings()
        {
            ClearRectangles(rectArrBuildings);
            foreach (var obj in _world.Map.Mines)
            {
                rectArrBuildings[obj.X, obj.Y].Fill = _mineBrushes[(int)obj.MineType];
                rectArrBuildings[obj.X, obj.Y].Width = obj.Width * width;
                rectArrBuildings[obj.X, obj.Y].Height = obj.Height * height;

                if (obj.KingId == _player.King.Id)
                    rectArrBuildings[obj.X, obj.Y].Effect = _playerLighting;
                else if (obj.KingId != null)
                    rectArrBuildings[obj.X, obj.Y].Effect = _enemyLighting;
            }
            foreach (var obj in _world.Map.Castles)
            {
                rectArrBuildings[obj.X, obj.Y].Fill = _brushCastle;
                rectArrBuildings[obj.X, obj.Y].Width = obj.Width * width;
                rectArrBuildings[obj.X, obj.Y].Height = obj.Height * height;

                if (obj.KingId == _player.King.Id)
                    rectArrBuildings[obj.X, obj.Y].Effect = _playerLighting;
                else if (obj.KingId != null)
                    rectArrBuildings[obj.X, obj.Y].Effect = _enemyLighting;
            }
        }

        public void DrawDynamicObjects()
        {
            ClearRectangles(rectArrDynamicObjects);
            foreach (var obj in _world.Map.Resources)
            {
                rectArrDynamicObjects[obj.X, obj.Y].Fill = _resourceBrushes[(int)obj.ResourceType];
            }
            foreach (var obj in _world.Map.Kings)
            {
                rectArrDynamicObjects[obj.X, obj.Y].Fill = _brushKing;

                if (obj.Id != _player.King.Id)
                    rectArrDynamicObjects[obj.X, obj.Y].Effect = _enemyLighting;
            }
        }

        public void DrawAll()
        {
            DrawGround();
            DrawLandscape();
            DrawBuildings();
            DrawDynamicObjects();
            AddRectanglesToCanvas();
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
            GetGameStateRequest request = new GetGameStateRequest();
            GameCore.Instance.Network.Send(request);
        }

        #region Выделение
        DropShadowEffect active = new DropShadowEffect();
        int lastX = 0;
        int lastY = 0;
        private void canvasLandscape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {//TODO: Проверить, работает ли
            if (_rectIsInit)
            {
                active.BlurRadius = 10;
                active.Color = Colors.Red;
                active.ShadowDepth = 5;
                Point mousePosition = e.GetPosition((IInputElement)sender);
                //индексы для ячейки карты в массиве ячеек rectArr
                int x = (int)(mousePosition.X / width);
                int y = (int)(mousePosition.Y / height);

                rectArrGround[lastX, lastY].Effect = null;

                rectArrGround[x, y].Effect = active;
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
            /*foreach (AliveChessLibrary.GameObjects.Buildings.Castle castle in _player.King.Castles)
            {
                rectArrBuildings[castle.X, castle.Y].Fill = _brushCastle;
                rectArrBuildings[castle.X, castle.Y].Width = castle.Width * width;
                rectArrBuildings[castle.X, castle.Y].Height = castle.Height * height;
                rectArrBuildings[castle.X, castle.Y].Effect = _playerLighting;
            }*/
            foreach (var res in _player.King.Resources)
            {
                switch (res.ResourceType)
                {
                    case ResourceTypes.Gold:
                        LabelGoldQuantity.Content = res.CountResource.ToString();
                        break;
                    case ResourceTypes.Stone:
                        LabelStoneQuantity.Content = res.CountResource.ToString();
                        break;
                    case ResourceTypes.Wood:
                        LabelWoodQuantity.Content = res.CountResource.ToString();
                        break;
                    case ResourceTypes.Iron:
                        LabelIronQuantity.Content = res.CountResource.ToString();
                        break;
                    case ResourceTypes.Coal:
                        LabelCoalQuantity.Content = res.CountResource.ToString();
                        break;
                }
            }
            /*if (_kingPosition.X >= 0)
                rectArrDynamicObjects[(int)_kingPosition.X, (int)_kingPosition.Y].Fill = _brushKing;
            else
            {*/
            /*if (_kingPosition.X >= 0)
                rectArrDynamicObjects[(int)_kingPosition.X, (int)_kingPosition.Y].Fill = Brushes.Transparent;
            _kingPosition = new Point(_player.King.X, _player.King.Y);
            rectArrDynamicObjects[_player.King.X, _player.King.Y].Fill = _brushKing;*/
            //KingToFocus();

            //}
            /*Refresh();*/

        }

        public void ShowCaptureMineResult(CaptureMineResponse response)
        {
            DrawBuildings();
        }

        private void KingToFocus()
        {
            scrollViewerMap.ScrollToHorizontalOffset(_player.King.X * width + width / 2 - scrollViewerMap.ActualWidth / 2);
            scrollViewerMap.ScrollToVerticalOffset(_player.King.Y * width + width / 2 - scrollViewerMap.ActualHeight / 2);
        }

        private void canvasDynamicObjects_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                    Point mousePosition = e.GetPosition((IInputElement)sender);
                    //индексы для ячейки карты в массиве ячеек rectArr
                    int x = (int)(mousePosition.X / width);
                    int y = (int)(mousePosition.Y / height);

                    rectArrDynamicObjects[lastX, lastY].Effect = null;

                    rectArrDynamicObjects[x, y].Effect = _selectionLighting;
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
            /*KingToFocus();
            rectArrDynamicObjects[response.King.X, response.King.Y].Fill = _brushKing;
            _kingPosition = new Point(response.King.X, response.King.Y);*/
            //scrollViewer1.ScrollToHorizontalOffset(1);

        }

        private GameObjects? IsGameObject(int x, int y)
        {

            if (_player.King.X == x && _player.King.Y == y)
            {
                return GameObjects.King;
            }
            else
            {
                foreach (var castle in _player.King.Castles)
                {
                    if (castle.X == x && castle.Y == y)
                        return GameObjects.Castle;
                }
            }
            return null;
        }

        public void ShowMoveKingResult(MoveKingResponse response)
        {/*

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
            }*/
            _kingIsFocused = false;
            //rectArrGame[response.King.X, response.King.Y].Fill = brushKing;
        }

        public void DoMove(Position pos)
        {
            /*rectArrDynamicObjects[(int)_kingPosition.X, (int)_kingPosition.Y].Fill = Brushes.Transparent;
            rectArrDynamicObjects[pos.X, pos.Y].Fill = _brushKing;
            _kingPosition.X = pos.X;
            _kingPosition.Y = pos.Y;
            //if (_kingIsFocused)
            KingToFocus();
            canvasDynamicObjects.InvalidateVisual();
            rectArrDynamicObjects[pos.X, pos.Y].InvalidateVisual();
            //System.Threading.Thread.SpinWait(100);
            //System.Threading.Thread.Sleep(100);*/

        }

        /// <summary>
        /// Отображение объектов в зоне видимости
        /// </summary>
        /// <param name="response"></param>
        public void ShowGetObjectsResult(GetObjectsResponse response)
        {
            DrawDynamicObjects();
            DrawBuildings();
        }

        public void ShowGetResourceMessageResult(GetResourceMessage message)
        {
            /*Resource res = GetResource(message.Resource.ResourceType);
            if (res != null)
                res.CountResource += message.Resource.CountResource;
            message.Resource.ChangeMapStateEvent += ChangeMapStateEventHandler;
            message.Resource.Disappear();*/
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
            timerUpdate.Stop();
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
    }

    public enum GameObjects
    {
        Castle, King
    }


}
