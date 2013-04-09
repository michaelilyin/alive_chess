using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Landscapes;

namespace WP7_Client.LogicLayer
{
    public class GameWorld
    {
        private bool _hasBeenCreated;
        public Player Player { get; set; }

        public void Create(GetMapResponse response)
        {
            Map = new Map(response.SizeMapX, response.SizeMapY)
                       {
                           BasePoints = response.BasePoints,
                           Borders = response.Borders,
                           Castles = response.Castles,
                           Mines = response.Mines,
                           SingleObjects = response.SingleObjects,
                           MultyObjects = response.MultyObjects
                       };


            _hasBeenCreated = true;
        }

        public bool HasBeenCreated
        {
            get { return _hasBeenCreated; }
            set { _hasBeenCreated = value; }
        }

        public Map Map { get; private set; }
    }
}
