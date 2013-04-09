using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WP7_Client.NetLayer;
using WP7_Client.LogicLayer;
using WP7_Client.PresentationLayer;

namespace WP7_Client
{
    public delegate void AliveChessDelegate();
    public delegate void AliveChessErrorDelegate(string msg);

    public partial class App : Application
    {
        public const string Login = "Login";
        public const string Pass = "Password";
        public const string Address = "Address";
        public const string Port = "Port";
        public const string RememberPass = "Remember";

        public static IsolatedStorageSettings Settings { get; private set; }
        public PhoneApplicationFrame RootFrame { get; private set; }
        public ContentManager Content { get; private set; }
        public GameTimer FrameworkDispatcherTimer { get; private set; }
        public AppServiceProvider Services { get; private set; }

        public CommandPool Pool;
        public SocketTransport Transport;
        public RequestExecutor Executor;
        public GameWorld World { get; set; }
        public event AliveChessErrorDelegate OnErrorMessage;
        public event AliveChessDelegate OnAuthorize;

        public App()
        {
            World = new GameWorld();
            Pool = new CommandPool();
            Transport = new SocketTransport(new Logger(), Pool);
            Executor = new RequestExecutor(Pool);
            Settings = IsolatedStorageSettings.ApplicationSettings;

            if (Settings.Count == 0)
            {
                Settings.Add(Login, "player");
                Settings.Add(Pass, "pw");
                Settings.Add(Address, "localhost");
                Settings.Add(Port, 22000);
                Settings.Add(RememberPass, true);
            }
            UnhandledException += Application_UnhandledException;
            Exit += Application_Exit;
            OnErrorMessage += ShErrMsg;
            OnAuthorize += Authorize;
            InitializeComponent();
            InitializePhoneApplication();
            InitializeXnaApplication();
            if (!System.Diagnostics.Debugger.IsAttached) return;
            Current.Host.Settings.EnableFrameRateCounter = true;
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        public void ShErrMsg(string msg)
        {
            MessageBox.Show(msg);
        }

        public void Authorize()
        {
            RequestManager.SendGetMap();
        }

        public void OnGetBigMap()
        {
            ((PhoneApplicationFrame)RootVisual).Navigate(new Uri("/PresentationLayer/GamePage.xaml", UriKind.Relative));
        }

        private void Application_Exit(object sender, EventArgs e)
        {
            if (!(bool)Settings[RememberPass])
            {
                Settings[Pass] = "";
            }
            Settings.Save();
        }

        private void ApplicationLaunching(object sender, LaunchingEventArgs e)
        {

        }

        private void ApplicationActivated(object sender, ActivatedEventArgs e)
        {
        }

        private void ApplicationDeactivated(object sender, DeactivatedEventArgs e)
        {
        }

        private void ApplicationClosing(object sender, ClosingEventArgs e)
        {
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is ExitException)
            {
                return;
            }
            MessageBox.Show(e.ExceptionObject.Message);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization
        private bool _phoneApplicationInitialized;

        private void InitializePhoneApplication()
        {
            if (_phoneApplicationInitialized)
                return;
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;
            _phoneApplicationInitialized = true;
        }

        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            RootVisual = RootFrame;
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

        #region XNA application initialization

        private void InitializeXnaApplication()
        {
            Services = new AppServiceProvider();

            foreach (var obj in ApplicationLifetimeObjects.OfType<IGraphicsDeviceService>())
            {
                Services.AddService(typeof(IGraphicsDeviceService), obj);
            }

            Content = new ContentManager(Services, "Content");
            FrameworkDispatcherTimer = new GameTimer();
            FrameworkDispatcherTimer.FrameAction += FrameworkDispatcherFrameAction;
            FrameworkDispatcherTimer.Start();
        }
        private void FrameworkDispatcherFrameAction(object sender, EventArgs e)
        {
            FrameworkDispatcher.Update();
        }

        #endregion

        public void HandleAuthorizeInvocation()
        {
            OnAuthorize();
        }

        public void HandleError(string msg)
        {
            OnErrorMessage(msg);
        }

        public void ExitNow()
        {
            throw new ExitException();
        }
    }

    public class ExitException : Exception{}
}