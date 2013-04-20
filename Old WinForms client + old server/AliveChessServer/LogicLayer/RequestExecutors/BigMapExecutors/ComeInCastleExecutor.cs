using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class ComeInCastleExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public ComeInCastleExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            ComeInCastleRequest request = (ComeInCastleRequest)msg.Command;

            Player player = msg.Sender;
            Castle castle = player.Map.SearchCastleByPointId(request.CastleId);

            if (castle.Player != null && castle.Player.Id == player.Id)
            {
                int castleId = castle.Id;
                player.King.ComeInCastle(castle);
                player.Messenger.SendNetworkMessage(new ComeInCastleResponse(castleId));
            }
        }
    }
}
