using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AliveChessClient.GameLayer.AliveChessGraphics;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;

namespace AliveChessClient.GameLayer.Controls
{
    public partial class BigMapControl : UserControl
    {
        private Player player;
        private Bitmap bitmap;
        private Game game;
        private Queue<MapPoint> stateQueue;
        private GameData context;
        private BigMapGraphicsManager grManager;
        private Dictionary<long, IObserver> observers;
        private object syncStateQueue = new object();

        public BigMapControl(Game game)
        {
            InitializeComponent();

            button1.Enabled = false;

            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

            this.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            this.game = game;
            this.player = game.Player;
            this.bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            this.grManager = new BigMapGraphicsManager(game, bitmap);
            this.pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            this.stateQueue = new Queue<MapPoint>();
            this.observers = new Dictionary<long, IObserver>();
            this.context = new GameData();
    
            GraphicManager.Load();

            timer1.Interval = 10;
            timer1.Tick += new EventHandler(Update);
        }

        public void Initialize()
        {
            game.LoadMap();
          
            QueryManager.SendGetObjectsRequestForPlayer(player);

            timer1.Enabled = true;
            timer1.Start();

            label_gold.Text = "Золото: " + player.King.StartCastle.ResourceStore.GetResourceCountInRepository(ResourceTypes.Gold);
            label_mine.Text = "Шахты: " + player.King.Mines.Count;
            label_castle.Text = "Замки: " + player.King.Castles.Count;
            label_timber.Text = "Древесина: " + player.King.StartCastle.ResourceStore.GetResourceCountInRepository(ResourceTypes.Wood);
            label_name.Text = "Ваше имя: " + game.Player.Login;
            label2.Text = "ID короля: " + game.Player.King.Id;
        }

        public void SetText(string text)
        {
            label1.Text = text;
        }

        public void ActivateUnionButton()
        {
            button1.Enabled = true;
        }

        public void DeactivateUnionButton()
        {
            button1.Enabled = false;
        }

        public void PutNewState(MapPoint o)
        {
            Monitor.Enter(syncStateQueue);
            stateQueue.Enqueue(o);
            Monitor.Exit(syncStateQueue);
        }

        public void Update(object sender, EventArgs e)
        {
            if (stateQueue.Count != 0)
            {
                Monitor.Enter(syncStateQueue);
                MapPoint obj = stateQueue.Dequeue();
                Monitor.Exit(syncStateQueue);

                if (obj.MapPointType == PointTypes.King)
                    obj.ObjectUnderThis = player.Map[obj.X, obj.Y];

                player.Map.SetObject(obj);
                player.Map[obj.X, obj.Y].Detected = true;

                grManager.Draw();
            }

            player.King.Update();
            if (player.King.Updated)
                grManager.Draw();
        }

        private Point PointOnMap(int x, int y)
        {
            return new Point(GraphicManager.DrawableArea.X + x / 20,
                GraphicManager.DrawableArea.Y + y / 20);
        }

        private bool IsCanMove(int x, int y)
        {
            return player.Map[x, y] != null && !(x == player.King.X && y == player.King.Y) &&
                player.Map.Locate(x, y) && player.Map[x, y].Detected && player.King.State == KingState.BigMap;
        }

        public void MoveKing(int x, int y)
        {
            if (IsCanMove(x, y))
            {
                player.King.IsMove = true;
      
                MoveKingRequest request = new MoveKingRequest();
                request.X = x;
                request.Y = y;
                ClientApplication.Instance.Transport.Send<MoveKingRequest>(request);
            }
        }

        public void UpdateKingVisibleSpace(King king)
        {
            //QueryManager.SendGetObjectsRequestForConcreteObserver(player, king);
            grManager.CalculateBounds();
        }

        public void ComeInCastle(King king, MapSector castle)
        {
            QueryManager.SendComeInCastle(game.Player, castle);
        }

        //public void CollectResource(MapObject resource)
        //{
        //    QueryManager.SendCollectResource(game.Player, resource);
        //}

        public void CaptureMine(King king, MapPoint mine)
        {
            //QueryManager.SendCaptureMine(game.Player, mine);
        }

        public void CaptureCastle(King king, MapSector castle)
        {
            //QueryManager.SendMeetCastle(game.Player, castle);
        }

        public void MeetKing(King king, MapPoint opponent)
        {
            if (king.Id != opponent.Id)
                QueryManager.SendMeetKing(game.Player, opponent);
        }

        public void MeetCastle(King player, MapSector castle)
        {
            QueryManager.SendMeetCastle(player.Player as Player, castle);
        }

        public void UpdateResource(object rCount, object rType)
        {
            int count = Convert.ToInt32(rCount);
            ResourceTypes type = (ResourceTypes)rType;
            if (type == ResourceTypes.Gold)
                label_gold.Text = "Золото: " + count.ToString();
            if (type == ResourceTypes.Wood)
                label_timber.Text = "Древесина: " + count.ToString();
        }

        public void UpdateMinesCount(int count)
        {
            label_mine.Text = "Шахты: " + count.ToString();
        }

        public void UpdateCastlesCount(int count)
        {
            label_castle.Text = "Замки: " + count.ToString();
        }

        public void UpdateResources(int count, ResourceTypes type)
        {
            if (type == ResourceTypes.Gold)
                label_gold.Text = "Золото: " + count.ToString();
            if (type == ResourceTypes.Wood)
                label_timber.Text = "Древесина" + count.ToString();
        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            Point pointOnMap = PointOnMap(e.X, e.Y);
            MoveKing(pointOnMap.X, pointOnMap.Y);
        }

        public Bitmap Bitmap
        {
            get { return bitmap; }
        }

        public Image GameScene
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }

        public GameData Context
        {
            get { return context; }
            set { context = value; }
        }

        public Dictionary<long, IObserver> Observers
        {
            get { return observers; }
        }

        public BigMapGraphicsManager GraphicManager
        {
            get { return grManager; }
            set { grManager = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QueryManager.SendGetUnionOrEmpireInfo(game.Player);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QueryManager.SendGetAliancesInfo(game.Player);
        }

        public void ShowAliances(List<GetAliancesInfoResponse.AlianceInfo> a)
        {
            listBox1.Items.Clear();
            a.ForEach(x =>
                {
                    listBox1.Items.Add(x.Id);
                });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(listBox1.SelectedItem);
            QueryManager.SendJoinToUnionOrEmpire(game.Player, i);
        }
    }
}
