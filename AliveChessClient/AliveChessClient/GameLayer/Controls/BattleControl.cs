using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class BattleControl : UserControl
    {

        private Game context;
        
        bool ok = true;
        bool course = true;
        IList<Unit> b = new List<Unit>();
        byte[,] MasBatle = new byte[8, 8];
        byte[] FromTo = new byte[2];
        int temp = 0;
        
        
        
        public BattleControl(Game context)
        {
            this.context = context;
            InitializeComponent();
        }

        private void BattleControl_Load(object sender, EventArgs e)
        {
            dataGridViewBatle.RowCount = 8;
            dataGridViewBatle[0, 7].Value = 5;
        }
        private void fild()
        {
            byte temp = 0;
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    MasBatle[i, j] = temp;
                    temp++;
                }
            }
        }
        public void downloadArmy(IList<Unit> arm, IList<Unit> opponentArm, bool c)
        {
            fild();
            int count;
            string temp = "";
            dataGridViewBatle[4, 7].Value = "Король!";
            for (int i = 0; i < arm.Count; i++)
            {
                if (Convert.ToInt32(arm[i].UnitType) == 2)
                {
                    count = arm[i].UnitCount / 2;
                    temp = "Ладья" + "  К.Б=" + Convert.ToString(count);
                    dataGridViewBatle[0, 7].Value = "Б " + temp;
                    dataGridViewBatle[7, 7].Value = "Б " + temp;
                }
                if (Convert.ToInt32(arm[i].UnitType) == 0)
                {
                    count = arm[i].UnitCount / 2;
                    temp = "Конь" + "  К.Б=" + Convert.ToString(count);
                    dataGridViewBatle[1, 7].Value = "Б " + temp;
                    dataGridViewBatle[6, 7].Value = "Б " + temp;
                }
                if (Convert.ToInt32(arm[i].UnitType) == 3)
                {
                    count = arm[i].UnitCount / 2;
                    temp = "Офицер" + "  К.Б=" + Convert.ToString(count);
                    dataGridViewBatle[2, 7].Value = "Б " + temp;
                    dataGridViewBatle[5, 7].Value = "Б " + temp;
                }
                if (Convert.ToInt32(arm[i].UnitType) == 1)
                {
                    temp = "Королева" + "  К.Б=" + Convert.ToString(arm[i].UnitCount);
                    dataGridViewBatle[3, 7].Value = temp;
                }
                if (Convert.ToInt32(arm[i].UnitType) == 10)
                {
                    count = arm[i].UnitCount / 8;
                    temp = "Пешка" + "  К.Б=" + Convert.ToString(count);
                    dataGridViewBatle[0, 6].Value = "Б " + temp;
                    dataGridViewBatle[1, 6].Value = "Б " + temp;
                    dataGridViewBatle[2, 6].Value = "Б " + temp;
                    dataGridViewBatle[3, 6].Value = "Б " + temp;
                    dataGridViewBatle[4, 6].Value = "Б " + temp;
                    dataGridViewBatle[5, 6].Value = "Б " + temp;
                    dataGridViewBatle[6, 6].Value = "Б " + temp;
                    dataGridViewBatle[7, 6].Value = "Б " + temp;
                }
            }
            dataGridViewBatle[4, 0].Value = "Король!";
            for (int i = 0; i < opponentArm.Count; i++)
            {
                if (Convert.ToInt32(opponentArm[i].UnitType) == 2)
                {
                    count = opponentArm[i].UnitCount / 2;
                    temp = "Ладья" + "  К.Б=" + Convert.ToString(count);
                    dataGridViewBatle[0, 0].Value = "Ч " + temp;
                    dataGridViewBatle[7, 0].Value = "Ч " + temp;
                }
                if (Convert.ToInt32(opponentArm[i].UnitType) == 0)
                {
                    count = opponentArm[i].UnitCount / 2;
                    temp = "Конь" + "  К.Б=" + Convert.ToString(count);
                    dataGridViewBatle[1, 0].Value = "Ч " + temp;
                    dataGridViewBatle[6, 0].Value = "Ч " + temp;
                }
                if (Convert.ToInt32(opponentArm[i].UnitType) == 3)
                {
                    count = opponentArm[i].UnitCount / 2;
                    temp = "Офицер" + "  К.Б=" + Convert.ToString(count);
                    dataGridViewBatle[2, 0].Value = "Ч " + temp;
                    dataGridViewBatle[5, 0].Value = "Ч " + temp;
                }
                if (Convert.ToInt32(opponentArm[i].UnitType) == 1)
                {
                    temp = "Королева" + "  К.Б=" + Convert.ToString(opponentArm[i].UnitCount);
                    dataGridViewBatle[3, 0].Value = "Ч " + temp;
                }
                if (Convert.ToInt32(opponentArm[i].UnitType) == 10)
                {
                    count = opponentArm[i].UnitCount / 8;
                    temp = "Пешка" + "  К.Б=" + Convert.ToString(count);
                    dataGridViewBatle[0, 1].Value = "Ч " + temp;
                    dataGridViewBatle[1, 1].Value = "Ч " + temp;
                    dataGridViewBatle[2, 1].Value = "Ч " + temp;
                    dataGridViewBatle[3, 1].Value = "Ч " + temp;
                    dataGridViewBatle[4, 1].Value = "Ч " + temp;
                    dataGridViewBatle[5, 1].Value = "Ч " + temp;
                    dataGridViewBatle[6, 1].Value = "Ч " + temp;
                    dataGridViewBatle[7, 1].Value = "Ч " + temp;
                }
            }



        }

        public void qwer(IList<Unit> a, IList<Unit> opp, bool c)
        {
            downloadArmy(a, opp, c);
            if (c == false) MessageBox.Show("ЖДИТЕ!");
            else MessageBox.Show("Ваш ход!");

        }

        public void playerMove(byte[] t, int c, bool step)
        {
            if (step == false) MessageBox.Show("Вы уже ходили!");
            try
            {
                int n = dataGridViewBatle.ColumnCount;
                int r = dataGridViewBatle.RowCount;
                int j = t[0] / n;
                int i = t[0] - n * j;
                string s = Convert.ToString(dataGridViewBatle[i, r - j - 1].Value);
                dataGridViewBatle[i, r - j - 1].Value = "";
                j = t[1] / n;
                i = t[1] - n * j;
                int temp = s.Length;
                char[] m = s.ToCharArray();
                for (int b = s.Length - 1; b < c.ToString().Length; b--)
                {
                    m[b] = c.ToString()[(s.Length - 1) - b];
                }

                dataGridViewBatle[i, r - j - 1].Value = s;//Convert.ToString(m);
                ok = !ok;
            }
            catch
            {
                MessageBox.Show("Вы уже ходили!");
            }
        }

        
        

        private void dataGridViewBatle_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                FromTo[temp] = MasBatle[dataGridViewBatle.SelectedCells[0].RowIndex, dataGridViewBatle.SelectedCells[0].ColumnIndex];
                MessageBox.Show(Convert.ToString(FromTo[temp]));
                temp++;
                if (temp == 2)
                {
                    if (ok)
                    {
                        QueryManager.SendMoveUnit(context.Battle, FromTo, ok);
                        temp = 0;

                    }
                    else
                    {
                        FromTo[0] = (byte)(63 - FromTo[0]);
                        FromTo[1] = (byte)(63 - FromTo[1]);
                        QueryManager.SendMoveUnit(context.Battle, FromTo, ok);
                        temp = 0;

                    }


                }

            }
            catch
            {
                MessageBox.Show("ЖДИТЕ!!!");
            }
        }
    }
}
