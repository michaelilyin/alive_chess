using System;
using System.Collections.Generic;
using System.Diagnostics;
using AliveChessClient.GameLayer.Controls;
using AliveChessClient.GameLayer.Forms;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Utility;

namespace AliveChessClient.GameLayer
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
        //private List<MapObject> _objects;
        private ObjectComparer comparer;

        private List<GetAlianceInfoResponse.MemberInfo> members;
        private List<GetAliancesInfoResponse.AlianceInfo> aliances;

        private Map map;
        private Player player;

        private Battle battle;
        private Dialog dispute;
        private Negotiate negotiate;

        private int _unionId;
        private object unionIdSync = new object();
        private object memberSync = new object();
        private object aliancesSync = new object();

        public Game()
        {
            this.player = new Player();
            //this._objects = new List<MapObject>();

            this.gameForm = new GameForm(this);
            this.startForm = new StartForm(this);

            this.comparer = new ObjectComparer();

            Debug.Assert(BigMap.Context != null);
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
            QueryManager.SendGetMapRequest(player);
        }

        public void InitMap(List<BasePoint> lt, List<MapPoint> points, List<MapSector> sectors)
        {

/*
            private List<Castle> _castles;
        [ProtoMember(5)]
        private List<Mine> _mines;
        [ProtoMember(6)]
        private List<BasePoint> _basePoints;
        [ProtoMember(7)]
        private List<SingleObject> _singleObjects;
        [ProtoMember(8)]
        private List<MultyObject> _multyObjects;
        [ProtoMember(9)]
        private List<Border> _borders;*/




            if (points != null)
            {
                foreach (MapPoint o in points)
                {
                    map.SetObject(o);
                    if (o.PointType == PointTypes.Resource)
                        o.ObjectUnderThis = Map.CreatePoint(o.X, o.Y, o.ImageId,
                            PointTypes.Landscape, null, 0);
                    //if (player.IsSuperUser) o.Detected = true;
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
                    FloodFillAlgorithm f = new FloodFillAlgorithm(map, pws, pws.LandscapeType, BigMap.Context);
                    f.Run(pws.X, pws.Y);
                }
            }

            for (int k = 0; k < map.SizeX; k++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    /*if (map[k, j] != null)
                        //map.SetObject(CreateMapPoint(0, k, j, 0, null, PointTypes.Landscape, null, 0));
                        if (player.IsSuperUser) map[k, j].Detected = true;*/
                }

            ImageInfo image = new ImageInfo();
            image.ImageId = 0;

            for (int i = 0; i < map.SizeX; i++)
            {
                for (int j = 0; j < map.SizeY; j++)
                {
                    if (map[i, j] == null)
                    {
                        MapPoint p = Map.CreatePoint(i, j, image, PointTypes.Landscape, null, 0);
                        map.SetObject(p);
                        //if (player.IsSuperUser) p.Detected = true;
                    }
                }
            }
        }

        public bool ContainsEntity(MapPoint obj)
        {
            if (player.King.ViewOnMap.Id == obj.Id) return true;

            for (int i = 0; i < map.SizeX; i++)
            {
                for (int j = 0; j < map.SizeY; j++)
                {
                    if (map[i, j] != null)
                    {
                        if (map[i, j].Id == obj.Id && map[i, j].X == obj.X &&
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
                        if (mo.Detected && !player.VisibleSpace.Contains(mo.X,
                            mo.Y) && (mo.PointType == PointTypes.King ||
                            mo.PointType == PointTypes.Resource))
                        {
                            if (mo.ObjectUnderThis == null)
                                Debugger.Break();

                            DeleteEntityFromModel(mo);
                        }
                    }
                }
            }
        }

        public void DeleteEntityFromModel(MapPoint obj)
        {
            map.SetObject(obj.ObjectUnderThis);
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
                return x.Id == y.Id;
            }

            public int GetHashCode(MapPoint obj)
            {
                return obj.Id.GetHashCode();
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
