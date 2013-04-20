using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.RegisterExecutors
{
    public class ExitFromGameExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private ProtoBufferTransport _transport;

        public ExitFromGameExecutor(LogicEntryPoint gameLogic, ProtoBufferTransport transport)
        {
            this._transport = transport;
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            ExitFromGameRequest request = (ExitFromGameRequest)msg.Command;
            Player player = msg.Sender;
            _playerManager.LogOutPlayer(player);
            player.Messenger.SendNetworkMessage(new ExitFromGameResponse());

            _transport.Disconnect(player);
        }
    }
}
