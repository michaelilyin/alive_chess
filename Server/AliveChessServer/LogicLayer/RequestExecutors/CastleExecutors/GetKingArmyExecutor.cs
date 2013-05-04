using System.Collections.Generic;
using System.Linq;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetKingArmyExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public GetKingArmyExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            GetKingArmyRequest request = (GetKingArmyRequest)msg.Command;
           // PlayerInfo info = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            List<Unit> arm = msg.Sender.King.Units.ToList();
            //_queryManager.SendGetArmyToKing(info, arm);
            msg.Sender.Messenger.SendNetworkMessage(new GetKingArmyResponse() { Units = arm });
        }
    }
}
