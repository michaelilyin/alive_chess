using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AliveChessClient.GameLayer.AliveChessGraphics;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class CastleControl : UserControl
    {
        private Bitmap bitmap;
        private Game context;
        private CastleGraphicManager grManager;

        public CastleControl(Game context)
        {
            InitializeComponent();
            this.context = context;
            this.pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            this.bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            this.grManager = new CastleGraphicManager(context, bitmap);
        }

        public void DrawScene()
        {
            grManager.Draw();
        }

        #region Player's actions inside the castle

        public void LeaveCastle()
        {
            LeaveCastleRequest request = new LeaveCastleRequest();
            ClientApplication.Instance.Transport.Send<LeaveCastleRequest>(request);
        }

        #endregion

        #region Player's input

        private void button1_Click(object sender, EventArgs e)
        {
            LeaveCastle();
        }

        #endregion

        public CastleGraphicManager GraphicManager
        {
            get { return grManager; }
        }

        public Image CastleScene
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }

        private void labelBuildingsInCastle_Click(object sender, EventArgs e)
        {

        }

        //получить количество ресурсов для постройки здания
        private void GetResBuild(InnerBuildingType type)
        {
            GetRecBuildingsRequest request = new GetRecBuildingsRequest();
            request.Type = type;
            ClientApplication.Instance.Transport.Send<GetRecBuildingsRequest>(request);
        }
        //выдать список зданий
        private void getlist()
        {
            GetListBuildingsInCastleRequest request = new GetListBuildingsInCastleRequest();
            ClientApplication.Instance.Transport.Send<GetListBuildingsInCastleRequest>(request);
        }
        //Нанять юнитов
        private void hireUnit(UnitType t, int count)
        {
            BuyFigureRequest request = new BuyFigureRequest();
            request.FigureType = t;
            request.FigureCount = count;
            ClientApplication.Instance.Transport.Send<BuyFigureRequest>(request);
        }

        public void ShowArmC(List<Unit> arm)
        {
            comboBoxKingArmy.Items.Clear();
            string s = "";
            for (int i = 0; i < arm.Count; i++)
            {
                s = "Имя юнита  " + Convert.ToString(arm[i].UnitType + ";  Количество " + Convert.ToString(arm[i].UnitCount) + ";");
                comboBoxKingArmy.Items.Add(s);
            }
        }
        //показать армию Замка
        private void showArmyCastle()
        {
            ShowArmyCastleRequest request = new ShowArmyCastleRequest();
            ClientApplication.Instance.Transport.Send<ShowArmyCastleRequest>(request);
        }

        //Показать армию короля
        private void showArmyKing()
        {
            ShowArmyKingRequest request = new ShowArmyKingRequest();
            ClientApplication.Instance.Transport.Send<ShowArmyKingRequest>(request);
        }

        //public void date(ResourceTypes res, int count)
        //{
        //    //label1.Text = "Уголь = " + Convert.ToString(res.Coal) + "; " + "Золото = " + Convert.ToString(res.Gold) + "; " + "Железо = "
        //    //    + Convert.ToString(res.Iron) + "; " + "Камень = " + Convert.ToString(res.Stone) + "; " + "Дерево = " + Convert.ToString(res.Wood);
        //    label1.Text = String.Concat(res.ToString(), "=", count);
        //}

        public void date(ResBuild res)
        {
            label1.Text = "Уголь = " + Convert.ToString(res.Coal) + "; " + "Золото = " + Convert.ToString(res.Gold) + "; " + "Железо = "
                + Convert.ToString(res.Iron) + "; " + "Камень = " + Convert.ToString(res.Stone) + "; " + "Дерево = " + Convert.ToString(res.Wood);
        }

        //Выдать армию
        public void getArmy(IList<Unit> arm, int temp)
        {
            comboBoxArmy.Items.Clear();
            if (arm != null)
            {
                string s = "";
                for (int i = 0; i < arm.Count; i++)
                {
                    s = "Имя юнита  " + Convert.ToString(arm[i].UnitType + ";  Количество " + Convert.ToString(arm[i].UnitCount) + ";");
                    if (temp == 1) comboBoxArmy.Items.Add(s);
                    if (temp == 2) comboBoxKingArmy.Items.Add(s);
                }
            }
            else comboBoxArmy.Items.Add("Армии нет");
        }
        //Список построенных зданий
        public void list(IInnerBuilding str)
        {
            comboBoxBuild.Items.Clear();
            comboBoxBuild.Items.Add(str.Name);
        }

        //Запрос на взятие армии из замка
        private void getArmyToKing()
        {
            GetArmyCastleToKingRequest request = new GetArmyCastleToKingRequest();
            ClientApplication.Instance.Transport.Send<GetArmyCastleToKingRequest>(request);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboBox1.SelectedIndex;
            switch (i)
            {
                case 0:
                    GetResBuild(InnerBuildingType.Voencomat);
                    break;
                case 1:
                    GetResBuild(InnerBuildingType.Stable);
                    break;
                case 2:
                    GetResBuild(InnerBuildingType.SchoolOfficers);
                    break;
                case 3:
                    GetResBuild(InnerBuildingType.GeneralStaff);
                    break;
                case 4:
                    GetResBuild(InnerBuildingType.VVU);
                    break;
            }
        }

        //построить
        private void build(InnerBuildingType i)
        {
            BuildingInCastleRequest request = new BuildingInCastleRequest();
            request.Type = i;
            ClientApplication.Instance.Transport.Send<BuildingInCastleRequest>(request);

        }

        private void button_Buildings_Click(object sender, EventArgs e)
        {
            int i = comboBox1.SelectedIndex;
            switch (i)
            {
                case 0:
                    build(InnerBuildingType.Voencomat);
                    break;
                case 1:
                    build(InnerBuildingType.Stable);
                    break;
                case 2:
                    build(InnerBuildingType.SchoolOfficers);
                    break;
                case 3:
                    build(InnerBuildingType.GeneralStaff);
                    break;
                case 4:
                    build(InnerBuildingType.VVU);
                    break;
            }
        }

        private void buttonListBuildings_Click(object sender, EventArgs e)
        {
            getlist();
        }

        private void buttonHireUnit_Click(object sender, EventArgs e)
        {
            string i = Convert.ToString(comboBoxBuild.SelectedItem);
            InnerBuildingType t = InnerBuildingType.Voencomat;
            UnitType ut = UnitType.Pawn;
            if (i == "Voencomat")
            {
                ut = UnitType.Pawn;
                t = InnerBuildingType.Voencomat;
            }
            if (i == "Stable")
            {
                ut = UnitType.Knight;
                t = InnerBuildingType.Stable;
            }
            if (i == "SchoolOfficers;")
            {
                ut = UnitType.Bishop;
                t = InnerBuildingType.SchoolOfficers;
            }
            if (i == "GeneralStaff")
            {
                ut = UnitType.Queen;
                t = InnerBuildingType.GeneralStaff;
            }
            if (i == "VVU")
            {
                ut = UnitType.Rook;
                t = InnerBuildingType.VVU;
            }

            hireUnit(ut, Convert.ToInt32(textBoxCountUnit.Text));
        }

        private void buttonArmyCastle_Click(object sender, EventArgs e)
        {
            showArmyCastle();
        }

        private void buttonKing_Click(object sender, EventArgs e)
        {
            showArmyKing(); 
        }

        private void buttonGetArmyToKing_Click(object sender, EventArgs e)
        {
            getArmyToKing();
        }

    }
}
