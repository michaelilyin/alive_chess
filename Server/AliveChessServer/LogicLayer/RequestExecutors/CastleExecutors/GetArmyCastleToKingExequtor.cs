using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;
using System.Collections.Generic;
using System.Data.Linq;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    class GetArmyCastleToKingExequtor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private Player _queryManager;
      
        public GetArmyCastleToKingExequtor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }
        public void Execute(Message msg)
        {
            _queryManager = msg.Sender;
            GetArmyCastleToKingRequest request = (GetArmyCastleToKingRequest)msg.Command;
            //PlayerInfo info = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            msg.Sender.King.CurrentCastle.GetArmyToKing();
            EntitySet<Unit> arm = msg.Sender.King.Units;
            List<Unit> resp_list = new List<Unit>();
            foreach(var u in arm)
            {
                resp_list.Add(u);
            }
            GetArmyCastleToKingResponse response = new GetArmyCastleToKingResponse();
            response.Arm = resp_list;
            _queryManager.Messenger.SendNetworkMessage(response);
        }
    }
}
