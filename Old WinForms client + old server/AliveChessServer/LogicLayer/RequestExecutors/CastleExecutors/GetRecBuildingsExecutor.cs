using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetRecBuildingsExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
      
        public GetRecBuildingsExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            //GetRecBuildingsRequest request = (GetRecBuildingsRequest)msg.Command;
            //PlayerInfo info = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            //ResBuild rb = new ResBuild();
            //_queryManager.SendGetResBuildings(info, rb);
        }

    }
}
