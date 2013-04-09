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
    public class IncludeKingInEmpireExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private LevelRoutine _levelManager;

        public IncludeKingInEmpireExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            this._levelManager = gameLogic.Environment.LevelManager;
        }

        public void Execute(Message msg)
        {
            IncludeKingInEmpireRequest request = (IncludeKingInEmpireRequest)msg.Command;
            Player player = msg.Sender; 
            Level level = _levelManager.GetLevelById(player.LevelId);
            King candidate = level.Map.SearchKingById(request.KingId);
            IAlliance aliance = level.EmpireManager.GetAlianceByMember(player.King);
            if (aliance != null && aliance.Status == AllianceStatus.Empire && player.King.IsLeader)
            {
                // добавляем игрока в список империи
                aliance.AddMember(candidate);
                // отправка лидеру сообщения о включении игрока в империю
                player.Messenger.SendNetworkMessage(new IncludeKingInEmpireResponse(true));
                // отправка игроку сообщения о присоединении к империи
                if(!candidate.Player.Bot)
                    candidate.Player.Messenger.SendNetworkMessage(new JoinToAlianceResponse(true));
                
                // Send news to all members
                aliance.PublishNews(player, NewsType.PlayerJoinedToAliance,
                    String.Format("Player {0} joins to us", candidate.Name));
            }
        }
    }
}
