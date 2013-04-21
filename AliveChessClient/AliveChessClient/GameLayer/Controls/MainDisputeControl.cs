using System;
using System.Windows.Forms;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class MainDisputeControl : UserControl
    {
        private Game game;
        private TimeSpan time = TimeSpan.Zero;

        public MainDisputeControl(Game game)
        {
            this.game = game;
            InitializeComponent();
        }

        public void Initialize()
        {
            time = TimeSpan.Zero;
            SetEnabled();
            //this.timer1.Start();
        }

        private void SetDisabled()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void SetEnabled()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        #region Queries

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            QueryManager.SendTradeMessage(game.Player, game.Dispute, DialogState.Offer);

            SetDisabled();
            label1.Text = "Ждите...";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            QueryManager.SendBattleMessage(game.Player, game.Dispute, DialogState.Offer);

            SetDisabled();
            label1.Text = "Ждите...";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            QueryManager.CreateUnionMessage(game.Player, game.Dispute, DialogState.Offer);

            SetDisabled();
            label1.Text = "Ждите...";
        }

        public void ShowText(string str)
        {
            label3.Text = str;
        }

        #endregion

        #region Answers

        public void AgreeTradeAnswer()
        {
            label2.Text = game.Dispute.Respondent.Name + " согласен торговать с вами";

            // get trade context
        }

        public void RefuseTradeAnswer()
        {
            label2.Text = game.Dispute.Respondent.Name + " отказывается иметь с вами дело";
            QueryManager.SendBigMapMessage(game.Player);
            //this.button4.Visible = true;
        }

        public void AgreeBattleAnswer()
        {
            label2.Text = game.Dispute.Respondent.Name + " согласен с вами сражаться";

            // get battle context
        }

        public void AgreeEmpireAnswer()
        {
            label2.Text = game.Dispute.Respondent.Name + " согласен создать империю";
            QueryManager.SendBigMapMessage(game.Player);
            game.BigMap.ActivateUnionButton();
        }

        public void RefuseEmpireAnswer()
        {
            label2.Text = game.Dispute.Respondent.Name + " отказывается создать империю";
            QueryManager.SendBigMapMessage(game.Player);
        }

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            QueryManager.SendBigMapMessage(game.Player);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            QueryManager.SendBigMapMessage(game.Player);
        }
    }
}
