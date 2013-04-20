using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class CaptureMineExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
     
        public CaptureMineExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        #region IExecutor Members

        public void Execute(Message msg)
        {
            //CaptureMineRequest request = (CaptureMineRequest)msg.Command;

            //PlayerInfo info = _playerManager.GetPlayerInfoById(msg.Sender.Id);

            //Map map = info.Player.Map;
            //King king = info.Player.Kings[0];
            //Mine mine = map.SearchMine(request.MineId);

            //if ((mine.Player == null) || (mine.Player != null && 
            //    mine.Player.Id != king.Player.Id
            //    && !mine.Player.Kings[0].Sleep))
            //{
            //    if (mine.Player != null && mine.Player.Id != king.Player.Id)
            //    {
            //        Player p = mine.Player;
            //        p.Kings[0].RemoveMine(mine);
            //        PlayerInfo opponentInfo = _playerManager.GetPlayerInfoById(p.Id);
            //        _queryManager.SendLooseMine(opponentInfo, mine);
            //    }

            //    king.Player.Kings[0].AddMine(mine);
            //    mine.King.Player = king.Player;

            //    _queryManager.SendCaptureMine(info, mine);

            //    mine.Activation();
            //}
        }

        #endregion
    }
}
