namespace WindowsMobileClientAliveChess.GameLayer.Forms
{
    partial class PreferencesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mMPreferences;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mMPreferences = new System.Windows.Forms.MainMenu();
            this.mIAccept = new System.Windows.Forms.MenuItem();
            this.mICancel = new System.Windows.Forms.MenuItem();
            this.tCPreferences = new System.Windows.Forms.TabControl();
            this.tPGeneral = new System.Windows.Forms.TabPage();
            this.cBLanguage = new System.Windows.Forms.ComboBox();
            this.lLanguage = new System.Windows.Forms.Label();
            this.tPConnection = new System.Windows.Forms.TabPage();
            this.lPrivilegies = new System.Windows.Forms.Label();
            this.lLogin = new System.Windows.Forms.Label();
            this.lPort = new System.Windows.Forms.Label();
            this.lIP = new System.Windows.Forms.Label();
            this.cBPrivilegies = new System.Windows.Forms.ComboBox();
            this.tBLogin = new System.Windows.Forms.TextBox();
            this.tBPort = new System.Windows.Forms.TextBox();
            this.tBIP = new System.Windows.Forms.TextBox();
            this.lPassword = new System.Windows.Forms.Label();
            this.tBPassword = new System.Windows.Forms.TextBox();
            this.tCPreferences.SuspendLayout();
            this.tPGeneral.SuspendLayout();
            this.tPConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // mMPreferences
            // 
            this.mMPreferences.MenuItems.Add(this.mIAccept);
            this.mMPreferences.MenuItems.Add(this.mICancel);
            // 
            // mIAccept
            // 
            this.mIAccept.Text = "Accept";
            this.mIAccept.Click += new System.EventHandler(this.mIAccept_Click);
            // 
            // mICancel
            // 
            this.mICancel.Text = "Cancel";
            this.mICancel.Click += new System.EventHandler(this.mICancel_Click);
            // 
            // tCPreferences
            // 
            this.tCPreferences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tCPreferences.Controls.Add(this.tPGeneral);
            this.tCPreferences.Controls.Add(this.tPConnection);
            this.tCPreferences.Dock = System.Windows.Forms.DockStyle.None;
            this.tCPreferences.Location = new System.Drawing.Point(0, 0);
            this.tCPreferences.Name = "tCPreferences";
            this.tCPreferences.SelectedIndex = 0;
            this.tCPreferences.Size = new System.Drawing.Size(240, 268);
            this.tCPreferences.TabIndex = 0;
            // 
            // tPGeneral
            // 
            this.tPGeneral.Controls.Add(this.cBLanguage);
            this.tPGeneral.Controls.Add(this.lLanguage);
            this.tPGeneral.Location = new System.Drawing.Point(0, 0);
            this.tPGeneral.Name = "tPGeneral";
            this.tPGeneral.Size = new System.Drawing.Size(240, 245);
            this.tPGeneral.Text = "General";
            // 
            // cBLanguage
            // 
            this.cBLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cBLanguage.Location = new System.Drawing.Point(99, 8);
            this.cBLanguage.Name = "cBLanguage";
            this.cBLanguage.Size = new System.Drawing.Size(134, 22);
            this.cBLanguage.TabIndex = 1;
            // 
            // lLanguage
            // 
            this.lLanguage.Location = new System.Drawing.Point(7, 10);
            this.lLanguage.Name = "lLanguage";
            this.lLanguage.Size = new System.Drawing.Size(100, 20);
            this.lLanguage.Text = "Language";
            // 
            // tPConnection
            // 
            this.tPConnection.Controls.Add(this.tBPassword);
            this.tPConnection.Controls.Add(this.lPassword);
            this.tPConnection.Controls.Add(this.lPrivilegies);
            this.tPConnection.Controls.Add(this.lLogin);
            this.tPConnection.Controls.Add(this.lPort);
            this.tPConnection.Controls.Add(this.lIP);
            this.tPConnection.Controls.Add(this.cBPrivilegies);
            this.tPConnection.Controls.Add(this.tBLogin);
            this.tPConnection.Controls.Add(this.tBPort);
            this.tPConnection.Controls.Add(this.tBIP);
            this.tPConnection.Location = new System.Drawing.Point(0, 0);
            this.tPConnection.Name = "tPConnection";
            this.tPConnection.Size = new System.Drawing.Size(240, 245);
            this.tPConnection.Text = "Connection";
            // 
            // lPrivilegies
            // 
            this.lPrivilegies.Location = new System.Drawing.Point(8, 122);
            this.lPrivilegies.Name = "lPrivilegies";
            this.lPrivilegies.Size = new System.Drawing.Size(85, 20);
            this.lPrivilegies.Text = "Privilegies";
            // 
            // lLogin
            // 
            this.lLogin.Location = new System.Drawing.Point(8, 63);
            this.lLogin.Name = "lLogin";
            this.lLogin.Size = new System.Drawing.Size(85, 20);
            this.lLogin.Text = "Login";
            // 
            // lPort
            // 
            this.lPort.Location = new System.Drawing.Point(8, 36);
            this.lPort.Name = "lPort";
            this.lPort.Size = new System.Drawing.Size(85, 20);
            this.lPort.Text = "Port";
            // 
            // lIP
            // 
            this.lIP.Location = new System.Drawing.Point(8, 8);
            this.lIP.Name = "lIP";
            this.lIP.Size = new System.Drawing.Size(85, 20);
            this.lIP.Text = "IP";
            // 
            // cBPrivilegies
            // 
            this.cBPrivilegies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cBPrivilegies.Location = new System.Drawing.Point(99, 120);
            this.cBPrivilegies.Name = "cBPrivilegies";
            this.cBPrivilegies.Size = new System.Drawing.Size(133, 22);
            this.cBPrivilegies.TabIndex = 3;
            // 
            // tBLogin
            // 
            this.tBLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tBLogin.Location = new System.Drawing.Point(99, 62);
            this.tBLogin.Name = "tBLogin";
            this.tBLogin.Size = new System.Drawing.Size(134, 21);
            this.tBLogin.TabIndex = 2;
            // 
            // tBPort
            // 
            this.tBPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tBPort.Location = new System.Drawing.Point(99, 35);
            this.tBPort.Name = "tBPort";
            this.tBPort.Size = new System.Drawing.Size(134, 21);
            this.tBPort.TabIndex = 1;
            // 
            // tBIP
            // 
            this.tBIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tBIP.Location = new System.Drawing.Point(99, 8);
            this.tBIP.Name = "tBIP";
            this.tBIP.Size = new System.Drawing.Size(134, 21);
            this.tBIP.TabIndex = 0;
            // 
            // lPassword
            // 
            this.lPassword.Location = new System.Drawing.Point(8, 92);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new System.Drawing.Size(85, 20);
            this.lPassword.Text = "Password";
            // 
            // tBPassword
            // 
            this.tBPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tBPassword.Location = new System.Drawing.Point(98, 92);
            this.tBPassword.Name = "tBPassword";
            this.tBPassword.Size = new System.Drawing.Size(134, 21);
            this.tBPassword.TabIndex = 7;
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.tCPreferences);
            this.Menu = this.mMPreferences;
            this.Name = "PreferencesForm";
            this.Text = "PreferencesForm";
            this.Load += new System.EventHandler(this.PreferencesForm_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.PreferencesForm_Closing);
            this.tCPreferences.ResumeLayout(false);
            this.tPGeneral.ResumeLayout(false);
            this.tPConnection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mIAccept;
        private System.Windows.Forms.MenuItem mICancel;
        private System.Windows.Forms.TabControl tCPreferences;
        private System.Windows.Forms.TabPage tPGeneral;
        private System.Windows.Forms.Label lLanguage;
        private System.Windows.Forms.TabPage tPConnection;
        private System.Windows.Forms.ComboBox cBLanguage;
        private System.Windows.Forms.Label lPrivilegies;
        private System.Windows.Forms.Label lLogin;
        private System.Windows.Forms.Label lPort;
        private System.Windows.Forms.Label lIP;
        private System.Windows.Forms.ComboBox cBPrivilegies;
        private System.Windows.Forms.TextBox tBLogin;
        private System.Windows.Forms.TextBox tBPort;
        private System.Windows.Forms.TextBox tBIP;
        private System.Windows.Forms.TextBox tBPassword;
        private System.Windows.Forms.Label lPassword;
    }
}