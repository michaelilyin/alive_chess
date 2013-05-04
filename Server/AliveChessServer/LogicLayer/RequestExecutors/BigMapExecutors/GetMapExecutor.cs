using System.Linq;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class GetMapExecutor : IExecutor
    {
        public void Execute(Message msg)
        {
            GetMapRequest request = (GetMapRequest) msg.Command;

            Player player = msg.Sender;
            Map map = player.Level.Map;
            var mines = map.Mines.ToList();
            var castles = map.Castles.ToList();
            var points = map.BasePoints.ToList();
            var single = map.SingleObjects.ToList();
            var multy = map.MultyObjects.ToList();
            var borders = map.Borders.ToList();

            player.Messenger.SendNetworkMessage(
                new GetMapResponse(map.Id, map.SizeX, map.SizeY,
                                   castles, mines, points, 
                                   borders, single, multy));

            player.Ready = true;
        }
    }
}
