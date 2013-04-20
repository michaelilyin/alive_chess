using System.Collections.Generic;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.Environment.Aliances
{
    public class Publisher
    {
        private List<IPlayer> _receivers;
        private PlayerManager _playerManager;

        public Publisher(PlayerManager playerManager)
        {
            this._playerManager = playerManager;
            this._receivers = new List<IPlayer>();
        }

        public void AddReceiver(King receiver)
        {
            _receivers.Add(receiver.Player);
        }

        public void AddReceivers(List<King> receivers)
        {
            receivers.ForEach(x => _receivers.Add(x.Player));
        }

        public virtual void PublishNews(Player sender, NewsType type, string message)
        {
            foreach (IPlayer r in _receivers)
                if (!r.Bot)
                    r.Messenger.SendNetworkMessage(
                        new MessageNewsMessage(type, sender != null ? sender.Id : 0, message));
        }

        public List<IPlayer> Receivers { get { return _receivers; } }
    }
}
