using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetRecBuildingsExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
      
        public GetRecBuildingsExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            GetRecBuildingsRequest request = (GetRecBuildingsRequest)msg.Command;
            //PlayerInfo info = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            ResBuild rb = new ResBuild();
            //InnerBuilding b = new InnerBuilding() { InnerBuildingType = request.Type };
            //_queryManager.SendGetResBuildings(info, rb);
            GetRecBuildingsResponse response = new GetRecBuildingsResponse();
            response.ResBuild = rb;
            msg.Sender.Messenger.SendNetworkMessage(response);
        }

    }
}
