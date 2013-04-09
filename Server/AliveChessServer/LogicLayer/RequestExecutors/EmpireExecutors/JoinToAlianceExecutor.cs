using System;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Alliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.EmpireExecutors
{
    public class JoinToAlianceExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private LevelRoutine _levelManager;

        public JoinToAlianceExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            this._levelManager = gameLogic.Environment.LevelManager;
        }

        public void Execute(Message msg)
        {
            JoinToAlianceRequest request = (JoinToAlianceRequest)msg.Command;

            Player player = msg.Sender;
            Level level = _levelManager.GetLevelById(player.LevelId);
            IAlliance aliance = level.EmpireManager.GetAlianceById(request.AlianceId);
            if (aliance != null)
            {
                if (aliance.Status == AllianceStatus.Union)
                {
                    aliance.AddMember(player.King);
                    player.Messenger.SendNetworkMessage(new JoinToAlianceResponse(true));
                    aliance.PublishNews(player, NewsType.PlayerJoinedToAliance,
                        String.Format("Player {0} joins to union", player.King.Name));
                }
                else if (aliance.Status == AllianceStatus.Empire)
                {
                    Leader leader = (aliance as Empire).Leader;
                    if (!leader.Player.Bot)
                    {
                        leader.Player.Messenger.SendNetworkMessage(new JoinRequestMessage(player.King));
                    }
                    else
                    {
                        // AI
                    }
                }
            }
        }
    }
}
