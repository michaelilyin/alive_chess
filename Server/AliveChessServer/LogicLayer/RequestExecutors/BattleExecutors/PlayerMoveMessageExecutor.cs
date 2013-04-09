using AliveChessLibrary.Commands.BattleCommand;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BattleExecutors
{
    public class PlayerMoveRequestExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
      
        public PlayerMoveRequestExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }
        public void Execute(Message msg)
        {
            MoveUnitRequest request = (MoveUnitRequest)msg.Command;

            byte r = request.Position;
            int lB = (r & (15 << 4)) >> 4;
            int rB = (r & 15);

            //

            MoveUnitResponse response = new MoveUnitResponse();
            response.Succeess = true;

            msg.Sender.Messenger.SendNetworkMessage(response);
        }
    }
}
