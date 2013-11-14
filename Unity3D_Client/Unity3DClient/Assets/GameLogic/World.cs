using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Landscapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Data.Linq;

namespace Assets.GameLogic
{
    public class World
    {
        private Map _map;
        public Map Map
        {
            get { return _map; }
        }
        private bool _updated;
        public bool Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }

        public World()
        {
            Updated = false;
        }

        public void Create(GetMapResponse response)
        {
            _map = new Map(response.SizeMapX, response.SizeMapY);
            Debug.Log(String.Format("Map size x = {0}; y = {1}", response.SizeMapX, response.SizeMapY));
            lock (GameCore.Instance.World.Map.BasePoints)
            {
                if (response.BasePoints != null)
                {
                    foreach (var basePoint in response.BasePoints)
                    {
                        _map.AddBasePoint(basePoint);
                        Debug.Log(String.Format("Base point {0}:{1}:{2}:{3}:{4}:{5}:{6}", basePoint.LandscapeType, basePoint.Type, basePoint.X, basePoint.Y, basePoint.Id, basePoint.ImageId, basePoint.WayCost));
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
                        Debug.Log(String.Format("Border {0}:{1}:{2}:{3}:{4}", border.Id, border.ImageId, border.Type, border.X, border.Y));
                    }
                }
            }
            lock (GameCore.Instance.World.Map.Castles)
            {
                if (response.Castles != null)
                {
                    foreach (var castle in response.Castles)
                    {
                        //castle.BuildingManager = new ProductionManager<InnerBuildingType>();
                        //castle.RecruitingManager = new ProductionManager<UnitType>();
                        _map.AddCastle(castle);
                       // Debug.Log(String.Format("Castle {0}:{1}:{2}:{3}:{4}", castle.X, castle.Y, castle.Type, castle.Width, castle.Height));
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
                       // Debug.Log(String.Format("Mine {0}:{1}:{2}:{3}:{4}", mine.X, mine.Y, mine.Width, mine.Height, mine.MineType));
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
                       // Debug.Log(String.Format("Single obg {0}:{1}:{2}", singleObject.X, singleObject.Y, singleObject.SingleObjectType));
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
                       // Debug.Log(String.Format("Multi obg {0}:{1}:{2}:{3}:{4}", multyObject.MultyObjectType, multyObject.X, multyObject.Y, multyObject.Width, multyObject.Height));
                    }
                }
            }
            _map.Fill();
            Updated = true;
        }
    }
}
