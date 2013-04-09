namespace WindowsMobileClientAliveChess.GameLayer.Controls
{
    partial class AlianceControl
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
            this.pBReturn = new System.Windows.Forms.PictureBox();
            this.tCAlliance = new System.Windows.Forms.TabControl();
            this.tPAlliance = new System.Windows.Forms.TabPage();
            this.bStartImpeachment = new System.Windows.Forms.Button();
            this.bSupportLeader = new System.Windows.Forms.Button();
            this.bTryTakeLeadership = new System.Windows.Forms.Button();
            this.bRuleEmpire = new System.Windows.Forms.Button();
            this.pBExitAlliance = new System.Windows.Forms.PictureBox();
            this.tPSendDemand = new System.Windows.Forms.TabPage();
            this.cBReciever = new System.Windows.Forms.ComboBox();
            this.bRequestHelp = new System.Windows.Forms.Button();
            this.bSend = new System.Windows.Forms.Button();
            this.nUDUnits = new System.Windows.Forms.NumericUpDown();
            this.nUDResources = new System.Windows.Forms.NumericUpDown();
            this.cBUnits = new System.Windows.Forms.ComboBox();
            this.cBRecources = new System.Windows.Forms.ComboBox();
            this.cMHelp = new System.Windows.Forms.ContextMenu();
            this.mIRes = new System.Windows.Forms.MenuItem();
            this.mIUnits = new System.Windows.Forms.MenuItem();
            this.tCAlliance.SuspendLayout();
            this.tPAlliance.SuspendLayout();
            this.tPSendDemand.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBReturn
            // 
            this.pBReturn.Location = new System.Drawing.Point(185, 243);
            this.pBReturn.Name = "pBReturn";
            this.pBReturn.Size = new System.Drawing.Size(48, 48);
            // 
            // tCAlliance
            // 
            this.tCAlliance.Controls.Add(this.tPAlliance);
            this.tCAlliance.Controls.Add(this.tPSendDemand);
            this.tCAlliance.Location = new System.Drawing.Point(0, 0);
            this.tCAlliance.Name = "tCAlliance";
            this.tCAlliance.SelectedIndex = 0;
            this.tCAlliance.Size = new System.Drawing.Size(240, 237);
            this.tCAlliance.TabIndex = 1;
            // 
            // tPAlliance
            // 
            this.tPAlliance.Controls.Add(this.bStartImpeachment);
            this.tPAlliance.Controls.Add(this.bSupportLeader);
            this.tPAlliance.Controls.Add(this.bTryTakeLeadership);
            this.tPAlliance.Controls.Add(this.bRuleEmpire);
            this.tPAlliance.Controls.Add(this.pBExitAlliance);
            this.tPAlliance.Location = new System.Drawing.Point(0, 0);
            this.tPAlliance.Name = "tPAlliance";
            this.tPAlliance.Size = new System.Drawing.Size(240, 214);
            this.tPAlliance.Text = "Aliance";
            // 
            // bStartImpeachment
            // 
            this.bStartImpeachment.Location = new System.Drawing.Point(7, 111);
            this.bStartImpeachment.Name = "bStartImpeachment";
            this.bStartImpeachment.Size = new System.Drawing.Size(226, 46);
            this.bStartImpeachment.TabIndex = 7;
            this.bStartImpeachment.Text = "Dismiss";
            // 
            // bSupportLeader
            // 
            this.bSupportLeader.Location = new System.Drawing.Point(7, 59);
            this.bSupportLeader.Name = "bSupportLeader";
            this.bSupportLeader.Size = new System.Drawing.Size(226, 46);
            this.bSupportLeader.TabIndex = 6;
            this.bSupportLeader.Text = "Support";
            // 
            // bTryTakeLeadership
            // 
            this.bTryTakeLeadership.Location = new System.Drawing.Point(7, 7);
            this.bTryTakeLeadership.Name = "bTryTakeLeadership";
            this.bTryTakeLeadership.Size = new System.Drawing.Size(226, 46);
            this.bTryTakeLeadership.TabIndex = 5;
            this.bTryTakeLeadership.Text = "Choose";
            // 
            // bRuleEmpire
            // 
            this.bRuleEmpire.Location = new System.Drawing.Point(7, 163);
            this.bRuleEmpire.Name = "bRuleEmpire";
            this.bRuleEmpire.Size = new System.Drawing.Size(172, 48);
            this.bRuleEmpire.TabIndex = 4;
            this.bRuleEmpire.Text = "Rule";
            // 
            // pBExitAlliance
            // 
            this.pBExitAlliance.Location = new System.Drawing.Point(185, 163);
            this.pBExitAlliance.Name = "pBExitAlliance";
            this.pBExitAlliance.Size = new System.Drawing.Size(48, 48);
            // 
            // tPSendDemand
            // 
            this.tPSendDemand.Controls.Add(this.cBReciever);
            this.tPSendDemand.Controls.Add(this.bRequestHelp);
            this.tPSendDemand.Controls.Add(this.bSend);
            this.tPSendDemand.Controls.Add(this.nUDUnits);
            this.tPSendDemand.Controls.Add(this.nUDResources);
            this.tPSendDemand.Controls.Add(this.cBUnits);
            this.tPSendDemand.Controls.Add(this.cBRecources);
            this.tPSendDemand.Location = new System.Drawing.Point(0, 0);
            this.tPSendDemand.Name = "tPSendDemand";
            this.tPSendDemand.Size = new System.Drawing.Size(232, 211);
            this.tPSendDemand.Text = "Send/Recieve";
            // 
            // cBReciever
            // 
            this.cBReciever.Location = new System.Drawing.Point(8, 60);
            this.cBReciever.Name = "cBReciever";
            this.cBReciever.Size = new System.Drawing.Size(225, 22);
            this.cBReciever.TabIndex = 7;
            // 
            // bRequestHelp
            // 
            this.bRequestHelp.Location = new System.Drawing.Point(8, 191);
            this.bRequestHelp.Name = "bRequestHelp";
            this.bRequestHelp.Size = new System.Drawing.Size(225, 20);
            this.bRequestHelp.TabIndex = 6;
            this.bRequestHelp.Text = "Ask";
            // 
            // bSend
            // 
            this.bSend.Location = new System.Drawing.Point(8, 88);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(225, 20);
            this.bSend.TabIndex = 4;
            this.bSend.Text = "Send";
            // 
            // nUDUnits
            // 
            this.nUDUnits.Location = new System.Drawing.Point(8, 32);
            this.nUDUnits.Name = "nUDUnits";
            this.nUDUnits.Size = new System.Drawing.Size(67, 22);
            this.nUDUnits.TabIndex = 3;
            // 
            // nUDResources
            // 
            this.nUDResources.Location = new System.Drawing.Point(8, 7);
            this.nUDResources.Name = "nUDResources";
            this.nUDResources.Size = new System.Drawing.Size(67, 22);
            this.nUDResources.TabIndex = 2;
            // 
            // cBUnits
            // 
            this.cBUnits.Location = new System.Drawing.Point(81, 32);
            this.cBUnits.Name = "cBUnits";
            this.cBUnits.Size = new System.Drawing.Size(152, 22);
            this.cBUnits.TabIndex = 1;
            // 
            // cBRecources
            // 
            this.cBRecources.Location = new System.Drawing.Point(81, 7);
            this.cBRecources.Name = "cBRecources";
            this.cBRecources.Size = new System.Drawing.Size(152, 22);
            this.cBRecources.TabIndex = 0;
            // 
            // cMHelp
            // 
            this.cMHelp.MenuItems.Add(this.mIRes);
            this.cMHelp.MenuItems.Add(this.mIUnits);
            // 
            // mIRes
            // 
            this.mIRes.Text = "Resources";
            // 
            // mIUnits
            // 
            this.mIUnits.Text = "Units";
            // 
            // AlianceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tCAlliance);
            this.Controls.Add(this.pBReturn);
            this.Name = "AlianceControl";
            this.Size = new System.Drawing.Size(240, 294);
            this.tCAlliance.ResumeLayout(false);
            this.tPAlliance.ResumeLayout(false);
            this.tPSendDemand.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBReturn;
        private System.Windows.Forms.TabControl tCAlliance;
        private System.Windows.Forms.TabPage tPSendDemand;
        private System.Windows.Forms.TabPage tPAlliance;
        private System.Windows.Forms.Button bRuleEmpire;
        private System.Windows.Forms.PictureBox pBExitAlliance;
        private System.Windows.Forms.Button bStartImpeachment;
        private System.Windows.Forms.Button bSupportLeader;
        private System.Windows.Forms.Button bTryTakeLeadership;
        private System.Windows.Forms.Button bRequestHelp;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.NumericUpDown nUDUnits;
        private System.Windows.Forms.NumericUpDown nUDResources;
        private System.Windows.Forms.ComboBox cBUnits;
        private System.Windows.Forms.ComboBox cBRecources;
        private System.Windows.Forms.ComboBox cBReciever;
        private System.Windows.Forms.ContextMenu cMHelp;
        private System.Windows.Forms.MenuItem mIRes;
        private System.Windows.Forms.MenuItem mIUnits;
    }
}
