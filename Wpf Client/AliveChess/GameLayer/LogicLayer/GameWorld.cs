using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Objects;

namespace AliveChess.GameLayer.LogicLayer
{
    public class GameWorld
    {
        private Map _map;
        private bool _hasBeenCreated;

        public void Create(GetMapResponse response)
        {
            _map = new Map(response.SizeMapX, response.SizeMapY);

            lock (GameCore.Instance.World.Map.BasePoints)
            {
                if (response.BasePoints != null)
                {
                    foreach (var basePoint in response.BasePoints)
                    {
                        _map.AddBasePoint(basePoint);
                    }
                }
            }

            lock (GameCore.Instance.World.Map.Borders)
            {
                if (response.Borders != null)
                {
                    foreach (var border in response.Borders)
                    {
                        _map.AddBorder(border);
                    }
                }
            }

            lock (GameCore.Instance.World.Map.Castles)
            {
                if (response.Castles != null)
                {
                    foreach (var castle in response.Castles)
                    {
                        castle.BuildingManager = new ProductionManager<InnerBuildingType>();
                        castle.RecruitingManager = new ProductionManager<UnitType>();
                        _map.AddCastle(castle);
                    }
                }
            }

            lock (GameCore.Instance.World.Map.Mines)
            {
                if (response.Mines != null)
                {
                    foreach (var mine in response.Mines)
                    {
                        _map.AddMine(mine);
                    }
                }
            }

            lock (GameCore.Instance.World.Map.SingleObjects)
            {
                if (response.SingleObjects != null)
                {
                    foreach (var singleObject in response.SingleObjects)
                    {
                        _map.AddSingleObject(singleObject);
                    }
                }
            }

            lock (GameCore.Instance.World.Map.MultyObjects)
            {
                if (response.MultyObjects != null)
                {
                    foreach (var multyObject in response.MultyObjects)
                    {
                        _map.AddMultyObject(multyObject);
                    }
                }
            }

            _map.Fill();
            //FillMap();
            _hasBeenCreated = true;
        }

        public bool HasBeenCreated
        {
            get { return _hasBeenCreated; }
            set { _hasBeenCreated = value; }
        }

        public Map Map
        {
            get { return _map; }
        }
    }
}
