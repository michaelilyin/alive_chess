using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Interfaces;
using WindowsMobileClientAliveChess.GameLayer.AliveChessGraphics;
using WindowsMobileClientAliveChess.NetLayer.Main;

namespace WindowsMobileClientAliveChess.GameLayer.Controls
{
    public partial class BigMapControl : UserControl
    {
        private Bitmap bitmap;
        private Game context;
        private Queue<MapPoint> stateQueue;
        private Dictionary<long, IObserver> observers;
        private BigMapGraphicsManager grManager;
        private object syncStateQueue = new object();
        private GameTime time = new GameTime();
        private bool pressed;
        private bool click;
        private Point curr_point= new Point();

        public BigMapControl(Game context)
        {
            InitializeComponent();
            mIJoin.Enabled = false;
            pBMap.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            this.context = context;
            this.bitmap = new Bitmap(pBMap.Width, pBMap.Height);
            this.grManager = new BigMapGraphicsManager(context, bitmap);
            this.stateQueue = new Queue<MapPoint>();
            this.observers = new Dictionary<long, IObserver>();
            GraphicManager.Load();
            tGameTime.Interval = 10;
            tGameTime.Tick += new EventHandler(Update);
            context.onGetMap += new AliveChessDelegate(context_onGetMap);
        }

        #region Properties
        public Game Context
        {
            get
            {
                return context;
            }
        }

        public Bitmap Bitmap
        {
            get { return bitmap; }
        }

        public Image GameScene
        {
            get { return pBMap.Image; }
            set { pBMap.Image = value; }
        }

        public Image RecourcesPlace
        {
            get { return pBGold.Image; }
            set { pBGold.Image = value; }
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

        public Player Player
        {
            get { return this.context.Player; }
            set { this.context.Player = value; }
        }
        #endregion

        public void Draw()
        {
            grManager.Draw();
            tGameTime.Enabled = true;
        }
        public void Initialize()
        {
            context.LoadMap();
        }

        public void SetText(string text)
        {
            this.Parent.Text = text;
        }

        public void ActivateUnionButton()
        {
            mIJoin.Enabled = true;
        }

        public void DeactivateUnionButton()
        {
            mIJoin.Enabled = false;
        }

        public void PutNewState(MapPoint o)
        {
            Monitor.Enter(syncStateQueue);
            stateQueue.Enqueue(o);
            Monitor.Exit(syncStateQueue);
        }

        //private TimeSpan time = TimeSpan.Zero;

        public void Update(object sender, EventArgs e)
        {
            time.Update();
            if (time.Elapsed > TimeSpan.FromMilliseconds(50))
            {
                if (stateQueue.Count != 0)
                {
                    Monitor.Enter(syncStateQueue);
                    MapPoint obj = stateQueue.Dequeue();
                    Monitor.Exit(syncStateQueue);
                    Player.Map.SetObject(obj);
                    grManager.Draw();
                }
                if (Player.King != null)
                {
                    Player.King.Update();
                    if (Player.King.Updated)
                        grManager.Draw();
                }
                time.Memorize();
            }
        }

        private Point PointOnMap(int x, int y)
        {
            return new Point(GraphicManager.DrawableArea.X + x / 20,
                GraphicManager.DrawableArea.Y + y / 20);
        }

        private bool IsCanMove(int x, int y)
        {
            return Player.Map[x, y] != null && !(x == Player.King.X && y == Player.King.Y) &&
                Player.Map.Locate(x, y) && Player.King.State == KingState.BigMap;
        }

        public void MoveKing(int x, int y)
        {
            if (IsCanMove(x, y))
            {
                Player.King.IsMove = true;
                QueryManager.SendMoveKing(x, y);
            }
        }

        public void UpdateKingVisibleSpace(King king, PointTypes type)
        {
            QueryManager.SendGetObjectsRequestForConcreteObserver(king, type);
            grManager.CalculateBounds();
        }

        public void ComeInCastle(King king, MapSector castle)
        {
            QueryManager.SendComeInCastle( castle);
        }

        public void CaptureMine(King king, MapPoint mine)
        {
            QueryManager.SendCaptureMine( mine);
        }

        public void CaptureCastle(King king, MapSector castle)
        {
            QueryManager.SendMeetCastle(castle);
        }

        public void MeetKing(King king, MapPoint opponent)
        {
            if (king.Id != opponent.Owner.Id)
                QueryManager.SendMeetKing(opponent);
        }

        public void MeetCastle(King player, MapSector castle)
        {
            QueryManager.SendMeetCastle(castle);
        }

        public void UpdateResource(object rCount, object rType)
        {
            int count = Convert.ToInt32(rCount);
            ResourceTypes type = (ResourceTypes)rType;
            grManager.DrawResourceIcon(type, count, (Bitmap)this.RecourcesPlace);
        }

        public void UpdateMinesCount(int count)
        {
            //upd mines
        }

        public void UpdateCastlesCount(int count)
        {
            //Update castles
        }

        public void UpdateResources(int count, ResourceTypes type)
        {
            grManager.DrawResourceIcon(type, count, (Bitmap)this.RecourcesPlace);
        }

        public void ShowAliances(List<GetAliancesInfoResponse.AlianceInfo> a)
        {
            //alliances
        }

        private void pBMenu_Click(object sender, EventArgs e)
        {
            cMMain.Show((sender as Control), (sender as Control).Location);
        }

        private void mIExit_Click(object sender, EventArgs e)
        {
            QueryManager.SendExit();
            Application.Exit();
        }

        private void mIOnMap_Click(object sender, EventArgs e)
        {
            QueryManager.SendGetAliancesInfo();
        }

        private void pBMap_MouseClick(object sender, MouseEventArgs e)
        {
    
            //    Point pointOnMap = PointOnMap(e.X, e.Y);
              //  MoveKing(pointOnMap.X, pointOnMap.Y);
                //grManager.CalculateBounds();
        }

        private void context_onGetMap()
        {
            Invoke(new AliveChessDelegate(QueryManager.SendGetObjectsRequestForPlayer));
        }

        private void pBMap_MouseDown(object sender, MouseEventArgs e)
        {
            pressed = true;
            click = true;
            curr_point = PointOnMap(e.X, e.Y);
        }

        private void moveMap(int x, int y)
        {
            if (x >= 0 && y>= 0 && x + grManager.DrawableArea.Width <= context.Player.Map.SizeX && y + grManager.DrawableArea.Height <= context.Player.Map.SizeY)
            {
                grManager.DrawableArea = new Rectangle(x, y, grManager.DrawableArea.Height, grManager.DrawableArea.Width);
                grManager.Draw();
            }
 
        }
        private void pBMap_MouseMove(object sender, MouseEventArgs e)
        {
            int x = grManager.DrawableArea.X;
            int y = grManager.DrawableArea.Y;
            int w = grManager.DrawableArea.Width;
            int h = grManager.DrawableArea.Height;


            if (e.X < 20)
            {
                moveMap(x - 1, y);
            }
            if (e.Y < 20)
            {
                moveMap(x, y - 1);
            }
            if (e.X > Screen.PrimaryScreen.WorkingArea.Width-20)
            {
                moveMap(x+1, y);
            }
            if (e.Y > Screen.PrimaryScreen.WorkingArea.Height - 70)
            {
                moveMap(x, y +1);
            }
            
            
        }

        private void pBMap_Click(object sender, EventArgs e)
        {

        }
    }
}
