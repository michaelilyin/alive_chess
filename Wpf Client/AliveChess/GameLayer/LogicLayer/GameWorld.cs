using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
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

            if (response.BasePoints != null)
            {
                foreach (var basePoint in response.BasePoints)
                {
                    _map.AddBasePoint(basePoint);
                }
            }

            if (response.Borders != null)
            {
                foreach (var border in response.Borders)
                {
                    _map.AddBorder(border);
                }
            }

            if (response.Castles != null)
            {
                foreach (var castle in response.Castles)
                {
                    _map.AddCastle(castle);
                }
            }

            if (response.Mines != null)
            {
                foreach (var mine in response.Mines)
                {
                    _map.AddMine(mine);
                }
            }

            if (response.SingleObjects != null)
            {
                foreach (var singleObject in response.SingleObjects)
                {
                    _map.AddSingleObject(singleObject);
                }
            }

            if (response.MultyObjects != null)
            {
                foreach (var multyObject in response.MultyObjects)
                {
                    _map.AddMultyObject(multyObject);
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
