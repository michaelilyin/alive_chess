using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using WindowsMobileClientAliveChess.NetLayer.Main;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.Commands.BigMapCommand;

namespace WindowsMobileClientAliveChess.GameLayer.Forms
{
    public partial class StartForm : Form
    {
        Game context;
        private object sync = new object();
        AuthorizeRequest authorize;
        delegate void mainThreadDelegate();
        
        public StartForm(Game context)
        {
            InitializeComponent();
            this.context = context;
            context.OnSuccesAuthorization += new AliveChessDelegate(context_OnSuccesAuthorization);
            context.OnGameReady += new AliveChessDelegate(context_OnGameReady);
            TextInit();
        }

        void context_OnGameReady()
        {
            BeginInvoke(new mainThreadDelegate(GameReady));
        }

        void context_OnSuccesAuthorization()
        {
  
            EnterGame();
        }

        private void GetTextes(Control c)
        {
            foreach (Control con in c.Controls)
            {
                con.Text = LanguageSwitcher.GetElementName(this.GetType(), con.Name);
                if (c.Controls.Count > 0)
                {
                    GetTextes(con);
                }
            }
        }

        private void TextInit()
        {
            this.Text = LanguageSwitcher.GetFormName(this.GetType());
            GetTextes(this);
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bPreferences_Click(object sender, EventArgs e)
        {
            new PreferencesForm(this).Show();
            this.Hide();
            this.Enabled = false;
        }

        private void EnterGame()
        {
            if (context.Authorized)
            {
              
                    GetGameStateRequest getState = new GetGameStateRequest();
                    ClientApplication.Instance.Transport.Send<GetGameStateRequest>(getState);
                
             
            }
        }
        private void GameReady()
        {
            if (context.Ready)
            {
                context.Player.Login = authorize.Login;
                context.Player.Password = authorize.Password;
                context.Run();
                this.Hide();
            }
            else
            {
                MessageBox.Show(LanguageSwitcher.GetExceptionMessage("GameCannotBeStarted"));
            }
        }
        private void bStartGame_Click(object sender, EventArgs e)
        {
            try
            {
                PreferencesForm.Preferences pref = LanguageSwitcher.ReadPreferenses(new PreferencesForm.Preferences());
                ClientApplication.CreateClient(context, pref.IP);
                if (ClientApplication.Instance.isStarted())
                    ClientApplication.Instance.StopClient();
                ClientApplication.Instance.StartClient();
                authorize = new AuthorizeRequest();
                authorize.Login = pref.Login;
                authorize.Password = pref.Password;
                Monitor.Enter(sync);

                ClientApplication.Instance.Transport.Send<AuthorizeRequest>(authorize);

            }
            catch (SocketException ex)
            {
                MessageBox.Show(LanguageSwitcher.GetExceptionMessage("SocketConnectionException"));
            }
        }

        private void StartForm_EnabledChanged(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                LanguageSwitcher.Initialize();
                TextInit();
            }
        }
    }
}