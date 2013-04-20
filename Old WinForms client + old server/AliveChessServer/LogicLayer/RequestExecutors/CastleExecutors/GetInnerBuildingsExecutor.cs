using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetInnerBuildingsExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
      
        public GetInnerBuildingsExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            //BuildingInCastleRequest request = (BuildingInCastleRequest)msg.Command;
            //PlayerInfo plInfo = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            //King king = plInfo.Player.King;
            //king.CurrentCastle.AddBuildings(delegate() { return GuidGenerator.Instance.GeneratePair(); },
            //    request.Type);
            //int l = king.CurrentCastle.SizeListbuilldingsInCastle();
            //List<InnerBuilding> s = new List<InnerBuilding>();
            //for (int i = 0; i < l; i++)
            //{
            //    s.Add(king.CurrentCastle.GetBuildings(i));
            //}

            //_queryManager.SendGetListBuildings(plInfo, s);
        }
    }
}
