using System;
using System.Windows.Forms;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class NegotiateControl : UserControl
    {
        private Game game;

        public NegotiateControl(Game game)
        {
            this.game = game;
            InitializeComponent();
        }

        public void Initialize()
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QueryManager.SendWarMessage(game.Player, game.Negotiate, DialogState.Coerce);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            QueryManager.SendBigMapMessage(game.Player);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            QueryManager.SendJoinEmperiesMessage(game.Player, game.Negotiate, DialogState.Offer);
        }

        public void AgreeEmperiesJoinAnswer()
        {
            label1.Text = game.Negotiate.Respondent.Name + " согласен объединить имерии";
            QueryManager.SendBigMapMessage(game.Player);
        }

        public void RefuseEmperiesJoinAnswer()
        {
            label1.Text = game.Negotiate.Respondent.Name + " отказывается от объединения";
        }
    }
}
