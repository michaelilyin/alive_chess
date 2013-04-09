using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace WindowsMobileClientAliveChess.GameLayer.Forms
{
    public partial class PreferencesForm : Form
    {
        Form parent;
        public struct Preferences
        {
            public string[] Languages;
            public string IP;
            public string Port;
            public string Password;
            public string Login;
            public string[] Privilegies;
            public int priv_selected;
            public int lang_selected;
        }

        public PreferencesForm(Form parent)
        {
            InitializeComponent();
            this.parent = parent;
            TextInit();
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
            Preferences pref = new Preferences();
            pref = LanguageSwitcher.ReadPreferenses(pref);
            tBIP.Text = pref.IP;
            tBPort.Text = pref.Port;
            tBLogin.Text = pref.Login;
            tBPassword.Text = pref.Password;
            for (int i = 0; i < pref.Languages.Length; i++)
                cBLanguage.Items.Add(pref.Languages[i]);
            for (int i = 0; i < pref.Privilegies.Length; i++)
                cBPrivilegies.Items.Add(pref.Privilegies[i]);
            cBLanguage.SelectedIndex = pref.lang_selected;
            cBPrivilegies.SelectedIndex = pref.priv_selected;
            tBIP.Text = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
        }

        private void PreferencesForm_Closing(object sender, CancelEventArgs e)
        {
            parent.Show();
            parent.Enabled = true;
        }

        private void mICancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PreferencesForm_Load(object sender, EventArgs e)
        {

        }

        private void mIAccept_Click(object sender, EventArgs e)
        {
            Preferences pref = new Preferences();
            pref.IP = tBIP.Text;
            pref.Port = tBPort.Text;
            pref.Login = tBLogin.Text;
            pref.Password = tBPassword.Text;
            pref.Privilegies = new string[cBPrivilegies.Items.Count];
            for (int i = 0; i < cBPrivilegies.Items.Count; i++)
            {
                pref.Privilegies[i] = cBPrivilegies.Items[i].ToString();
            }
            pref.Languages = new string[cBLanguage.Items.Count];
            for (int i = 0; i < cBLanguage.Items.Count; i++)
            {
                pref.Languages[i] = cBLanguage.Items[i].ToString();
            }
            pref.lang_selected = cBLanguage.SelectedIndex;
            pref.priv_selected = cBPrivilegies.SelectedIndex;
            LanguageSwitcher.SetNewPreferences(pref);
            Close();
        }
    }
}