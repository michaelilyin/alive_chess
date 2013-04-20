using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class ShowArmyCastleExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public ShowArmyCastleExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }
        public void Execute(Message msg)
        {
            //ShowArmyCastleRequest request = (ShowArmyCastleRequest)msg.Command;
            //PlayerInfo info = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            //List<Unit> arm = info.Player.King.CurrentCastle.ArmyInsideCastle;
            //_queryManager.SendGetArmyCastle(info, arm);

        }
    }
}
