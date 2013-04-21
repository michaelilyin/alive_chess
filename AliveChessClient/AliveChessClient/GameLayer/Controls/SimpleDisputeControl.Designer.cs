namespace AliveChessClient.GameLayer.Controls
{
    partial class SimpleDisputeControl
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
            this.btn_agree = new System.Windows.Forms.Button();
            this.btn_refuse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btn_agree
            // 
            this.btn_agree.Location = new System.Drawing.Point(167, 51);
            this.btn_agree.Name = "btn_agree";
            this.btn_agree.Size = new System.Drawing.Size(119, 23);
            this.btn_agree.TabIndex = 0;
            this.btn_agree.Text = "Согласен";
            this.btn_agree.UseVisualStyleBackColor = true;
            // 
            // btn_refuse
            // 
            this.btn_refuse.Location = new System.Drawing.Point(167, 80);
            this.btn_refuse.Name = "btn_refuse";
            this.btn_refuse.Size = new System.Drawing.Size(119, 23);
            this.btn_refuse.TabIndex = 1;
            this.btn_refuse.Text = "Отказываюсь";
            this.btn_refuse.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(3, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 135);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Король";
            // 
            // SimpleDisputeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_refuse);
            this.Controls.Add(this.btn_agree);
            this.Name = "SimpleDisputeControl";
            this.Size = new System.Drawing.Size(290, 176);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_agree;
        private System.Windows.Forms.Button btn_refuse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Timer timer1;
    }
}
