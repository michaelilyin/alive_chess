using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Landscapes;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using GameModel.Utils;
using GameModel.CastleManagers;
using AliveChessLibrary.GameObjects.Characters;

namespace GameModel
{
    public class World
    {
        public World()
        {
            _player = new Player();
            Steps = new Queue<Position>();
            //IsCreated = false;
            //IsGameStateUpdated = false;
            //IsObjectsUpdated = false;
        }

        private Map _map;
        public Map Map
        {
            get
            {
                IsCreated = false;
                //IsObjectsUpdated = false;
                return _map;
            }
        }
        private Player _player;
        public Player Player
        {
            get
            {
                //IsGameStateUpdated = false;
                return _player;
            }
        }
        public bool IsCreated { get; private set; }
        //public bool IsGameStateUpdated { get; private set; }
        //public bool IsObjectsUpdated { get; private set; }
        //public event EventHandler Created;
        public event EventHandler GameStateUpdated;
        public event EventHandler ObjectsUpdated;
        public Queue<Position> Steps { get; private set; }
        public Dictionary<ResourceTypes, int> PlayerResources { get; private set; }

        internal void Create(GetMapResponse data)
        {
            _map = new Map(data.SizeMapX, data.SizeMapY);
            Log.Message(String.Format("Map created with params width={0} heigth={0}", _map.SizeX, _map.SizeY));
            lock (_map.BasePoints)
            {
                if (data.BasePoints != null)
                {
                    foreach (var basePoint in data.BasePoints)
                    {
                        _map.AddBasePoint(basePoint);
                        Log.Message(String.Format("Creating base point: landscape <type: {0}>, <type: {1}>, <x: {2}>, <y: {3}>, <id: {4}>, <image id: {5}>, <way cost: {6}>", 
                            basePoint.LandscapeType, basePoint.Type, basePoint.X, basePoint.Y, basePoint.Id, basePoint.ImageId, basePoint.WayCost));
                    }
                }
            }
            lock (_map.Borders)
            {
                if (data.Borders != null)
                {
                    foreach (var border in data.Borders)
                    {
                        _map.AddBorder(border);
                        Log.Message(String.Format("Creating border <id: {0}>, <image id: {1}>, <type: {2}>, <x: {3}>, <y: {4}>", 
                            border.Id, border.ImageId, border.Type, border.X, border.Y));
                    }
                }
            }
            lock (_map.Castles)
            {
                if (data.Castles != null)
                {
                    foreach (var castle in data.Castles)
                    {
                        castle.BuildingManager = new ProductionManager<InnerBuildingType>();
                        castle.RecruitingManager = new ProductionManager<UnitType>();
                        _map.AddCastle(castle);
                        Log.Message(String.Format("Creating castle <x: {0}>, <y: {1}>, <type: {2}>, <width: {3}>, <height: {4}>", 
                            castle.X, castle.Y, castle.Type, castle.Width, castle.Height));
                    }
                }
            }
            lock (GameCore.Instance.World.Map.Mines)
            {
                if (data.Mines != null)
                {
                    foreach (var mine in data.Mines)
                    {
                        _map.AddMine(mine);
                        Log.Message(String.Format("Creating mine <x: {0}>, <y: {1}>, <width: {2}>, <height: {3}>, <type: {4}>", 
                            mine.X, mine.Y, mine.Width, mine.Height, mine.MineType));
                    }
                }
            }
            lock (GameCore.Instance.World.Map.SingleObjects)
            {
                if (data.SingleObjects != null)
                {
                    foreach (var singleObject in data.SingleObjects)
                    {
                        _map.AddSingleObject(singleObject);
                        Log.Message(String.Format("Creating single object <x: {0}>, <y: {1}>, <type: {2}>", 
                            singleObject.X, singleObject.Y, singleObject.SingleObjectType));
                    }
                }
            }
            lock (GameCore.Instance.World.Map.MultyObjects)
            {
                if (data.MultyObjects != null)
                {
                    foreach (var multyObject in data.MultyObjects)
                    {
                        _map.AddMultyObject(multyObject);
                        Log.Message(String.Format("Creating multi obg <type: {0}>, <x: {1}>, <y: {2}>, <width: {3}>, <height: {4}>",
                            multyObject.MultyObjectType, multyObject.X, multyObject.Y, multyObject.Width, multyObject.Height));
                    }
                }
            }
            _map.Fill();
            IsCreated = true;
            //if (Created != null)
            //    Created(this, new EventArgs());
            Log.Message("Map filled");
        }
        internal void UpdateGameState(GetGameStateResponse data)
        {
            if (data.King != null)
            {
                if (_player.King == null)
                {
                    _player.King = data.King;
                }
                else
                {
                    lock (_player)
                    {
                        _player.King.Sync(data.King);
                        //_player.King.X = data.King.X;
                        //_player.King.Y = data.King.Y;
                        //_player.King.Experience = data.King.Experience;
                        //_player.King.MilitaryRank = data.King.MilitaryRank;
                        if (_player.King.ResourceStore == null)
                            _player.King.ResourceStore = new ResourceStore();
                        _player.SetResources(data.Resources);
                    }
                }
                //IsGameStateUpdated = true;
                if (GameStateUpdated != null)
                    GameStateUpdated(this, new EventArgs());
            }       
        }
        internal void UpdateObjects(GetObjectsResponse data)
        {
            lock (_map.Resources)
            {
                _map.Resources = CollectionConverter.ListToEntitySet(data.Resources);
            }
            lock (_map.Kings)
            {
                _map.Kings = CollectionConverter.ListToEntitySet(data.Kings);
                //_map.Kings.Add(GameCore.Instance.Player.King);
            }

            if (data.Castles != null)
            {
                lock (_map.Castles)
                {
                    foreach (var castle in data.Castles)
                    {
                        Castle oldCastle = _map.SearchCastleById(castle.Id);
                        if (oldCastle != null)
                        {
                            oldCastle.KingId = castle.KingId;
                            //oldCastle.SetBuildings(castle.GetInnerBuildingListCopy());
                            //foreach (var a in castle.GetInnerBuildingListCopy())
                            //{
                            //    Log.Debug(a.InnerBuildingType.ToString());
                            //}
                        }
                        else
                        {
                            castle.BuildingManager = new ProductionManager<InnerBuildingType>();
                            castle.RecruitingManager = new ProductionManager<UnitType>();
                            _map.AddCastle(castle);
                        }
                    }
                }
            }

            if (data.Mines != null)
            {
                lock (_map.Mines)
                {
                    foreach (var mine in data.Mines)
                    {
                        Mine oldMine = _map.SearchMineById(mine.Id);
                        if (oldMine != null)
                        {
                            oldMine.KingId = mine.KingId;
                            oldMine.X = mine.X;
                            oldMine.Y = mine.Y;
                            oldMine.Width = mine.Width;
                            oldMine.Height = mine.Height;
                            oldMine.WayCost = mine.WayCost;
                            oldMine.MiningResource = mine.MiningResource;
                            oldMine.Capacity = mine.Capacity;
                        }
                        else
                        {
                            _map.AddMine(mine);
                        }
                    }
                }
            }
            //IsObjectsUpdated = true;
            if (ObjectsUpdated != null)
                ObjectsUpdated(this, new EventArgs());
        }
        internal void SetPlayerSteps(MoveKingResponse data)
        {
            Steps.Clear();
            foreach (Position pos in data.Steps)
            {
                Steps.Enqueue(pos);
            }

        }
    }
}
