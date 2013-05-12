using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using System.Collections.Generic;
using System.Data.Linq;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetCastleArmyExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public GetCastleArmyExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }
        public void Execute(Message msg)
        {
            Player player = msg.Sender;
            GetCastleArmyRequest request = (GetCastleArmyRequest)msg.Command;
            GetCastleArmyResponse response = new GetCastleArmyResponse();
            response.Units = player.King.CurrentCastle.Army.GetUnitListCopy();
            player.Messenger.SendNetworkMessage(response);

        }
    }
}
