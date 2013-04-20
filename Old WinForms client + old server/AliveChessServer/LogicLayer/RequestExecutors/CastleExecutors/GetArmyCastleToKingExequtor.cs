using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    class GetArmyCastleToKingExequtor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
      
        public GetArmyCastleToKingExequtor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }
        public void Execute(Message msg)
        {
            //GetArmyCastleToKingRequest request = (GetArmyCastleToKingRequest)msg.Command;
            //PlayerInfo info = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            //info.Player.King.CurrentCastle.GetArmyToKing();
            //List<Unit> arm = info.Player.King.Units.ToList();
            //_queryManager.SendGetArmyToKing(info, arm);
        }
    }
}
