namespace AliveChessClient.GameLayer.Controls
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
            this.components = new System.ComponentModel.Container();
            this.label_castle = new System.Windows.Forms.Label();
            this.label_mine = new System.Windows.Forms.Label();
            this.label_timber = new System.Windows.Forms.Label();
            this.label_gold = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label_name = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label_castle
            // 
            this.label_castle.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.label_castle.AutoSize = true;
            this.label_castle.Font = new System.Drawing.Font("Monotype Corsiva", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_castle.Location = new System.Drawing.Point(3, 155);
            this.label_castle.Name = "label_castle";
            this.label_castle.Size = new System.Drawing.Size(48, 15);
            this.label_castle.TabIndex = 12;
            this.label_castle.Text = "Замки:";
            // 
            // label_mine
            // 
            this.label_mine.AutoSize = true;
            this.label_mine.Font = new System.Drawing.Font("Monotype Corsiva", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_mine.Location = new System.Drawing.Point(3, 128);
            this.label_mine.Name = "label_mine";
            this.label_mine.Size = new System.Drawing.Size(54, 15);
            this.label_mine.TabIndex = 11;
            this.label_mine.Text = "Шахты:";
            // 
            // label_timber
            // 
            this.label_timber.AutoSize = true;
            this.label_timber.Font = new System.Drawing.Font("Monotype Corsiva", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_timber.Location = new System.Drawing.Point(3, 103);
            this.label_timber.Name = "label_timber";
            this.label_timber.Size = new System.Drawing.Size(69, 15);
            this.label_timber.TabIndex = 10;
            this.label_timber.Text = "Древесина:";
            // 
            // label_gold
            // 
            this.label_gold.AutoSize = true;
            this.label_gold.Font = new System.Drawing.Font("Monotype Corsiva", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_gold.Location = new System.Drawing.Point(3, 77);
            this.label_gold.Name = "label_gold";
            this.label_gold.Size = new System.Drawing.Size(56, 15);
            this.label_gold.TabIndex = 9;
            this.label_gold.Text = "Золото:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(182, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(333, 408);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown_1);
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Font = new System.Drawing.Font("Monotype Corsiva", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_name.Location = new System.Drawing.Point(3, 29);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(83, 15);
            this.label_name.TabIndex = 7;
            this.label_name.Text = "Имя короля:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 209);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Ваш союз";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 238);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(170, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Союзы на карте";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 267);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(170, 134);
            this.listBox1.TabIndex = 16;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 414);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(170, 23);
            this.button3.TabIndex = 17;
            this.button3.Text = "Войти в союз";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Monotype Corsiva", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 15);
            this.label2.TabIndex = 19;
            this.label2.Text = "ID короля:";
            // 
            // BigMapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label_castle);
            this.Controls.Add(this.label_mine);
            this.Controls.Add(this.label_timber);
            this.Controls.Add(this.label_gold);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label_name);
            this.Name = "BigMapControl";
            this.Size = new System.Drawing.Size(521, 449);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_castle;
        private System.Windows.Forms.Label label_mine;
        private System.Windows.Forms.Label label_timber;
        private System.Windows.Forms.Label label_gold;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        public System.Windows.Forms.Button Button1
        {
            get { return button1; }
            set { button1 = value; }
        }

    }
}
