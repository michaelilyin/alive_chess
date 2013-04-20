using System;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Aliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.EmpireExecutors
{
    public class ExitFromAlianceExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private LevelRoutine _levelManager;

        public ExitFromAlianceExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            this._levelManager = gameLogic.Environment.LevelManager;
        }

        public void Execute(Message msg)
        {
            ExitFromAlianceRequest request = (ExitFromAlianceRequest)msg.Command;

            Player player = msg.Sender;
            Level level = _levelManager.GetLevelById(player.LevelId);
            IAliance aliance = level.EmpireManager.GetAlianceByMember(player.King);
            aliance.RemoveMember(player.King);
            player.Messenger.SendNetworkMessage(new ExitFromAlianceResponse(true));
            aliance.PublishNews(player, NewsType.PlayerLeaveAliance,
               String.Format("Player {0} exited from aliance", player.King.Name));
        }
    }
}
