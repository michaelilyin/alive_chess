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
            //GetKingArmyRequest request = (GetKingArmyRequest)msg.Command;
            Player player = msg.Sender;

            GetKingArmyResponse response = new GetKingArmyResponse();
            response.Units = player.King.Army.GetUnitListCopy();
            player.Messenger.SendNetworkMessage(response);
        }
    }
}
