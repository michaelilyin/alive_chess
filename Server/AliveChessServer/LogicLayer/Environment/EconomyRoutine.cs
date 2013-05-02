using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.LogicLayer.Environment
{
    public class EconomyRoutine : IRoutine
    {
        private Level _level;
        private PlayerManager playerManager;
        private TimeManager _timeManager;

        public EconomyRoutine(TimeManager timeManager)
        {
            this._timeManager = timeManager;
        }

        public void Update()
        {
           //
        }
        
        public void SendResource(King player, Resource r, bool fromMine)
        {
            player.StartCastle.ResourceStore.AddResourceToRepository(r);
            //player.Player.Messenger.SendNetworkMessage(new GetResourceMessage(r, fromMine));
        }

        public PlayerManager PlayerManager
        {
            get { return playerManager; }
            set { playerManager = value; }
        }
    }
}
