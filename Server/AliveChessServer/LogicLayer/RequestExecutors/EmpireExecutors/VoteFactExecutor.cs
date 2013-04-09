using AliveChessLibrary.Commands.EmpireCommand;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.Environment.Alliances;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.EmpireExecutors
{
    public class VoteFactExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private LevelRoutine _levelManager;

        public VoteFactExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
            this._levelManager = gameLogic.Environment.LevelManager;
        }

        public void Execute(Message msg)
        {
            VoteBallotMessage resp = (VoteBallotMessage)msg.Command;
            Player player = msg.Sender;
            Level level = _levelManager.GetLevelById(player.LevelId);
            IAlliance aliance = level.EmpireManager.GetAlianceByMember(player.King);
            if (aliance.IsVoteInProgress)
            {
                Ballot b = new Ballot();
                b.From = player.King;
                b.Yes = resp.Support;
                aliance.BallotBox.AddBallot(b);
            }
        }
    }
}
