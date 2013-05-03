using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class ComeInCastleExecutor : IExecutor
    {
        public void Execute(Message msg)
        {
            ComeInCastleRequest request = (ComeInCastleRequest)msg.Command;

            Player player = msg.Sender;
            Castle castle = player.Map.SearchCastleById(request.CastleId);
            if (castle != null)
            {
                if (castle.Player != null && castle.BelongsTo(player.King))
                {
                    int castleId = castle.Id;
                    player.King.ComeInCastle(castle);
                    player.Messenger.SendNetworkMessage(new ComeInCastleResponse(castleId));
                }
            }
            else
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Castle not found"));
            }
        }
    }
}
