using System;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessServer.LogicLayer.EconomyEngine;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.LogicLayer.Environment
{
    public class EconomyRoutine : ITimeRoutine
    {
        private Level _level;
        private PlayerManager _playerManager;
        private TimeManager _timeManager;
        private Economy _economy;

        public EconomyRoutine(Level level, TimeManager timeManager)
        {
            _level = level;
            _timeManager = timeManager;
        }

        public void Initialize(Economy economy)
        {
            _economy = economy;
            foreach (var castle in _level.Map.Castles)
            {
                castle.BuildingFactory = new BuildingFactory(_economy);
                castle.UnitFactory = new UnitFactory(_economy);
            }
        }

        public void Update(GameTime time)
        {
            if (time.Elapsed > TimeSpan.FromMilliseconds(50))
            {

                time.SavePreviousTimestamp();
            }
        }
        
        public void SendResource(King player, Resource r, bool fromMine)
        {
            player.ResourceStore.AddResourceToStore(r);
            //player.Player.Messenger.SendNetworkMessage(new GetResourceMessage(r, fromMine));
        }

        public PlayerManager PlayerManager
        {
            get { return _playerManager; }
            set { _playerManager = value; }
        }

        public Economy Economy
        {
            get { return _economy; }
        }
    }
}
