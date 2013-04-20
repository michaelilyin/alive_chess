using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BattleExecutors
{
    public class PlayerMoveRequestExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        //byte[,] Fild = new byte[8, 8];
        private Battle _dispute;
        private BattleRoutine BR;

        public PlayerMoveRequestExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            //this.BR = gameLogic.Environment.BattleRoutine;
        }
        public void Execute(Message msg)
        {
            //bool temp = true;

            //PlayerMoveRequest request = (PlayerMoveRequest)msg.Command;
            //King king1 = msg.Sender.King;
            //_dispute = _environment.BattleRoutine.GetBattleById(request.BattleID);
            //King king2 = _environment.BattleRoutine.GetOpponent(_dispute, msg.Sender.King);
            //PlayerInfo plInfo = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            //PlayerInfo opponentInfo = _playerManager.GetPlayerInfoById(request.OpponentID);
            //byte[] t = BR.UserMov(request.Move);
            //bool ok = true;
            //if (t == null)
            //{
            //    if (request.Ok)
            //    {
            //        _queryManager.SendPlayerMove(plInfo, request.Move, BR.E.V);
            //        request.Move[0] = Convert.ToByte(64 - request.Move[0]);
            //        request.Move[1] = Convert.ToByte(64 - request.Move[1]);
            //        _queryManager.SendPlayerMove(opponentInfo, request.Move, BR.E.V);
            //        ok = false;
            //    }
            //    else
            //    {
            //        _queryManager.SendPlayerMove(opponentInfo, request.Move, BR.E.V);
            //        request.Move[0] = Convert.ToByte(64 - request.Move[0]);
            //        request.Move[1] = Convert.ToByte(64 - request.Move[1]);
            //        _queryManager.SendPlayerMove(plInfo, request.Move, BR.E.V);
            //        ok = false;
            //    }
            //}



            //if (ok)
            //{
            //    if (t.Length == 3)
            //    {
            //        _queryManager.SendPlayerMove(plInfo, t, BR.E.GetDateArm((byte)msg.Move[0], (byte)msg.Move[1]));
            //        QueryManager.SendPlayerMove(t, opponentContext, Cache.Instance.BR1.E.GetDateArm((byte)msg.Move[0], (byte)msg.Move[1]));
            //    }
            //    else { int c = 1 + 1; } QueryManager.SendPlayerMove(t, playerContext, Cache.Instance.BR1.E.GetDateArm((byte)msg.Move[0], (byte)msg.Move[1]));
            //}
        }
    }
}
