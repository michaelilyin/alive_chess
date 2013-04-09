using AliveChessLibrary;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.Net;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.RegisterExecutors
{
    public class RegisterExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private ProtoBufferTransport _transport;
      
        public RegisterExecutor(GameLogic gameLogic, ProtoBufferTransport transport)
        {
            this._transport = transport;
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            RegisterRequest request = (RegisterRequest)msg.Command;
            string login = request.Login;
            string password = request.Password;
            ConnectionInfo conn = _transport.SocketTransport.SearchConnection(msg.RemoteEndPoint);
            if (!_playerManager.SearchPlayerInDataBase(login, password))
            {
                try
                {
                    _playerManager.Register(new Identity(login, password), conn);
                }
                catch (AliveChessException ex)
                {
                    RegisterResponse response = new RegisterResponse();
                    response.IsSuccessed = false;
                    _transport.Send(conn.Socket, response);
                }
            }
        }
    }
}
