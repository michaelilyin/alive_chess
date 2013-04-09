namespace WindowsMobileClientAliveChess.GameLayer.Controls
{
    partial class BigMapControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pBMap = new System.Windows.Forms.PictureBox();
            this.pMenu = new System.Windows.Forms.Panel();
            this.pBMenu = new System.Windows.Forms.PictureBox();
            this.pBGold = new System.Windows.Forms.PictureBox();
            this.cMMain = new System.Windows.Forms.ContextMenu();
            this.mIAlliences = new System.Windows.Forms.MenuItem();
            this.mIYours = new System.Windows.Forms.MenuItem();
            this.mIOnMap = new System.Windows.Forms.MenuItem();
            this.mIJoin = new System.Windows.Forms.MenuItem();
            this.mIExit = new System.Windows.Forms.MenuItem();
            this.tGameTime = new System.Windows.Forms.Timer();
            this.pMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBMap
            // 
            this.pBMap.Location = new System.Drawing.Point(0, 0);
            this.pBMap.Name = "pBMap";
            this.pBMap.Size = new System.Drawing.Size(240, 240);
            this.pBMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pBMap_MouseMove);
            this.pBMap.Click += new System.EventHandler(this.pBMap_Click);
            this.pBMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pBMap_MouseDown);
            this.pBMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pBMap_MouseClick);
            // 
            // pMenu
            // 
            this.pMenu.Controls.Add(this.pBMenu);
            this.pMenu.Controls.Add(this.pBGold);
            this.pMenu.Location = new System.Drawing.Point(0, 239);
            this.pMenu.Name = "pMenu";
            this.pMenu.Size = new System.Drawing.Size(240, 54);
            // 
            // pBMenu
            // 
            this.pBMenu.Location = new System.Drawing.Point(187, 3);
            this.pBMenu.Name = "pBMenu";
            this.pBMenu.Size = new System.Drawing.Size(50, 48);
            this.pBMenu.Click += new System.EventHandler(this.pBMenu_Click);
            // 
            // pBGold
            // 
            this.pBGold.Location = new System.Drawing.Point(3, 3);
            this.pBGold.Name = "pBGold";
            this.pBGold.Size = new System.Drawing.Size(178, 48);
            // 
            // cMMain
            // 
            this.cMMain.MenuItems.Add(this.mIAlliences);
            this.cMMain.MenuItems.Add(this.mIExit);
            // 
            // mIAlliences
            // 
            this.mIAlliences.MenuItems.Add(this.mIYours);
            this.mIAlliences.MenuItems.Add(this.mIOnMap);
            this.mIAlliences.MenuItems.Add(this.mIJoin);
            this.mIAlliences.Text = "Alliances";
            // 
            // mIYours
            // 
            this.mIYours.Text = "Yours";
            // 
            // mIOnMap
            // 
            this.mIOnMap.Text = "AlliancesOnMap";
            this.mIOnMap.Click += new System.EventHandler(this.mIOnMap_Click);
            // 
            // mIJoin
            // 
            this.mIJoin.Text = "Join";
            // 
            // mIExit
            // 
            this.mIExit.Text = "Exit";
            this.mIExit.Click += new System.EventHandler(this.mIExit_Click);
            // 
            // BigMapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pMenu);
            this.Controls.Add(this.pBMap);
            this.Name = "BigMapControl";
            this.Size = new System.Drawing.Size(240, 294);
            this.pMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBMap;
        private System.Windows.Forms.Panel pMenu;
        private System.Windows.Forms.PictureBox pBGold;
        private System.Windows.Forms.PictureBox pBMenu;
        private System.Windows.Forms.ContextMenu cMMain;
        private System.Windows.Forms.MenuItem mIAlliences;
        private System.Windows.Forms.MenuItem mIYours;
        private System.Windows.Forms.MenuItem mIOnMap;
        private System.Windows.Forms.MenuItem mIJoin;
        private System.Windows.Forms.MenuItem mIExit;
        private System.Windows.Forms.Timer tGameTime;
    }
}
