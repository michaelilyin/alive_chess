using System;
using System.Windows.Forms;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class BattleDisputeControl : UserControl
    {
        private Game game;
        private TimeSpan time = TimeSpan.Zero;

        public BattleDisputeControl(Game game)
        {
            this.game = game;
            InitializeComponent();
        }

        public void Initialize()
        {
            time = TimeSpan.Zero;
            SetEnabled();
            //this.timer1.Start();
            this.label4.Text = game.Dispute.Respondent.Name + " напал на вас";
        }

        public void SetEnabled()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        public void SetDisabled()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        #region Queries

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            QueryManager.SendBattleMessage(game.Player, game.Dispute, DialogState.Agree);
            SetDisabled();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            QueryManager.SendPayOffMessage(game.Player, game.Dispute, DialogState.Offer, 10);
            SetDisabled();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            CapitulateDialogMessage msg = new CapitulateDialogMessage();
            msg.DisputeId = game.Dispute.Id;
            msg.State = DialogState.Offer;

            ClientApplication.Instance.Transport.Send<CapitulateDialogMessage>(msg);
            SetDisabled();
        }

        #endregion

        #region Answers

        public void AgreePayOffAnswer()
        {
            label4.Text = game.Dispute.Respondent.Name + " согласен принять ваш откуп";
            QueryManager.SendBigMapMessage(game.Player);
        }

        public void RefusePayOffAnswer()
        {
            label4.Text = game.Dispute.Respondent.Name + " отказывается от откупа";

            // get battle context
        }

        public void AgreeCapitulateAnswer()
        {
            label4.Text = game.Dispute.Respondent.Name + " согласен отпустить вас";
            QueryManager.SendBigMapMessage(game.Player);
        }

        public void RefuseCapitulateAnswer()
        {
            label4.Text = game.Dispute.Respondent.Name + " все равно хочет сражаться";

            // get battle context
        }

        #endregion
    }
}
