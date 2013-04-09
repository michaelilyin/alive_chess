using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WindowsMobileClientAliveChess.GameLayer.Controls
{
    public partial class AlianceControl : UserControl
    {
        private Game context;

        public AlianceControl(Game context)
        {
            InitializeComponent();
            this.context = context;
        }

        private void TextInit()
        {
            this.Text = LanguageSwitcher.GetFormName(this.GetType());
            GetTextes(this);
        }
        private void GetTextes(Control c)
        {
            foreach (Control con in c.Controls)
            {
                if (con is Label || con is TabPage)
                {
                    con.Text = LanguageSwitcher.GetElementName(this.GetType(), con.Name);
                }
                else
                {
                    con.Text = LanguageSwitcher.GetElementName(this.GetType(), con.Text);
                }
                if (c.Controls.Count > 0)
                {
                    GetTextes(con);
                }
            }
        }

        public void Initialize()
        {
            TextInit();
        }
    }
}
