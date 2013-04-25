using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using AliveChess.Utilities;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;
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
#warning Создание карты
            _map = new Map(response.SizeMapX, response.SizeMapY);

            EntitySet<BasePoint> basePoints = CustomConverter.L2ES<BasePoint>(response.BasePoints);
            if (basePoints != null)
            {
                foreach (var basePoint in basePoints)
                {
                    _map.AddBasePoint(basePoint);
                }
            }

            EntitySet<Border> borders = CustomConverter.L2ES<Border>(response.Borders);
            if (borders != null)
            {
                foreach (var border in borders)
                {
                    _map.AddBorder(border);
                }
            }

            EntitySet<Castle> castles = CustomConverter.L2ES<Castle>(response.Castles);
            if (castles != null)
            {
                foreach (var castle in castles)
                {
                    _map.AddCastle(castle);
                }
            }

            EntitySet<Mine> mines = CustomConverter.L2ES<Mine>(response.Mines);
            if (mines != null)
            {
                foreach (var mine in mines)
                {
                    _map.AddMine(mine);
                }
            }

            EntitySet<SingleObject> singleObjects = CustomConverter.L2ES<SingleObject>(response.SingleObjects);
            if (singleObjects != null)
            {
                foreach (var singleObject in singleObjects)
                {
                    _map.AddSingleObject(singleObject);
                }
            }

            EntitySet<MultyObject> multyObjects = CustomConverter.L2ES<MultyObject>(response.MultyObjects);
            if (multyObjects != null)
            {
                foreach (var multyObject in multyObjects)
                {
                    _map.AddMultyObject(multyObject);
                }
            }

            _map.Fill();
            //FillMap();
            _hasBeenCreated = true;
        }

        /// <summary>
        /// заполнение карты объектами
        /// </summary>
        public void FillMap()
        {
            for (int x = 0; x < _map.SizeX; x++)
            {
                for (int y = 0; y < _map.SizeY; y++)
                {
                    if (_map[x, y] == null)
                        _map.SetObject(Map.CreatePoint(x, y, PointTypes.None));
                }
            }
            ClientFloodAlgorithm algorithm = new ClientFloodAlgorithm(_map);
            List<BasePoint> newBasePoints = new List<BasePoint>();
            foreach (var basePoint in _map.BasePoints)
            {
                newBasePoints.AddRange(algorithm.Run(basePoint));
            }
            _map.BasePoints.AddRange(newBasePoints);
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
