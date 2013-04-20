using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BattleExecutors
{
    public class DownloadBattlefildExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private BattleRoutine BR;
        /// <summary>
        /// диспут необходим для получения контекста оппонента
        /// </summary>
        private Dialog _dispute;

        public DownloadBattlefildExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            //this.BR = gameLogic.Environment.BattleRoutine;
        }
        public void Execute(Message msg)
        {
            //DownloadBattlefieldRequest request = (DownloadBattlefieldRequest)msg.Command;
            //King king1 = msg.Sender.King;
            //_dispute = _environment.DisputeRoutine.GetDisputeById(request.Opponent);
            //King king2 = _environment.DisputeRoutine.GetOpponent(_dispute, msg.Sender.King);
            //PlayerInfo plInfo = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            //PlayerInfo opponentInfo = _playerManager.GetPlayerInfoById(request.Opponent);
            //BR.War1.DownloadArmy(king2.Units, king1.Units);
            //_queryManager.SendDownloadBattlefild(plInfo, king1.Units, king2.Units);
            //_queryManager.SendDownloadBattlefild(opponentInfo, king2.Units, king1.Units);
        }
    }
}
