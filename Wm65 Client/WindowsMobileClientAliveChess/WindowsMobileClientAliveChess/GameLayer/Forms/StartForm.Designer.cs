namespace WindowsMobileClientAliveChess.GameLayer.Forms
{
    partial class StartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.pBBackground = new System.Windows.Forms.PictureBox();
            this.bExit = new System.Windows.Forms.Button();
            this.bPreferences = new System.Windows.Forms.Button();
            this.bStartGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pBBackground
            // 
            this.pBBackground.Image = ((System.Drawing.Image)(resources.GetObject("pBBackground.Image")));
            this.pBBackground.Location = new System.Drawing.Point(0, 0);
            this.pBBackground.Name = "pBBackground";
            this.pBBackground.Size = new System.Drawing.Size(240, 294);
            // 
            // bExit
            // 
            this.bExit.BackColor = System.Drawing.Color.White;
            this.bExit.Location = new System.Drawing.Point(8, 240);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(224, 48);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "Exit";
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bPreferences
            // 
            this.bPreferences.BackColor = System.Drawing.Color.White;
            this.bPreferences.Location = new System.Drawing.Point(8, 186);
            this.bPreferences.Name = "bPreferences";
            this.bPreferences.Size = new System.Drawing.Size(224, 48);
            this.bPreferences.TabIndex = 2;
            this.bPreferences.Text = "Preferences";
            this.bPreferences.Click += new System.EventHandler(this.bPreferences_Click);
            // 
            // bStartGame
            // 
            this.bStartGame.BackColor = System.Drawing.Color.White;
            this.bStartGame.Location = new System.Drawing.Point(8, 132);
            this.bStartGame.Name = "bStartGame";
            this.bStartGame.Size = new System.Drawing.Size(224, 48);
            this.bStartGame.TabIndex = 3;
            this.bStartGame.Text = "Start";
            this.bStartGame.Click += new System.EventHandler(this.bStartGame_Click);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.bStartGame);
            this.Controls.Add(this.bPreferences);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.pBBackground);
            this.Name = "StartForm";
            this.Text = "Welcome";
            this.EnabledChanged += new System.EventHandler(this.StartForm_EnabledChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBBackground;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button bPreferences;
        private System.Windows.Forms.Button bStartGame;
    }
}