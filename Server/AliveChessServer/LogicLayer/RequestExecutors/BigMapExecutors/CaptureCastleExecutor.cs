using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class CaptureCastleExecutor : IExecutor
    {
        public void Execute(Message msg)
        {
            CaptureCastleRequest request = (CaptureCastleRequest)msg.Command;

            Player player = msg.Sender;
          
            Map map = player.Map;
            King king = player.King;
            Castle castle = map.SearchCastleById(request.CastleId);
            if (castle != null)
            {
                if (castle.Player != null)
                {
                    if (!castle.Player.Bot)
                        castle.Player.Messenger.SendNetworkMessage(new LooseCastleMessage(castle.Id));
                    castle.King.RemoveCastle(castle);
                }

                king.AddCastle(castle);
                player.Messenger.SendNetworkMessage(new CaptureCastleResponse(castle));
            }
            else
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Castle not found"));
            }
        }
    }
}
