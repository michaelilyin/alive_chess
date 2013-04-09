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

        private Rectangle[,] rectArrWorld = new Rectangle[50,50];
        private Rectangle[,] rectArrGame = new Rectangle[50, 50];

        private Point kingPosition;
        private Point castlePosition;

        int width = 100;
        int height = 100;
        bool rectIsInit = false;

        bool KingIsFocused = false;
        bool KingInCastle = false;

        DispatcherTimer timerMove = new DispatcherTimer();
        Point nextStep;
        List<Position> steps;
        int stepCount;

        public MapScene()
        {
            //if (!rectIsInit)
            //{
                InitializeComponent();
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(ConnectCallback), true);
                ResponceComplete.responceComplete += new ResponceCompleteDelegate(ResponceComplete_responceComplete);
                timerMove.Tick += new EventHandler(timerMove_Tick);
            
                
            //}
            //MyEvent += new MyDelegate(MapScene_MyEvent);
        }

        void timerMove_Tick(object sender, EventArgs e)
        {
            timerMove.Stop();

            if (nextStep != null)
            {
                DoMove(brushKing, new Position((int)(nextStep.X), (int)(nextStep.Y)));
                stepCount--;
                if (stepCount > 0)
                {
                    nextStep = new Point(steps[steps.Count - stepCount].X, steps[steps.Count - stepCount].Y);
                    timerMove.Start();
                }
                else
                {
                    steps = null;
                    stepCount = 0;
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

        
        private void InitRect()
        {
            BitmapImage bm0 = new BitmapImage(new Uri(@"Resources\0.jpg", UriKind.RelativeOrAbsolute));
            ImageBrush brush0 = new ImageBrush(bm0);
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Height = height;
                    r.Width = width;
                    r.Fill = brush0;
                    TranslateTransform t = new TranslateTransform();
                    t.X = j * width;
                    t.Y = i * height;
                    r.RenderTransform = t;

                    rectArrWorld[i, j] = r;
                        
                }
                for (int k = 0; k < 50; k++)
                {
                    Rectangle r = new Rectangle();
                    r.Height = height;
                    r.Width = width;
                    r.Fill = Brushes.Transparent;
                    TranslateTransform t = new TranslateTransform();
                    t.X = k * width;
                    t.Y = i * height;
                    r.RenderTransform = t;

                    rectArrGame[i, k] = r;

                }
            }
            rectIsInit = true;
        }

        private void AddMapToCanvas()
        {
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    canvasMap.Children.Add(rectArrWorld[i,j]);
                    canvasGame.Children.Add(rectArrGame[i,j]);
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
            InitRect();
            //трава
            BitmapImage bm0 = new BitmapImage(new Uri(@"Resources\0.jpg", UriKind.RelativeOrAbsolute));
            ImageBrush brush0 = new ImageBrush(bm0);
            //Одиночное дерево
            BitmapImage bm1 = new BitmapImage(new Uri(@"Resources\1.jpg", UriKind.RelativeOrAbsolute));
            ImageBrush brush1 = new ImageBrush(bm1);
            //Скала (непроходимая)
            BitmapImage bm2 = new BitmapImage(new Uri(@"Resources\2.jpg", UriKind.RelativeOrAbsolute));
            ImageBrush brush2 = new ImageBrush(bm2);
            //Золото (ресурс)
            BitmapImage bm3 = new BitmapImage(new Uri(@"Resources\3.jpg", UriKind.RelativeOrAbsolute));
            ImageBrush brush3 = new ImageBrush(bm3);
            //Дерево (ресурс)
            BitmapImage bm4 = new BitmapImage(new Uri(@"Resources\4.jpg", UriKind.RelativeOrAbsolute));
            ImageBrush brush4 = new ImageBrush(bm4);
            //Золотая шахта
            BitmapImage bm5 = new BitmapImage(new Uri(@"Resources\5.jpg", UriKind.RelativeOrAbsolute));
            ImageBrush brush5 = new ImageBrush(bm5);

            foreach (var obj in _world.Map.SingleObjects)
            {

                if (obj.SingleObjectType == AliveChessLibrary.GameObjects.Objects.SingleObjectType.Tree)
                {
                    rectArrWorld[obj.X, obj.Y].Fill = brush1;
                }
                else if (obj.SingleObjectType == AliveChessLibrary.GameObjects.Objects.SingleObjectType.Obstacle)
                {
                    rectArrWorld[obj.X, obj.Y].Fill = brush2;
                }
            }
            foreach (var obj in _world.Map.Resources)
            {

                if (obj.ResourceType == AliveChessLibrary.GameObjects.Resources.ResourceTypes.Gold)
                {
                     rectArrGame[obj.X, obj.Y].Fill = brush3;
                }
                else if (obj.ResourceType == AliveChessLibrary.GameObjects.Resources.ResourceTypes.Wood)
                {
                    rectArrGame[obj.X, obj.Y].Fill = brush4;
                }
            }

            foreach (var obj in _world.Map.Mines)
            {

                if (obj.MineType == ResourceTypes.Gold)
                {
                    rectArrGame[obj.X, obj.Y].Fill = brush5;
                }
                //else if (obj.ResourceType == AliveChessLibrary.GameObjects.Resources.ResourceTypes.Wood)
                //{
                //    rectArrGame[obj.X, obj.Y].Fill = brush4;
                //}
            }
            AddMapToCanvas();
            //Получение короля и замка:
            GetGameState();
            //GetKing();

        }

        private void GetKing()
        {
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
            private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                if (rectIsInit)
                {
                    active.BlurRadius = 10;
                    active.Color = Colors.Red;
                    active.ShadowDepth = 5;
                    Point mousePosition = e.GetPosition((IInputElement)sender);
                    //индексы для ячейки карты в массиве ячеек rectArr
                    int y = (int)(mousePosition.X / width);
                    int x = (int)(mousePosition.Y / height);

                    rectArrWorld[lastX, lastY].Effect = null;

                    rectArrWorld[x, y].Effect = active;
                    lastX = x;
                    lastY = y;
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<bool>(ConnectCallback), true);
                }
            }
        #endregion

            public void ShowGetMapResult(GetMapResponse response)
            {
                ShowGetMapResult();
            }

            public void ShowGetStateResult(GetGameStateResponse response)
            {
                BitmapImage bmKing = new BitmapImage(new Uri(@"Resources\king.jpg", UriKind.RelativeOrAbsolute));
                ImageBrush brushKing = new ImageBrush(bmKing);
                rectArrGame[response.King.X, response.King.Y].Fill = brushKing;
                kingPosition = new Point(response.King.X, response.King.Y);

                KingToFocus();
                
                BitmapImage bmCastle = new BitmapImage(new Uri(@"Resources\castle.jpg", UriKind.RelativeOrAbsolute));
                ImageBrush brushCastle = new ImageBrush(bmCastle);
                rectArrGame[response.Castle.X, response.Castle.Y].Fill = brushCastle;
                castlePosition = new Point(response.Castle.X, response.Castle.Y);
                foreach (AliveChessLibrary.GameObjects.Buildings.Castle castle in response.King.Castles)
                {
                    bmCastle = new BitmapImage(new Uri(@"Resources\castle.jpg", UriKind.RelativeOrAbsolute));
                    brushCastle = new ImageBrush(bmCastle);
                    rectArrGame[castle.X, castle.Y].Fill = brushCastle;
                    castlePosition = new Point(response.Castle.X, response.Castle.Y);
                }
                
            }

            private void KingToFocus()
            {
                scrollViewer1.ScrollToHorizontalOffset(kingPosition.Y * 100 - scrollViewer1.ContentHorizontalOffset/2);
                scrollViewer1.ScrollToVerticalOffset(kingPosition.X * 100);
            }

            private void canvasGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                if (KingIsFocused)
                {
                    Point mousePosition = e.GetPosition((IInputElement)sender);
                    //индексы для ячейки карты в массиве ячеек rectArr
                    int y = (int)(mousePosition.X / width);
                    int x = (int)(mousePosition.Y / height);

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
                if (rectIsInit)
                {
                    active.BlurRadius = 10;
                    active.Color = Colors.Red;
                    active.ShadowDepth = 5;
                    Point mousePosition = e.GetPosition((IInputElement)sender);
                    //индексы для ячейки карты в массиве ячеек rectArr
                    int y = (int)(mousePosition.X / width);
                    int x = (int)(mousePosition.Y / height);

                    rectArrGame[lastX, lastY].Effect = null;

                    rectArrGame[x, y].Effect = active;
                    lastX = x;
                    lastY = y;
                    if (IsGameObject(x, y) == GameObjects.King)
                    {
                        KingIsFocused = true;
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
                BitmapImage bmKing = new BitmapImage(new Uri(@"Resources\king.jpg", UriKind.RelativeOrAbsolute));
                ImageBrush brushKing = new ImageBrush(bmKing);
                rectArrWorld[response.King.X, response.King.Y].Fill = brushKing;
                kingPosition = new Point(response.King.X, response.King.Y);
                scrollViewer1.ScrollToHorizontalOffset(1);
               
            }

            private GameObjects? IsGameObject(double x, double y)
            {

                if (kingPosition == (new Point(x, y)))
                {
                    return GameObjects.King;
                }
                else if (castlePosition == (new Point(x, y)))
                {
                    return GameObjects.Castle;
                }
                else
                    return null;
            }

            static BitmapImage bmKing = new BitmapImage(new Uri(@"Resources\king.jpg", UriKind.RelativeOrAbsolute));
            ImageBrush brushKing = new ImageBrush(bmKing);

            public void ShowMoveKingResult(MoveKingResponse response)
            {
                
                if (response.Steps != null)
                {
                    //foreach (var pos in response.Steps)
                    //{
                    //    DoMove(brushKing, pos);
                    //    KingToFocus();
                    //}
                    steps = response.Steps;
                    nextStep = new Point();
                    nextStep.X = steps[0].X;
                    nextStep.Y = steps[0].Y;
                    stepCount = steps.Count; 
                    timerMove.Interval = new TimeSpan(0,0,1);
                    timerMove.Start();
                }
                KingIsFocused = false;
                //rectArrGame[response.King.X, response.King.Y].Fill = brushKing;
            }

            public void DoMove(Brush brushKing, Position pos)
            {
                rectArrGame[(int)kingPosition.X, (int)kingPosition.Y].Fill = Brushes.Transparent;
                rectArrGame[pos.X, pos.Y].Fill = brushKing;
                kingPosition.X = pos.X;
                kingPosition.Y = pos.Y;
                canvasGame.InvalidateVisual();
                stackPanel.InvalidateVisual();
                rectArrGame[pos.X, pos.Y].InvalidateVisual();
                //System.Threading.Thread.SpinWait(100);
                //System.Threading.Thread.Sleep(100);
                
            }
            
            /// <summary>
            /// Отображение объектов в зоне видимости
            /// </summary>
            /// <param name="response"></param>
            public void ShowGetObjectsResult(GetObjectsResponse response)
            {
                BitmapImage bm3 = new BitmapImage(new Uri(@"Resources\3.jpg", UriKind.Relative));
                ImageBrush brush3 = new ImageBrush(bm3);

                BitmapImage bm4 = new BitmapImage(new Uri(@"Resources\4.jpg", UriKind.Relative));
                ImageBrush brush4 = new ImageBrush(bm4);
                //Золотая шахта
                BitmapImage bm5 = new BitmapImage(new Uri(@"Resources\5.jpg", UriKind.RelativeOrAbsolute));
                ImageBrush brush5 = new ImageBrush(bm5);
                //Угольная шахта
                BitmapImage bm6 = new BitmapImage(new Uri(@"Resources\6.jpg", UriKind.RelativeOrAbsolute));
                ImageBrush brush6 = new ImageBrush(bm6);

                foreach (var item in response.Resources)
                {
                    if ((item.ResourceType == AliveChessLibrary.GameObjects.Resources.ResourceTypes.Gold) && (!collapsedResource.Contains(item.Id)))
                    {
                        rectArrGame[item.X, item.Y].Fill = brush3;
                    }
                    else if ((item.ResourceType == AliveChessLibrary.GameObjects.Resources.ResourceTypes.Wood)&& (!collapsedResource.Contains(item.Id)))
                    {
                        rectArrGame[item.X, item.Y].Fill = brush4;
                    }
                }
                foreach (var obj in _world.Map.Mines)
                {

                    if (obj.MineType == ResourceTypes.Gold)
                    {
                        rectArrGame[obj.X, obj.Y].Fill = brush5;
                    }
                    else if (obj.MineType == ResourceTypes.Coal)
                    {
                        rectArrGame[obj.X, obj.Y].Fill = brush6;
                    }
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
            if (rectIsInit == false)
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
