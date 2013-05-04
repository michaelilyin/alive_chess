using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetBuildingCostExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
      
        public GetBuildingCostExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            GetBuildingCostRequest request = (GetBuildingCostRequest)msg.Command;
            //PlayerInfo info = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            CreationCost rb = new CreationCost();
            //InnerBuilding b = new InnerBuilding() { InnerBuildingType = request.Type };
            //_queryManager.SendGetResBuildings(info, rb);
            GetBuildingCostResponse response = new GetBuildingCostResponse();
            response.BuildingCost = rb;
            msg.Sender.Messenger.SendNetworkMessage(response);
        }

    }
}
