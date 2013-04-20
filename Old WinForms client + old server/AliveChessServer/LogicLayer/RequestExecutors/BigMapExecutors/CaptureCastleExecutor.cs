using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class CaptureCastleExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public CaptureCastleExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        #region IExecutor Members

        public void Execute(Message msg)
        {
            CaptureCastleRequest request = (CaptureCastleRequest)msg.Command;

            Player player = msg.Sender;
          
            Map map = player.Map;
            King king = player.King;
            Castle castle = map.SearchCastleById(request.CastleId);

            if (castle.Player != null)
            {
                if(!castle.Player.Bot)
                    castle.Player.Messenger.SendNetworkMessage(new LooseCastleMessage(castle.Id));
                castle.King.RemoveCastle(castle);
            }

            king.AddCastle(castle);
            castle.King.Player = king.Player;

            player.Messenger.SendNetworkMessage(new CaptureCastleResponse(castle));
        }

        #endregion
    }
}
