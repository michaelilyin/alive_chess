using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Landscapes;

namespace AliveChess.GameLayer.LogicLayer
{
    public class GameWorld
    {
        private Map _map;
        private bool _hasBeenCreated;

        public void Create(GetMapResponse response)
        {
            _map = new Map(response.SizeMapX, response.SizeMapY);

            _map.BasePoints = response.BasePoints;
            _map.Borders = response.Borders;
            _map.Castles = response.Castles;
            _map.Mines = response.Mines;
            _map.SingleObjects = response.SingleObjects;
            _map.MultyObjects = response.MultyObjects;

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
