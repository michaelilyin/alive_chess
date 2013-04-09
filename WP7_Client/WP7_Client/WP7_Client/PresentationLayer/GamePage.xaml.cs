using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WP7_Client.NetLayer;
using WP7_Client.PresentationLayer.ConcreteScreens;
using WP7_Client.PresentationLayer.XNAScreenManager;

namespace WP7_Client.PresentationLayer
{
    public partial class GamePage : PhoneApplicationPage
    {
        public ContentManager ContentManager { get; set; }
        readonly GameTimer _timer;
        public SpriteBatch SpriteBatch { get; set; }
        private readonly ScreenManager _screenManager;
        private MenuScreen menu;
        public GamePage()
        {
            InitializeComponent();
            ContentManager = ((App)Application.Current).Content;
            _timer = new GameTimer {UpdateInterval = TimeSpan.FromTicks(333333)};
            _timer.Update += OnUpdate;
            _timer.Draw += OnDraw;
            _screenManager = new ScreenManager(this, SharedGraphicsDeviceManager.Current.GraphicsDevice);
            var bigMap = new BigMapScreen();
            ((App)Application.Current).Executor.CreateBigMapExecutors(bigMap);
            RequestManager.SendGetGameState();
            RequestManager.SendGetObjects();
            _screenManager.AddScreen(bigMap);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);
            SpriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
            Textures.Initiaize(this.ContentManager);
            _timer.Start();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _timer.Stop();
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);
            base.OnNavigatedFrom(e);
        }

        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            UpdateLayout();
            _screenManager.Update();
        }

        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.AliceBlue);
            _screenManager.Draw();
        }

        private void PhoneApplicationPageBackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            ShowMenu();
        }

        private void ShowMenu()
        {
            if (menu == null)
            {
                menu = new MenuScreen();
            }
            foreach (var screen in _screenManager.GetScreens().OfType<MenuScreen>())
            {
                _screenManager.RemoveScreen(screen);
                return;
            }
            _screenManager.AddScreen(menu);
        }
    }
}