using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AliveChessLibrary.Entities.Characters;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Entities.Abstract;
using AliveChessLibrary.Entities.Resources;
using AliveChessLibrary.Entities.Buildings;
using AliveChessLibrary.Commands;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class CaptureCastleControl : UserControl
    {
        private GameContext game;
        private Castle castle;

        public CaptureCastleControl(GameContext game)
        {
            this.game = game;
            InitializeComponent();
        }

        public void Initialize(Castle castle)
        {
            this.castle = castle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisputeCastleRequest request = new DisputeCastleRequest();
            request.PlayerId = game.Player.Id;
            request.CastleId = castle.Id;
            ClientApplication.Instance.Transport.Send<DisputeCastleRequest>(request);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BigMapRequest request = new BigMapRequest();
            request.PlayerId = game.Player.Id;
            ClientApplication.Instance.Transport.Send<BigMapRequest>(request);
        }

        public void AgreeCaptureAnswer()
        {
            label1.Text = game.Dispute.SecondKing.Name + " готов отстоять свой замок";

            // get battle context
        }

        public void RefuseCaptureAnswer()
        {
            label1.Text = game.Dispute.SecondKing.Name + " капитулировал";

            CaptureCastleRequest request = new CaptureCastleRequest();
            request.PlayerId = game.Player.Id;
            request.CastleId = castle.Id;
            ClientApplication.Instance.Transport.Send<CaptureCastleRequest>(request);

            this.button2.Visible = true;
        }
    }
}
