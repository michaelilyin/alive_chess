using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WP7_Client.NetLayer;

namespace WP7_Client.PresentationLayer
{
    public partial class Settings : PhoneApplicationPage
    {
        private readonly Brush _defTextBoxBrush;
        private readonly Brush _invalidTextBoxBrush = new SolidColorBrush(Color.FromArgb(0xee, 0xff, 0, 0));

        public Settings()
        {
            InitializeComponent();
            Host.Text = (string) App.Settings[App.Address];
            Port.Text = App.Settings[App.Port].ToString();
            _defTextBoxBrush = Port.Background;
        }

        private void SaveClick(object sender, EventArgs e)
        {
            App.Settings[App.Address] = Host.Text;
            App.Settings[App.Port] = int.Parse(Port.Text);
            NavigationService.GoBack();
        }

        private void PortTextChanged(object sender, TextChangedEventArgs e)
        {
            int port;
            if (!int.TryParse(Port.Text, out port))
            {
                Port.Background = _invalidTextBoxBrush;
                ((ApplicationBarIconButton) ApplicationBar.Buttons[0]).IsEnabled = false;
            }
            else
            {
                Port.Background = _defTextBoxBrush;
                ((ApplicationBarIconButton) ApplicationBar.Buttons[0]).IsEnabled = true;
            }
        }

        private void BRegister_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!((App)Application.Current).Transport.Connected())
            {
                if (!((App)Application.Current).Transport.Connect((string)App.Settings[App.Address], (int)App.Settings[App.Port]))
                {
                    MessageBox.Show("Unable to connect to remote host" + ((App)Application.Current).Transport.Error, "Sorry", MessageBoxButton.OK);
                    return;
                }
                ((App)Application.Current).Executor.Start();
            }
            RequestManager.SendRegistration(Login.Text, Password.Password);
        }
    }
}