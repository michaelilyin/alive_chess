using System;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Aliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.EmpireExecutors
{
    public class GetHelpResourceExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private LevelRoutine _levelManager;

        public GetHelpResourceExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            this._levelManager = gameLogic.Environment.LevelManager;
        }

        public void Execute(Message msg)
        {
            GetHelpResourceRequest request = (GetHelpResourceRequest)msg.Command;

            Player player = msg.Sender;
            Level level = _levelManager.GetLevelById(player.LevelId);
            IAliance aliance = level.EmpireManager.GetAlianceByMember(player.King);
            if (aliance != null)
            {
                if (aliance.Status == AlianceStatus.Union)
                    aliance.PublishNews(player, NewsType.HelpResource,
                      String.Format("Player {0} wants {1} items of {2}", player.King.Id,
                      request.ResourceCount, request.ResourceType.ToString()));
                else
                {
                    Empire e = aliance as Empire;
                    e.SendMessageToLeader(player, NewsType.HelpResource,
                    String.Format("Player {0} wants {1} items of {2}", player.King.Id,
                    request.ResourceCount, request.ResourceType.ToString()));
                }
            }
        }
    }
}
