using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors
{
    public class LeaveCastleExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public LeaveCastleExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        #region IExecutor Members

        public void Execute(Message msg)
        {
            LeaveCastleRequest request = (LeaveCastleRequest)msg.Command;

            King king = msg.Sender.King;
        
            if (king != null && king.State == KingState.Castle)
            {
                king.LeaveCastle();
                king.Player.Messenger.SendNetworkMessage(new LeaveCastleResponse());
            }
        }

        #endregion
    }
}
