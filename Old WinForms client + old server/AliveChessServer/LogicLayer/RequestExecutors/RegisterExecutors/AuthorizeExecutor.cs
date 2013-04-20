using System.Linq;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.Net;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.RegisterExecutors
{
    public class AuthorizeExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private ProtoBufferTransport _transport;

        public AuthorizeExecutor(LogicEntryPoint gameLogic, ProtoBufferTransport transport)
        {
            this._transport = transport;
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            // получаем от игрока запрос
            AuthorizeRequest request = (AuthorizeRequest)msg.Command;

            // логин и пароль
            string name = request.Login;
            string password = request.Password;

            ConnectionInfo conn = _transport.SocketTransport.SearchConnection(msg.RemoteEndPoint);

            if (conn != null)
            {
                Level level = _environment.LevelManager.EasyLevel;
                Player player = _playerManager.AuthorizeInGame(new Identity(name, password), level);

                if (player != null)
                {
                    conn.Player = player;
                    player.Level = level;
                    player.Connection = conn;
                    player.IsSuperUser = request.IsSuperUser;
                    player.Messenger = new Messenger(_transport, conn);

                    _playerManager.LogInPlayer(player);
                    player.Messenger.SendNetworkMessage(
                        new AuthorizeResponse(player.King, player.King.StartCastle,
                        player.IsSuperUser, player.King.StartCastle.ResourceStore.Resources.ToList()));
                }
                else
                {
                    _transport.Disconnect(conn);
                }
            }

        }
    }
}
