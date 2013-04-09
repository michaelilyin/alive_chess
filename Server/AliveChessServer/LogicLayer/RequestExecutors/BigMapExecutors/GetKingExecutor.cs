using System.Linq;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class GetKingExecutor : IExecutor
    {
        public void Execute(Message msg)
        {
            GetKingRequest request = (GetKingRequest) msg.Command;
            Map map = msg.Sender.Map;
            King king = map.Kings.FirstOrDefault(x => x.Id == request.KingId);
            GetKingResponse response = new GetKingResponse();
            response.King = king;
            msg.Sender.Messenger.SendNetworkMessage(response);
        }
    }
}
