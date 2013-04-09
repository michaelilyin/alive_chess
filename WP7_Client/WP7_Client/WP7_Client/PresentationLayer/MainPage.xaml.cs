using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using WP7_Client.NetLayer;

namespace WP7_Client.PresentationLayer
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Login.Text = (string)App.Settings[App.Login];
            Password.Password = (string)App.Settings[App.Pass];
            Remember.IsChecked = (bool)App.Settings[App.RememberPass];
        }

        private void PhoneApplicationPageLoaded(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigating += NavigateRequest;
        }

        private void NavigateRequest(object sender, EventArgs e)
        {
            App.Settings[App.Login] = Login.Text;
            App.Settings[App.Pass] = Password.Password;
        }

        private void ButtonSettings(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PresentationLayer/Settings.xaml", UriKind.Relative));
        }

        private void ButtonStart(object sender, RoutedEventArgs e)
        {
            App.Settings[App.Login] = Login.Text;
            App.Settings[App.Pass] = Password.Password;
            if (!((App)Application.Current).Transport.Connected())
            {
                if (!((App)Application.Current).Transport.Connect((string)App.Settings[App.Address], (int)App.Settings[App.Port]))
                {
                    MessageBox.Show("Unable to connect to remote host" + ((App)Application.Current).Transport.Error, "Sorry", MessageBoxButton.OK);
                    return;
                }
                ((App)Application.Current).Executor.Start();
            }
            RequestManager.SendAuthorization((string)App.Settings[App.Login], (string)App.Settings[App.Pass]);
        }

        private void RememberClick(object sender, RoutedEventArgs e)
        {
            App.Settings[App.RememberPass] = ((CheckBox)sender).IsChecked;
        }
    }
}