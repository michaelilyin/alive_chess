using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetListBuildingsInCastleExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        
        public GetListBuildingsInCastleExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }
        public void Execute(Message cmd)
        {
            //GetListBuildingsInCastleRequest request = (GetListBuildingsInCastleRequest)cmd.Command;
            //PlayerInfo info = _playerManager.GetPlayerInfoById(cmd.Sender.Id);
            //int l = info.Player.King.CurrentCastle.InnerBuildings.Count;
            //List<InnerBuilding> s = new List<InnerBuilding>();
            //for (int i = 0; i < l; i++) s.Add(info.Player.King.CurrentCastle.GetBuildings(i));
            //_queryManager.SendGetListBuildings(info, s);
        }
    }
}
