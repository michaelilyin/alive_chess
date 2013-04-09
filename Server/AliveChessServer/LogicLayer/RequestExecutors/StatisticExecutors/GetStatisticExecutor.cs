using AliveChessLibrary.Commands.StatisticCommand;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.StatisticExecutors
{
    public class GetStatisticExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public GetStatisticExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            GetStatisticRequest request = (GetStatisticRequest) msg.Command;
        }
    }
}
