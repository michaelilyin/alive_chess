using System;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Aliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.EmpireExecutors
{
    public class EmbedTaxRateExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private LevelRoutine _levelManager;

        public EmbedTaxRateExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            this._levelManager = gameLogic.Environment.LevelManager;
        }

        public void Execute(Message msg)
        {
            EmbedTaxRateRequest request = (EmbedTaxRateRequest)msg.Command;

            Player player = msg.Sender;
            Level level = _levelManager.GetLevelById(player.LevelId);
            IAliance aliance = level.EmpireManager.GetAlianceByMember(player.King);
            if (aliance != null && aliance.Status == AlianceStatus.Empire && player.King.IsLeader)
            {
                // Embed new tax rate
                Empire empire = aliance as Empire;
                empire.TaxRate = request.Rate;

                player.Messenger.SendNetworkMessage(new EmbedTaxRateResponse(true));
                aliance.PublishNews(player, NewsType.PlayerExcludedFromEmpire,
                   String.Format("Leader {0} change tax rate. New rate is {1}", player.King.Name, empire.TaxRate));
            }
        }
    }
}
