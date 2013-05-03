using System.Linq;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class GetGameStateExecutor : IExecutor
    {
        private AliveChessLogger _logger;

        public GetGameStateExecutor(AliveChessLogger logger)
        {
            this._logger = logger;
        }

        public void Execute(Message msg)
        {
            GetGameStateRequest request = (GetGameStateRequest) msg.Command;

            //_logger.Log(
            //    msg.Sender.Login,
            //    "GameData",
            //    msg.Sender.King.Id.ToString(),
            //    msg.Sender.Connection.ToString(),
            //    msg.Sender.King.X + ":" + msg.Sender.King.Y,
            //    msg.Sender.King.StartCastle.X + ":" + msg.Sender.King.StartCastle.Y);

            GetGameStateResponse response =
                new GetGameStateResponse(
                    msg.Sender.King, msg.Sender.King.StartCastle,
                    msg.Sender.King.ResourceStore.Resources.ToList());

            msg.Sender.Messenger.SendNetworkMessage(response);
        }
    }
}
