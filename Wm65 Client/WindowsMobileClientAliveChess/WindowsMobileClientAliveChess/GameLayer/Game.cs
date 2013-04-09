using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using WindowsMobileClientAliveChess.GameLayer.Forms;
using WindowsMobileClientAliveChess.GameLayer.Controls;
using WindowsMobileClientAliveChess.NetLayer.Main;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Utility;


namespace WindowsMobileClientAliveChess.GameLayer
{
    public delegate void AliveChessDelegate();
    public delegate void AliveChessDelegateWithID(int id);
    public delegate void AliveChessDelegateWithText(string text);

    /// <summary>
    /// Base game class. Contains all game's objects, 
    /// game scenes (controls) and realize actions of player
    /// </summary>
    public class Game
    {
        private GameForm gameForm;
        private StartForm startForm;
        private ObjectComparer comparer;
        private VisibleSpace _vsManager;

        private List<GetAlianceInfoResponse.MemberInfo> members;
        private List<GetAliancesInfoResponse.AlianceInfo> aliances;

        private Map map;
        private Player player;

        private Battle battle;
        private Dialog dispute;
        private Negotiate negotiate;
        private List<Resource> resource;

        private int _unionId;
        private bool _authorized = false;
        private bool _ready = false;
        public event AliveChessDelegate OnSuccesAuthorization;
        public event AliveChessDelegate OnGameReady;
        public event AliveChessDelegate onGetMap;

        public bool Authorized
        {
            get { return _authorized; }
            set { _authorized = value; }
        }
        private object unionIdSync = new object();
        private object memberSync = new object();
        private object aliancesSync = new object();

        public Game()
        {
            this.player = new Player();
            this.gameForm = new GameForm(this);
            this.startForm = new StartForm(this);
            this.resource = new List<Resource>();
            this.comparer = new ObjectComparer();
        }


        public void SetInitRes(List<Resource> res)
        {
            if (resource != null)
                resource = res;
        }

        public void Run()
        {
            this.BigMap.Initialize();
            this.GameForm.StartBigMap();
            this.GameForm.ShowDialog();
        }

        public void Stop()
        {
            this.GameForm.Hide();
        }

        public void LoadMap()
        {
            QueryManager.SendGetMapRequest();
        }

        public void InitMap(List<BasePoint> lt, List<MapPoint> points, List<MapSector> sectors)
        {
            if (points != null)
            {
                foreach (MapPoint o in points)
                {
                    map.SetObject(o);
                }
            }

            if (sectors != null)
            {
                foreach (MapSector s in sectors)
                {
                    map.InitializeSector(s);
                }
            }

            if (lt != null)
            {
                foreach (BasePoint pws in lt)
                {
                    FloodFillAlgorithm f = new FloodFillAlgorithm(map, pws, pws.LandscapeType);
                    f.Run(pws.X, pws.Y);
                }
            }
        
            ImageInfo image = new ImageInfo();
            image.ImageId = 0;

            for (int i = 0; i < map.SizeX; i++)
            {
                for (int j = 0; j < map.SizeY; j++)
                {
                    if (map[i, j] == null)
                    {
                        MapPoint p = Map.CreatePoint(i, j, PointTypes.Landscape);
                        map.SetObject(p);                    
                    }
                }
            }
        }
        public void HandleSuccesAuthorization()
        {
            if (OnSuccesAuthorization!= null) 
            OnSuccesAuthorization();
        }
        public void HandleReady()
        {
            if (OnGameReady!= null)
            {
                OnGameReady();
            }
        }

        public void HandleGetMap()
        {
            if (onGetMap!=null)
             {
                 onGetMap();
             }
        }

        public bool ContainsEntity(MapPoint obj)
        {
            if (player.King.ViewOnMap.Owner.Id == obj.Owner.Id) return true;

            for (int i = 0; i < map.SizeX; i++)
            {
                for (int j = 0; j < map.SizeY; j++)
                {
                    if (map[i, j] != null)
                    {
                        if (map[i, j].Owner.Id == obj.Owner.Id && map[i, j].X == obj.X &&
                            map[i, j].Y == obj.Y)
                            return true;
                    }
                }
            }

            return false;
        }

        public void RefreshMap()
        {
            for (int i = 0; i < map.SizeX; i++)
            {
                for (int j = 0; j < map.SizeY; j++)
                {
                    MapPoint mo = map[i, j];
                    if (mo != null)
                    {
                        if (!player.VisibleSpace.Contains(mo.X,
                            mo.Y) && (mo.MapPointType == PointTypes.King ||
                            mo.MapPointType == PointTypes.Resource))
                        {
                            DeleteEntityFromModel(mo);
                        }
                    }
                }
            }
        }

        public void DeleteEntityFromModel(MapPoint obj)
        {
            map.SetObject(obj);
        }

        public void AddCastle(Castle castle)
        {
            player.King.AddCastle(castle);
        }

        public void RemoveCastle(int castleId)
        {
            player.King.RemoveCastle(castleId);
        }

        public void AddMine(Mine mine)
        {
            player.King.AddMine(mine);
        }

        public void RemoveMine(int mineId)
        {
            player.King.RemoveMine(mineId);
        }

        public BigMapControl BigMap
        {
            get { return gameForm.BigMapControl; }
        }

        public MainDisputeControl DisputeControl
        {
            get { return gameForm.MainDisputControl; }
        }

        public CastleControl CastleControl
        {
            get { return gameForm.CastleControl; }
        }

        public Map Map
        {
            get { return map; }
            set { map = value; }
        }

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public Battle Battle
        {
            get { return battle; }
            set { battle = value; }
        }

        public Dialog Dispute
        {
            get { return dispute; }
            set { dispute = value; }
        }

        public bool Ready
        {
            get { return _ready; }
            set { _ready = value; }
        }

        public Negotiate Negotiate
        {
            get { return negotiate; }
            set { negotiate = value; }
        }

        public GameForm GameForm
        {
            get { return gameForm; }
        }

        public StartForm StartForm
        {
            get { return startForm; }
        }


        public VisibleSpace VisibleSpace
        {
            get { return _vsManager; }
            set { _vsManager = value; }
        }

        public int UnionId
        {
            get
            {
                lock (unionIdSync)
                    return _unionId;
            }
            set
            {
                lock (unionIdSync)
                    _unionId = value;
            }
        }

        private class ObjectComparer : IEqualityComparer<MapPoint>
        {
            public bool Equals(MapPoint x, MapPoint y)
            {
                return x.Owner.Id == y.Owner.Id;
            }

            public int GetHashCode(MapPoint obj)
            {
                return obj.Owner.Id.GetHashCode();
            }
        }

        public List<GetAlianceInfoResponse.MemberInfo> Members
        {
            get { return members; }
            set { lock (memberSync) members = value; }
        }

        public List<GetAliancesInfoResponse.AlianceInfo> Aliances
        {
            get { return aliances; }
            set { lock (aliancesSync) aliances = value; }
        }
    }
}
