using System;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Alliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.EmpireExecutors
{
    public class ExcludeKingFromEmpireExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private LevelRoutine _levelManager;

        public ExcludeKingFromEmpireExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            this._levelManager = gameLogic.Environment.LevelManager;
        }

        public void Execute(Message msg)
        {
            ExcludeKingFromEmpireRequest request = (ExcludeKingFromEmpireRequest)msg.Command;
            Player player = msg.Sender;
            Level level = _levelManager.GetLevelById(player.LevelId);
            King candidate = level.Map.SearchKingById(request.KingId);
            IAlliance aliance = level.EmpireManager.GetAlianceByMember(candidate);
            if (aliance != null && aliance.Status == AllianceStatus.Empire && player.King.IsLeader)
            {
                // удаляем игрока из списка империи
                aliance.RemoveMember(candidate);
                // отправляем сообщение лидеру о том, что игрок был исключен из империи
                player.Messenger.SendNetworkMessage(new ExcludeKingFromEmpireResponse(true));
                // отправка игроку сообщения о исключении из империи
                if (!candidate.Player.Bot)
                    candidate.Player.Messenger.SendNetworkMessage(new ExcludeFromEmpireMessage());

                aliance.PublishNews(player, NewsType.PlayerExcludedFromEmpire,
                    String.Format("Player {0} excluded", candidate.Name));
            }
        }
    }
}
