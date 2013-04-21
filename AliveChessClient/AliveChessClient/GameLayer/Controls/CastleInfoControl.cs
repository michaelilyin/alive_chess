using System;
using System.Windows.Forms;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class CastleInfoControl : UserControl
    {
        private Game game;
        private Castle castle;
        private TimeSpan time = TimeSpan.Zero;

        public CastleInfoControl(Game game)
        {
            this.game = game;
            InitializeComponent();

            this.timer1.Tick += new EventHandler(SendDefault);
        }

        public void Initialize(Castle castle)
        {
            time = TimeSpan.Zero;
           // this.timer1.Start();
            this.castle = castle;
            button1.Enabled = true;
        }

        private void SendDefault(object sender, EventArgs e)
        {
            if (time > TimeSpan.FromSeconds(50))
            {
                timer1.Stop();
                time = TimeSpan.Zero;
                button1_Click(sender, e);
                button1.Enabled = false;
                //timer1.Tick -= new EventHandler(SendDefault);
            }
            else time += TimeSpan.FromSeconds(1);
        }

        #region Queries

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button1.Enabled = false;
            QueryManager.SendCaptureCastleMessage(game.Player, game.Dispute, DialogState.Offer);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            QueryManager.SendBigMapMessage(game.Player);
        }

        #endregion

        #region Answers

        public void AgreeCaptureAnswer()
        {
            label1.Text = game.Dispute.Respondent.Name + " готов отстоять свой замок";

            // get battle context
        }

        public void RefuseCaptureAnswer()
        {
            label1.Text = game.Dispute.Respondent.Name + " капитулировал";

            QueryManager.SendCaptureCastle(game.Player, game.Player.Map[castle.X, castle.Y]);

            //this.button2.Visible = true;
            QueryManager.SendBigMapMessage(game.Player);
        }

        #endregion
    }
}
