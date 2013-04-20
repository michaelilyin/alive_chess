using System;
using System.Windows.Forms;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class SimpleDisputeControl : UserControl
    {
        private Game game;
        private TimeSpan time = TimeSpan.Zero;

        public SimpleDisputeControl(Game game)
        {
            this.game = game;

            InitializeComponent();
        }

        private void SetEnabled()
        {
            this.btn_agree.Enabled = true;
            this.btn_refuse.Enabled = true;
        }

        private void SetDisabled()
        {
            this.btn_agree.Enabled = false;
            this.btn_refuse.Enabled = false;
        }

        #region Initialization

        public void InitializeTrade()
        {
            time = TimeSpan.Zero;
            this.timer1.Start();
            this.label1.Text = game.Dispute.Respondent.Name + 
                " предлагает вам торговлю. Вы согласны?";

            SetEnabled();
            this.btn_agree.Click += new EventHandler(AgreeTrade);
            this.btn_refuse.Click += new EventHandler(RefuseTrade);
        }

        public void InitializeEmpire()
        {
            time = TimeSpan.Zero;
            this.timer1.Start();
            this.label1.Text = game.Dispute.Respondent.Name +
                " предлагает вам создать империю. Вы согласны?";

            SetEnabled();
            this.btn_agree.Click += new EventHandler(AgreeEmpire);
            this.btn_refuse.Click += new EventHandler(RefuseEmpire);
        }

        public void InitializePayOff()
        {
            time = TimeSpan.Zero;
            this.label1.Text = game.Dispute.Respondent.Name + 
                " предлагает откуп. Вы согласны?";

            SetEnabled();
            this.btn_agree.Click += new EventHandler(AgreePayOff);
            this.btn_refuse.Click += new EventHandler(RefusePayOff);
        }

        public void InitializeCapitulate()
        {
            time = TimeSpan.Zero;
            this.label1.Text = game.Dispute.Respondent.Name + 
                " не хочет сражаться. Желаете ли отпустить его?";

            SetEnabled();
            this.btn_agree.Click += new EventHandler(AgreeCapitulate);
            this.btn_refuse.Click += new EventHandler(RefuseCapitulate);
        }

        public void InitializeCaptureCastle()
        {
            time = TimeSpan.Zero;
            this.label1.Text = game.Dispute.Respondent.Name + 
                " напал на ваш замок. Желаете ли вы встать на защиту своих владений?";

            SetEnabled();
            this.btn_agree.Click += new EventHandler(AgreeCaptureCastle);
            this.btn_refuse.Click += new EventHandler(RefuseCaptureCastle);
        }

        public void InitializeJoinEmperies()
        {
            time = TimeSpan.Zero;
            this.label1.Text = game.Negotiate.Respondent.Name +
                " Предлагает объединить империи";

            SetEnabled();
            this.btn_agree.Click += new EventHandler(AgreeEmpiresJoin);
            this.btn_refuse.Click += new EventHandler(RefuseEmpiresJoin);
        }

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            QueryManager.SendBigMapMessage(game.Player);
        }

        #region Answers

        private void AgreeTrade(object sender, EventArgs e)
        {
            QueryManager.SendTradeMessage(game.Player, game.Dispute, DialogState.Agree);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreeTrade);
            this.btn_refuse.Click -= new EventHandler(RefuseTrade);

            // get trade context
        }

        private void RefuseTrade(object sender, EventArgs e)
        {
            QueryManager.SendTradeMessage(game.Player, game.Dispute, DialogState.Refuse);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreeTrade);
            this.btn_refuse.Click -= new EventHandler(RefuseTrade);

            QueryManager.SendBigMapMessage(game.Player);
        }

        private void AgreeEmpire(object sender, EventArgs e)
        {
            QueryManager.SendCreateUnionMessage(game.Player, game.Dispute, DialogState.Agree);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreeEmpire);
            this.btn_refuse.Click -= new EventHandler(RefuseEmpire);

            QueryManager.SendBigMapMessage(game.Player);

            game.BigMap.Button1.Enabled = true;
        }

        private void RefuseEmpire(object sender, EventArgs e)
        {
            QueryManager.SendCreateUnionMessage(game.Player, game.Dispute, DialogState.Refuse);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreeEmpire);
            this.btn_refuse.Click -= new EventHandler(RefuseEmpire);

            QueryManager.SendBigMapMessage(game.Player);
        }

        private void AgreeEmpiresJoin(object sender, EventArgs e)
        {
            QueryManager.SendJoinEmperiesMessage(game.Player, game.Negotiate, DialogState.Agree);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreeEmpiresJoin);
            this.btn_refuse.Click -= new EventHandler(RefuseEmpiresJoin);

            QueryManager.SendBigMapMessage(game.Player);

            game.BigMap.Button1.Enabled = true;
        }

        private void RefuseEmpiresJoin(object sender, EventArgs e)
        {
            QueryManager.SendJoinEmperiesMessage(game.Player, game.Negotiate, DialogState.Refuse);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreeEmpiresJoin);
            this.btn_refuse.Click -= new EventHandler(RefuseEmpiresJoin);

            QueryManager.SendBigMapMessage(game.Player);
        }


        private void AgreePayOff(object sender, EventArgs e)
        {
            //timer1.Stop();

            QueryManager.SendPayOffMessage(game.Player, game.Dispute, DialogState.Agree, 0);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreePayOff);
            this.btn_refuse.Click -= new EventHandler(RefusePayOff);

            QueryManager.SendBigMapMessage(game.Player);
        }

        private void RefusePayOff(object sender, EventArgs e)
        {
            QueryManager.SendPayOffMessage(game.Player, game.Dispute, DialogState.Refuse, 0);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreePayOff);
            this.btn_refuse.Click -= new EventHandler(RefusePayOff);

            // start battle
        }


        private void AgreeCapitulate(object sender, EventArgs e)
        {
            QueryManager.SendCapitulateMessage(game.Player, game.Dispute, DialogState.Agree);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreeCapitulate);
            this.btn_refuse.Click -= new EventHandler(RefuseCapitulate);

            QueryManager.SendBigMapMessage(game.Player);
        }

        private void RefuseCapitulate(object sender, EventArgs e)
        {
            QueryManager.SendCapitulateMessage(game.Player, game.Dispute, DialogState.Refuse);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreeCapitulate);
            this.btn_refuse.Click -= new EventHandler(RefuseCapitulate);

            // start battle
        }

        private void AgreeCaptureCastle(object sender, EventArgs e)
        {
            QueryManager.SendCaptureCastleMessage(game.Player, game.Dispute, DialogState.Agree);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreeCaptureCastle);
            this.btn_refuse.Click -= new EventHandler(RefuseCaptureCastle);

            QueryManager.SendBigMapMessage(game.Player);
        }

        private void RefuseCaptureCastle(object sender, EventArgs e)
        {
            QueryManager.SendCaptureCastleMessage(game.Player, game.Dispute, DialogState.Refuse);

            SetDisabled();
            this.btn_agree.Click -= new EventHandler(AgreeCaptureCastle);
            this.btn_refuse.Click -= new EventHandler(RefuseCaptureCastle);

            QueryManager.SendBigMapMessage(game.Player);
        }

        #endregion
    }
}
