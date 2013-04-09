using System.Collections.Generic;
using System.Linq;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class ShowArmyKingExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public ShowArmyKingExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            ShowArmyKingRequest request = (ShowArmyKingRequest)msg.Command;
           // PlayerInfo info = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            List<Unit> arm = msg.Sender.King.Units.ToList();
            //_queryManager.SendGetArmyToKing(info, arm);
            msg.Sender.Messenger.SendNetworkMessage(new ShowArmyKingResponse() { Army_list = arm });
        }
    }
}
