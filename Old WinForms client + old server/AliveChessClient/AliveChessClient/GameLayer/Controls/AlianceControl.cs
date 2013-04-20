using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class AlianceControl : UserControl
    {
        private Game game;
        private Dictionary<string, ResourceTypes> resTypes = new Dictionary<string, ResourceTypes>();
        private Dictionary<string, UnitType> unitTypes = new Dictionary<string, UnitType>();

        public AlianceControl(Game game)
        {
            this.game = game;
            InitializeComponent();
      
            button9.Enabled = false;
            this.comboBox1.Items.Add("Ресурсы");
            this.comboBox1.Items.Add("Фигуры");

            resTypes.Add("Coal", ResourceTypes.Coal);
            resTypes.Add("Gold", ResourceTypes.Gold);
            resTypes.Add("Iron", ResourceTypes.Iron);
            resTypes.Add("Stone", ResourceTypes.Stone);
            resTypes.Add("Wood", ResourceTypes.Wood);

            unitTypes.Add("Bishop", UnitType.Bishop);
            unitTypes.Add("Knight", UnitType.Knight);
            unitTypes.Add("Pawn", UnitType.Pawn);
            unitTypes.Add("Queen", UnitType.Queen);
            unitTypes.Add("Rook", UnitType.Rook);

            comboBox1.SelectedIndex = 0;
        }

        private void InitResourceCombo()
        {
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(resTypes.Keys.ToArray());
            comboBox2.SelectedIndex = 1;
        }

        private void InitFigureCombo()
        {
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(unitTypes.Keys.ToArray());
            comboBox2.SelectedIndex = 2;
        }

        public void Initialize()
        {
            ShowMembers(game.Members);
            label1.Text = "Union id is: " + game.UnionId.ToString();
        }

        public void ShowMembers(List<GetAlianceInfoResponse.MemberInfo> m)
        {
            checkedListBox1.Items.Clear();
            m.ForEach(x => { checkedListBox1.Items.Add(x.MemberId); });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(numericUpDown1.Value);
            if (comboBox1.SelectedIndex == 0)
            {
                ResourceTypes type = resTypes[comboBox2.Text];
                QueryManager.SendGetHelpResource(game.Player, count, type);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(numericUpDown1.Value);
            if (comboBox1.SelectedIndex == 0)
            {
                ResourceTypes type = resTypes[comboBox2.Text];
                Resource r = new Resource();
                r.CountResource = count;
                r.ResourceType = type;
                List<Resource> l = new List<Resource>();
                l.Add(r);
                game.Player.King.StartCastle.ResourceStore.DeleteResourceFromRepository(type, count);
                foreach (object v in checkedListBox1.SelectedItems)
                {
                    int i = Convert.ToInt32(v);
                    QueryManager.SendResourceHelpMessage(game.Player, i, l);
                    Thread.Sleep(100);
                }
                game.BigMap.UpdateResources(game.Player.King.StartCastle.ResourceStore
                    .GetResourceCountInRepository(type), type);
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                UnitType type = unitTypes[comboBox2.Text];
                Unit r = new Unit();
                r.UnitCount = count;
                r.UnitType = type;
                List<Unit> l = new List<Unit>();
                l.Add(r);
                foreach (object v in checkedListBox1.CheckedItems)
                {
                    int i = Convert.ToInt32(v);
                    QueryManager.SendFigureHelpMessage(game.Player, i, l);
                    Thread.Sleep(100);
                }
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0)
                InitResourceCombo();
            else if (((ComboBox)sender).SelectedIndex == 1)
                InitFigureCombo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(numericUpDown1.Value);
            if (comboBox1.SelectedIndex == 1)
            {
                UnitType type = unitTypes[comboBox2.Text];
                QueryManager.SendGetHelpFigure(game.Player, count, type);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            QueryManager.SendBigMapMessage(game.Player);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            QueryManager.SendStartVote(game.Player, "Vote is starting");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            game.BigMap.SetText("You have voted");
            QueryManager.SendVoteFact(game.Player, true);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            QueryManager.SendStartImpeachment(game.Player, "Impeacment is starting");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            QueryManager.SendExitFromUnionOrEmpire(game.Player);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            game.GameForm.StartLeaderDialog();
        }

        public void ActivateLeaderButton()
        {
            button9.Enabled = true;
        }

        public void DeactivateLeaderButton()
        {
            button9.Enabled = false;
        }
    }
}
