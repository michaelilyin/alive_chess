using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AliveChessLibrary.Entities.Characters;
using AliveChessLibrary.Entities.Abstract;
using AliveChessLibrary.Commands;
using AliveChessClient.NetLayer.Main;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class DisputeControl : UserControl
    {
        private GameContext game;
     
        public DisputeControl(GameContext game)
        {
            this.game = game;
            InitializeComponent();
            this.button4.Visible = false;
        }

        public void Initialize(Dispute discussion)
        {
            this.game.Dispute = discussion;
            this.game.Dispute.FirstKing = game.Player.King;

            label2.Text = "Opponent name: " + discussion.SecondKing.Name;
            label3.Text = "Opponent experience: " + discussion.SecondKing.Experience.ToString();
            label4.Text = "Opponent military rank: " + discussion.SecondKing.MilitaryRank.ToString();

            if (discussion.YouStep)
                label1.Text = "You step";
            else label1.Text = "Please wait...";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (game.Dispute.YouStep)
            {
                TradeMessage trade = new TradeMessage();
                trade.DiscussionId = game.Dispute.Id;
                trade.PlayerId = game.Dispute.FirstKing.Id;
                trade.Type = DisputeTypes.Offer;

                ClientApplication.Instance.Transport.Send<TradeMessage>(trade);

                game.Dispute.YouStep = false;
                label1.Text = "Please wait...";
            }
        }

        public void Agree(string text)
        {
            label5.Text = game.Dispute.SecondKing.Name + text;

            // get trade context
        }

        public void Deny(string text)
        {
            label5.Text = game.Dispute.SecondKing.Name + text;
            this.button4.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BigMapRequest request = new BigMapRequest();
            request.PlayerId = game.Player.Id;
            ClientApplication.Instance.Transport.Send<BigMapRequest>(request);
        }
    }
}
