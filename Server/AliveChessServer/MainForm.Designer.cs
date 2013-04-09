namespace AliveChessServer
{
    partial class MainForm
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.listBoxMessageLog = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBoxDatabase = new System.Windows.Forms.ComboBox();
            this.buttonInit = new System.Windows.Forms.Button();
            this.buttonSync = new System.Windows.Forms.Button();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonStart.Enabled = false;
            this.buttonStart.Location = new System.Drawing.Point(144, 295);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(128, 23);
            this.buttonStart.TabIndex = 6;
            this.buttonStart.Text = "Запуск";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBoxMessageLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.listBoxMessageLog.FormattingEnabled = true;
            this.listBoxMessageLog.Location = new System.Drawing.Point(10, 12);
            this.listBoxMessageLog.Name = "listBox1";
            this.listBoxMessageLog.Size = new System.Drawing.Size(452, 277);
            this.listBoxMessageLog.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(10, 324);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(128, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Загрузить чат";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBoxDatabase
            // 
            this.comboBoxDatabase.FormattingEnabled = true;
            this.comboBoxDatabase.Items.AddRange(new object[] {
            "Connect to local cache",
            "Connect to real database"});
            this.comboBoxDatabase.Location = new System.Drawing.Point(278, 297);
            this.comboBoxDatabase.Name = "comboBoxDatabase";
            this.comboBoxDatabase.Size = new System.Drawing.Size(184, 21);
            this.comboBoxDatabase.TabIndex = 8;
            // 
            // buttonInit
            // 
            this.buttonInit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonInit.Location = new System.Drawing.Point(10, 295);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(128, 23);
            this.buttonInit.TabIndex = 9;
            this.buttonInit.Text = "Инициализация";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonSync
            // 
            this.buttonSync.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonSync.Enabled = false;
            this.buttonSync.Location = new System.Drawing.Point(144, 324);
            this.buttonSync.Name = "buttonSync";
            this.buttonSync.Size = new System.Drawing.Size(128, 23);
            this.buttonSync.TabIndex = 10;
            this.buttonSync.Text = "Синхронизация";
            this.buttonSync.UseVisualStyleBackColor = true;
            this.buttonSync.Click += new System.EventHandler(this.buttonSync_Click);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(278, 324);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(184, 20);
            this.textBoxIP.TabIndex = 11;
            this.textBoxIP.Text = "127.0.0.1";
            this.textBoxIP.TextChanged += new System.EventHandler(this.textBoxIP_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 363);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.buttonSync);
            this.Controls.Add(this.buttonInit);
            this.Controls.Add(this.comboBoxDatabase);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.listBoxMessageLog);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ListBox listBoxMessageLog;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBoxDatabase;
        private System.Windows.Forms.Button buttonInit;
        private System.Windows.Forms.Button buttonSync;
        private System.Windows.Forms.TextBox textBoxIP;
    }
}

