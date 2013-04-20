using System;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Utility;
using AliveChessServer.LogicLayer.Environment.Aliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.Environment
{
    public class EconomyRoutine : IRoutine
    {
        private Level _level;
        private GameWorld _environment;
        private PlayerManager playerManager;

        public EconomyRoutine(GameData data, GameWorld environment)
        {
            this._environment = environment;
        }

        public void DoLogic(GameTime time)
        {
            // foreach (Level level in _environment.LevelManager.NextLevel())
            _level.Emperies.ForEach(
                x =>
                    {
                        if (x.CurrentTimePeriod >= Empire.TaxPeriod)
                            x.TakeTax();
                        else x.CurrentTimePeriod += TimeSpan.FromMilliseconds(10);
                    });
        }

        public void SendResource(King player, Resource r, bool fromMine)
        {
            player.StartCastle.ResourceStore.AddResourceToRepository(r);
            player.Player.Messenger.SendNetworkMessage(new GetResourceMessage(r, fromMine));
        }

        public PlayerManager PlayerManager
        {
            get { return playerManager; }
            set { playerManager = value; }
        }
    }
}
