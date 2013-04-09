using System;
using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessLibrary.Interaction;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Alliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.EmpireExecutors
{
    public class GetHelpFigureExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private LevelRoutine _levelManager;

        public GetHelpFigureExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            this._levelManager = gameLogic.Environment.LevelManager;
        }

        public void Execute(Message msg)
        {
            GetHelpFigureRequest request = (GetHelpFigureRequest) msg.Command;

            Player player = msg.Sender;
            Level level = _levelManager.GetLevelById(player.LevelId);
            IAlliance aliance = level.EmpireManager.GetAlianceByMember(player.King);
            if (aliance != null)
            {
                if (aliance.Status == AllianceStatus.Union)
                    aliance.PublishNews(player, NewsType.HelpFigure,
                    String.Format("Player {0} wants {1} units of {2}", player.King.Id,
                    request.FigureCount, request.FigureType));
                else
                {
                    Empire e = aliance as Empire;
                    e.SendMessageToLeader(player, NewsType.HelpFigure,
                    String.Format("Player {0} wants {1} items of {2}", player.King.Id,
                    request.FigureCount, request.FigureCount.ToString()));
                }
            }
        }
    }
}
