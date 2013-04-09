using AliveChessLibrary;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.Commands.RegisterCommand;
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

        public AuthorizeExecutor(GameLogic gameLogic, ProtoBufferTransport transport)
        {
            this._transport = transport;
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            // получаем от игрока запрос
            AuthorizeRequest request = (AuthorizeRequest) msg.Command;

            // логин и пароль
            string name = request.Login;
            string password = request.Password;

            var conn = _transport.SocketTransport.SearchConnection(msg.RemoteEndPoint);

            if (conn != null)
            {
                try
                {
                    _playerManager.Authorize(new Identity(name, password), conn);
                }
                catch (AliveChessException)
                {
                    _transport.Send(conn.Socket, new ErrorMessage("You haven't registered yet"));
                }
            }
        }
    }
}
