using System.Collections.Generic;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetListBuildingsInCastleExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private Player _queryManager;

        public GetListBuildingsInCastleExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;

        }
        public void Execute(Message cmd)
        {
            this._queryManager = cmd.Sender;
            GetListBuildingsInCastleRequest request = (GetListBuildingsInCastleRequest)cmd.Command;
            Player info = _playerManager.GetPlayerInfoById(cmd.Sender.Id);
            int l = info.King.CurrentCastle.InnerBuildings.Count;
            List<InnerBuilding> s = new List<InnerBuilding>();
            for (int i = 0; i < l; i++) s.Add(info.King.CurrentCastle.GetBuildings(i));
            s.Add(new AliveChessLibrary.GameObjects.Buildings.InnerBuildingFactory().Build(new System.Guid("{1A853210-CF28-4F8A-9A77-C25589E93CC6}"),1, InnerBuildingType.Voencomat, "Voenkomat"));
            var response = new GetListBuildingsInCastleResponse();
            response.List = s;// as IList<InnerBuilding>;
            _queryManager.Messenger.SendNetworkMessage(response);
        }
    }
}
