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
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;


namespace AliveChess.GameLayer.PresentationLayer
{
    /// <summary>
    /// Interaction logic for MapScene.xaml
    /// </summary>
    public partial class MapScene : GameScene
    {
        private GameWorld _world = GameCore.Instance.World;
        private Player _player = GameCore.Instance.Player;

        private Rectangle[,] rectArrGround;
        private Rectangle[,] rectArrLandscape;
        private Rectangle[,] rectArrBuildings;
        private Rectangle[,] rectArrDynamicObjects;

        private const int width = 50;
        private const int height = width;
        private bool _rectanglesInitialized = false;

        private bool _kingSelected = false;
        private bool _kingInCastle = false;
        private bool _followingKing = false;


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
            InitializeComponent();
            scrollViewerMap.CanContentScroll = false;
            InitBrushes();
            GameCore.Instance.BigMapCommandController.MapScene = this;
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(ConnectCallback), true);
            ResponceComplete.responceComplete += new ResponceCompleteDelegate(ResponceComplete_responceComplete);
            /*scrollViewerMap.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            scrollViewerMap.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;*/
            //GameCore.Instance.BigMapCommandController.SendGetMapRequest();
        }
        private delegate void MyDelegate();
        private event MyDelegate MyEvent;

        void ResponceComplete_responceComplete()
        {
            if (MyEvent != null)
                MyEvent();
        }

        public void ConnectCallback(bool isConnected)
        {
            try
            {
                NavigationService.RemoveBackEntry();
            }
            catch (Exception) { }
            GameCore.Instance.BigMapCommandController.SendGetMapRequest();
        }

        private void SceneMap_Loaded(object sender, RoutedEventArgs e)
        {
            if (_rectanglesInitialized == false)
            {
                ConnectCallback(true);
            }
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
            _selectionLighting.Color = Colors.Gold;
        }

        private void InitRectangles()
        {
            rectArrGround = new Rectangle[_world.Map.SizeX, _world.Map.SizeY];
            rectArrLandscape = new Rectangle[_world.Map.SizeX, _world.Map.SizeY];
            rectArrBuildings = new Rectangle[_world.Map.SizeX, _world.Map.SizeY];
            rectArrDynamicObjects = new Rectangle[_world.Map.SizeX, _world.Map.SizeY];
            canvasGround.Width = width * rectArrGround.GetLength(0);
            canvasGround.Height = height * rectArrGround.GetLength(1);
            canvasLandscape.Width = width * rectArrLandscape.GetLength(0);
            canvasLandscape.Height = height * rectArrLandscape.GetLength(1);
            canvasBuildings.Width = width * rectArrBuildings.GetLength(0);
            canvasBuildings.Height = height * rectArrBuildings.GetLength(1);
            canvasDynamicObjects.Width = width * rectArrDynamicObjects.GetLength(0);
            canvasDynamicObjects.Height = height * rectArrDynamicObjects.GetLength(1);
            for (int i = 0; i < _world.Map.SizeX; i++)
            {
                for (int j = 0; j < _world.Map.SizeY; j++)
                {
                    rectArrGround[i, j] = null;
                }
                for (int k = 0; k < _world.Map.SizeY; k++)
                {
                    Rectangle r = new Rectangle();
                    r.CacheMode = new BitmapCache();
                    r.Height = height;
                    r.Width = width;
                    r.Fill = Brushes.Transparent;
                    TranslateTransform t = new TranslateTransform();
                    t.X = i * width;
                    t.Y = k * height;
                    r.RenderTransform = t;
                    rectArrLandscape[i, k] = r;
                }
                for (int k = 0; k < _world.Map.SizeY; k++)
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
                for (int k = 0; k < _world.Map.SizeY; k++)
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
            _rectanglesInitialized = true;
        }

        private void AddRectanglesToCanvas()
        {
            for (int i = 0; i < _world.Map.SizeX; i++)
            {
                for (int j = 0; j < _world.Map.SizeY; j++)
                {
                    if (rectArrGround[i, j] != null)
                        canvasGround.Children.Add(rectArrGround[i, j]);
                    canvasLandscape.Children.Add(rectArrLandscape[i, j]);
                    canvasBuildings.Children.Add(rectArrBuildings[i, j]);
                    canvasDynamicObjects.Children.Add(rectArrDynamicObjects[i, j]);
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
                    {
                        rectangles[i, j].Fill = Brushes.Transparent;
                        rectangles[i, j].Effect = null;
                    }
                }
            }
        }

        private MapPoint _getPoint(int x, int y)
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

        private void KingToFocus()
        {
            if (_player.King.X * width >= scrollViewerMap.HorizontalOffset + scrollViewerMap.ActualWidth - 2 * width)
                scrollViewerMap.ScrollToHorizontalOffset(_player.King.X * width - scrollViewerMap.ActualWidth + 4 * width);
            if (_player.King.X * width <= scrollViewerMap.HorizontalOffset + 2 * width)
                scrollViewerMap.ScrollToHorizontalOffset(_player.King.X * width - 4 * width);
            if (_player.King.Y * height >= scrollViewerMap.VerticalOffset + scrollViewerMap.ActualHeight - 3 * height)
                scrollViewerMap.ScrollToVerticalOffset(_player.King.Y * height - scrollViewerMap.ActualHeight + 5 * height);
            if (_player.King.Y * height <= scrollViewerMap.VerticalOffset + 2 * height)
                scrollViewerMap.ScrollToVerticalOffset(_player.King.Y * height - 4 * height);
            /*{
                scrollViewerMap.ScrollToHorizontalOffset(_player.King.X * width + width / 2 - scrollViewerMap.ActualWidth / 2);
                scrollViewerMap.ScrollToVerticalOffset(_player.King.Y * width + width / 2 - scrollViewerMap.ActualHeight / 2);
                _followingKing = true;
            }*/
            if (!_kingSelected)
                _followingKing = false;
        }

        public void DrawGround()
        {
            if (!_rectanglesInitialized)
                InitRectangles();
            foreach (var basePoint in _world.Map.BasePoints)
            {
                rectArrGround[basePoint.X, basePoint.Y] = _createRectangle(basePoint.X, basePoint.Y, basePoint.LandscapeType);
            }

            foreach (var basePoint in _world.Map.BasePoints)
            {
                Queue<MapPoint> cells = new Queue<MapPoint>();
                cells.Enqueue(_getPoint(basePoint.X, basePoint.Y));

                while (cells.Count > 0)
                {
                    MapPoint landscape = cells.Dequeue();

                    if (landscape.X > 0 && landscape.X < _world.Map.SizeX && rectArrGround[landscape.X - 1, landscape.Y] == null)
                    {
                        cells.Enqueue(_getPoint(landscape.X - 1, landscape.Y));
                        rectArrGround[landscape.X - 1, landscape.Y] = _createRectangle(landscape.X - 1, landscape.Y, basePoint.LandscapeType);
                    }
                    if (landscape.X < _world.Map.SizeX - 1 && rectArrGround[landscape.X + 1, landscape.Y] == null)
                    {
                        cells.Enqueue(_getPoint(landscape.X + 1, landscape.Y));
                        rectArrGround[landscape.X + 1, landscape.Y] = _createRectangle(landscape.X + 1, landscape.Y, basePoint.LandscapeType);
                    }
                    if (landscape.Y > 0 && landscape.Y < _world.Map.SizeY && rectArrGround[landscape.X, landscape.Y - 1] == null)
                    {
                        cells.Enqueue(_getPoint(landscape.X, landscape.Y - 1));
                        rectArrGround[landscape.X, landscape.Y - 1] = _createRectangle(landscape.X, landscape.Y - 1, basePoint.LandscapeType);
                    }
                    if (landscape.Y < _world.Map.SizeY - 1 && rectArrGround[landscape.X, landscape.Y + 1] == null)
                    {
                        cells.Enqueue(_getPoint(landscape.X, landscape.Y + 1));
                        rectArrGround[landscape.X, landscape.Y + 1] = _createRectangle(landscape.X, landscape.Y + 1, basePoint.LandscapeType);
                    }
                }
            }
        }

        public void DrawLandscape()
        {
            if (!_rectanglesInitialized)
                InitRectangles();
            foreach (var obj in _world.Map.SingleObjects)
            {
                rectArrLandscape[obj.X, obj.Y].Fill = _landscapeBrushes[(int)obj.SingleObjectType];
            }
        }

        public void DrawBuildings()
        {
            if (!_rectanglesInitialized)
                InitRectangles();
            //ClearRectangles(rectArrBuildings);
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
            if (!_rectanglesInitialized)
                InitRectangles();
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
            if (_kingSelected)
                rectArrDynamicObjects[_player.King.X, _player.King.Y].Effect = _selectionLighting;
        }

        public void DrawAll()
        {
            DrawGround();
            DrawLandscape();
            DrawBuildings();
            DrawDynamicObjects();
        }

        private void canvasLandscape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void canvasDynamicObjects_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition((IInputElement)sender);
            //индексы для ячейки карты в массиве ячеек rectArr
            int x = (int)(mousePosition.X / width);
            int y = (int)(mousePosition.Y / height);
            if (_kingSelected)
            {
                foreach (var castle in _world.Map.Castles)
                {
                    if (castle.X == x && castle.Y == y)
                    {
                        GameCore.Instance.BigMapCommandController.SendComeInCastleRequest(castle.Id);
                        return;
                    }
                }
                if (x != _player.King.X || y != _player.King.Y)
                {
                    GameCore.Instance.BigMapCommandController.SendMoveKingRequest(new Point(x, y));
                }
                else
                {
                    _followingKing = !_followingKing;
                }
            }
            else if (_player.King.X == x && _player.King.Y == y)
            {
                _kingSelected = true;
            }
        }

        private void canvasDynamicObjects_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _kingSelected = false;
            _followingKing = false;
        }


        public void ShowGetMapResult()
        {
            InitRectangles();
            DrawAll();
            AddRectanglesToCanvas();
            _followingKing = true;
        }

        public void ShowGetGameStateResult()
        {
            foreach (var res in _player.King.ResourceStore.Resources)
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

        /// <summary>
        /// Отображение объектов в зоне видимости
        /// </summary>
        public void ShowGetObjectsResult()
        {
            DrawBuildings();
            DrawDynamicObjects();
            if (_followingKing)
                KingToFocus();
        }

        public void ShowCaptureMineResult()
        {
            //DrawBuildings();
        }

        public void ShowGetKingResult()
        {
        }

        public void ShowMoveKingResult()
        {
        }

        public void ShowGetResourceMessageResult(GetResourceMessage message)
        {
        }

        public void ShowComeInCastleResult()
        {
            //timerUpdate.Stop();
            Uri uri = new Uri("/GameLayer/PresentationLayer/CastleScene.xaml", UriKind.Relative);
            base.MoveTo(uri);
            if ((NavigationService != null))
                NavigationService.Navigate(uri);
        }

        private void scrollViewerMap_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //_followingKing = false;
        }

        private void scrollViewerMap_KeyDown(object sender, KeyEventArgs e)
        {
            _followingKing = false;
            /*if (e.Key == Key.Up || e.Key == Key.W)
            {
                scrollViewerMap.ScrollToVerticalOffset(scrollViewerMap.VerticalOffset - height);
                _followingKing = false;
            } 
            else if (e.Key == Key.Down || e.Key == Key.S)
            {
                scrollViewerMap.ScrollToVerticalOffset(scrollViewerMap.VerticalOffset + height);
                _followingKing = false;
            } 
            else if (e.Key == Key.Left || e.Key == Key.A)
            {
                scrollViewerMap.ScrollToHorizontalOffset(scrollViewerMap.HorizontalOffset - width);
                _followingKing = false;
            } 
            else if (e.Key == Key.Right || e.Key == Key.D)
            {
                scrollViewerMap.ScrollToHorizontalOffset(scrollViewerMap.HorizontalOffset + width);
                _followingKing = false;
            } */
        }
    }

}
