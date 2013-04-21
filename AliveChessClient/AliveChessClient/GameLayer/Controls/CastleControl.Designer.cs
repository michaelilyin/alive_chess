namespace AliveChessClient.GameLayer.Controls
{
    partial class CastleControl
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
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonGetArmyToKing = new System.Windows.Forms.Button();
            this.comboBoxKingArmy = new System.Windows.Forms.ComboBox();
            this.buttonKing = new System.Windows.Forms.Button();
            this.buttonArmyCastle = new System.Windows.Forms.Button();
            this.comboBoxArmy = new System.Windows.Forms.ComboBox();
            this.buttonHireUnit = new System.Windows.Forms.Button();
            this.textBoxCountUnit = new System.Windows.Forms.TextBox();
            this.buttonListBuildings = new System.Windows.Forms.Button();
            this.comboBoxBuild = new System.Windows.Forms.ComboBox();
            this.labelBuildingsInCastle = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button_Buildings = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(651, 498);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(723, 489);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // buttonGetArmyToKing
            // 
            this.buttonGetArmyToKing.Location = new System.Drawing.Point(29, 205);
            this.buttonGetArmyToKing.Name = "buttonGetArmyToKing";
            this.buttonGetArmyToKing.Size = new System.Drawing.Size(121, 23);
            this.buttonGetArmyToKing.TabIndex = 30;
            this.buttonGetArmyToKing.Text = "Забрать армию";
            this.buttonGetArmyToKing.UseVisualStyleBackColor = true;
            this.buttonGetArmyToKing.Click += new System.EventHandler(this.buttonGetArmyToKing_Click);
            // 
            // comboBoxKingArmy
            // 
            this.comboBoxKingArmy.FormattingEnabled = true;
            this.comboBoxKingArmy.Location = new System.Drawing.Point(201, 388);
            this.comboBoxKingArmy.Name = "comboBoxKingArmy";
            this.comboBoxKingArmy.Size = new System.Drawing.Size(204, 21);
            this.comboBoxKingArmy.TabIndex = 29;
            // 
            // buttonKing
            // 
            this.buttonKing.Location = new System.Drawing.Point(234, 348);
            this.buttonKing.Name = "buttonKing";
            this.buttonKing.Size = new System.Drawing.Size(121, 23);
            this.buttonKing.TabIndex = 28;
            this.buttonKing.Text = "Армия короля!";
            this.buttonKing.UseVisualStyleBackColor = true;
            this.buttonKing.Click += new System.EventHandler(this.buttonKing_Click);
            // 
            // buttonArmyCastle
            // 
            this.buttonArmyCastle.Location = new System.Drawing.Point(481, 348);
            this.buttonArmyCastle.Name = "buttonArmyCastle";
            this.buttonArmyCastle.Size = new System.Drawing.Size(121, 25);
            this.buttonArmyCastle.TabIndex = 27;
            this.buttonArmyCastle.Text = "армия замка";
            this.buttonArmyCastle.UseVisualStyleBackColor = true;
            this.buttonArmyCastle.Click += new System.EventHandler(this.buttonArmyCastle_Click);
            // 
            // comboBoxArmy
            // 
            this.comboBoxArmy.FormattingEnabled = true;
            this.comboBoxArmy.Location = new System.Drawing.Point(440, 388);
            this.comboBoxArmy.Name = "comboBoxArmy";
            this.comboBoxArmy.Size = new System.Drawing.Size(195, 21);
            this.comboBoxArmy.TabIndex = 26;
            // 
            // buttonHireUnit
            // 
            this.buttonHireUnit.Location = new System.Drawing.Point(29, 178);
            this.buttonHireUnit.Name = "buttonHireUnit";
            this.buttonHireUnit.Size = new System.Drawing.Size(121, 21);
            this.buttonHireUnit.TabIndex = 25;
            this.buttonHireUnit.Text = "Нанять";
            this.buttonHireUnit.UseVisualStyleBackColor = true;
            this.buttonHireUnit.Click += new System.EventHandler(this.buttonHireUnit_Click);
            // 
            // textBoxCountUnit
            // 
            this.textBoxCountUnit.Location = new System.Drawing.Point(29, 151);
            this.textBoxCountUnit.Name = "textBoxCountUnit";
            this.textBoxCountUnit.Size = new System.Drawing.Size(121, 20);
            this.textBoxCountUnit.TabIndex = 24;
            // 
            // buttonListBuildings
            // 
            this.buttonListBuildings.Location = new System.Drawing.Point(29, 97);
            this.buttonListBuildings.Name = "buttonListBuildings";
            this.buttonListBuildings.Size = new System.Drawing.Size(121, 21);
            this.buttonListBuildings.TabIndex = 23;
            this.buttonListBuildings.Text = "Список зданий";
            this.buttonListBuildings.UseVisualStyleBackColor = true;
            this.buttonListBuildings.Click += new System.EventHandler(this.buttonListBuildings_Click);
            // 
            // comboBoxBuild
            // 
            this.comboBoxBuild.FormattingEnabled = true;
            this.comboBoxBuild.Location = new System.Drawing.Point(29, 124);
            this.comboBoxBuild.Name = "comboBoxBuild";
            this.comboBoxBuild.Size = new System.Drawing.Size(121, 21);
            this.comboBoxBuild.TabIndex = 22;
            // 
            // labelBuildingsInCastle
            // 
            this.labelBuildingsInCastle.AutoSize = true;
            this.labelBuildingsInCastle.Location = new System.Drawing.Point(26, 12);
            this.labelBuildingsInCastle.Name = "labelBuildingsInCastle";
            this.labelBuildingsInCastle.Size = new System.Drawing.Size(44, 13);
            this.labelBuildingsInCastle.TabIndex = 21;
            this.labelBuildingsInCastle.Text = "Здания";
            this.labelBuildingsInCastle.Click += new System.EventHandler(this.labelBuildingsInCastle_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Военкомат",
            "Конюшня",
            "Школа Офицеров",
            "Генеральный штаб",
            "ВВУ"});
            this.comboBox1.Location = new System.Drawing.Point(29, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 20;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button_Buildings
            // 
            this.button_Buildings.Location = new System.Drawing.Point(29, 68);
            this.button_Buildings.Name = "button_Buildings";
            this.button_Buildings.Size = new System.Drawing.Size(121, 23);
            this.button_Buildings.TabIndex = 19;
            this.button_Buildings.Text = "Построить";
            this.button_Buildings.UseVisualStyleBackColor = true;
            this.button_Buildings.Click += new System.EventHandler(this.button_Buildings_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Ресурсы";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(560, 498);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 17;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // CastleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonGetArmyToKing);
            this.Controls.Add(this.comboBoxKingArmy);
            this.Controls.Add(this.buttonKing);
            this.Controls.Add(this.buttonArmyCastle);
            this.Controls.Add(this.comboBoxArmy);
            this.Controls.Add(this.buttonHireUnit);
            this.Controls.Add(this.textBoxCountUnit);
            this.Controls.Add(this.buttonListBuildings);
            this.Controls.Add(this.comboBoxBuild);
            this.Controls.Add(this.labelBuildingsInCastle);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button_Buildings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Name = "CastleControl";
            this.Size = new System.Drawing.Size(731, 544);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonGetArmyToKing;
        private System.Windows.Forms.ComboBox comboBoxKingArmy;
        private System.Windows.Forms.Button buttonKing;
        private System.Windows.Forms.Button buttonArmyCastle;
        private System.Windows.Forms.ComboBox comboBoxArmy;
        private System.Windows.Forms.Button buttonHireUnit;
        private System.Windows.Forms.TextBox textBoxCountUnit;
        private System.Windows.Forms.Button buttonListBuildings;
        private System.Windows.Forms.ComboBox comboBoxBuild;
        private System.Windows.Forms.Label labelBuildingsInCastle;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button_Buildings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;

    }
}
