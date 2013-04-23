using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.Commands.BigMapCommand;
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
            _map.BasePoints = CustomConverter.L2ES<BasePoint>(response.BasePoints);
            _map.Borders = CustomConverter.L2ES<Border>(response.Borders);
            _map.Castles = CustomConverter.L2ES<Castle>(response.Castles);
            _map.Mines = CustomConverter.L2ES<Mine>(response.Mines);
            _map.SingleObjects = CustomConverter.L2ES<SingleObject>(response.SingleObjects);
            _map.MultyObjects = CustomConverter.L2ES<MultyObject>(response.MultyObjects);
            _map.Fill();
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
