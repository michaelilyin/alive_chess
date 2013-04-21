using System;
using System.Threading;
using System.Windows.Forms;
using AliveChessClient.NetLayer.Main;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class LeaderControl : UserControl
    {
        private Game game;

        public LeaderControl(Game game)
        {
            this.game = game;
            InitializeComponent();
        }

        public void Initialize()
        {
            ShowMembers();
        }

        public void AddCandidate(int id)
        {
            checkedListBox1.Items.Add(id);
        }

        public void ShowMembers()
        {
            checkedListBox2.Items.Clear();
            game.Members.ForEach(x => { checkedListBox2.Items.Add(x.MemberId); });

            checkedListBox3.Items.Clear();
            game.Aliances.ForEach(x => { checkedListBox3.Items.Add(x.Leader.MemberId); });
        }

        private void back_Click(object sender, EventArgs e)
        {
            QueryManager.SendBigMapMessage(game.Player);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (object v in checkedListBox1.SelectedItems)
            {
                int i = Convert.ToInt32(v);
                QueryManager.SendIncludeKingInEmpire(game.Player, i);
                Thread.Sleep(100);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (object v in checkedListBox2.SelectedItems)
            {
                int i = Convert.ToInt32(v);
                QueryManager.SendExcludeKingFromEmpire(game.Player, i);
                Thread.Sleep(100);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int rate = Convert.ToInt32(numericUpDown1.Value);
            QueryManager.SendEmbedTaxRate(game.Player, rate);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            object v = checkedListBox3.SelectedItem;
            int i = Convert.ToInt32(v);
            QueryManager.SendStartNegotiate(game.Player, i);
        }
    }
}
