using AliveChessPluginLibrary;
using AliveChessServer.Chat;
using AliveChessServer.LogicLayer;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer
{
    public class AliveChessServerContext : IAliveChessServerContext
    {
        private GameLogic _logic;
        private ServerApplication _network;

        private readonly CommandPool _commands;
        private readonly GameWorld _environment;
        private readonly PlayerManager _playerManager;
        private readonly TimeManager _timeManager;
       
        private IChatConnector _chatConnector;

        public AliveChessServerContext(MainForm view, PluginPool pluginPool, AliveChessLogger logger)
        {
            this._commands = new CommandPool();
            this._timeManager = new TimeManager();
            this._environment = new GameWorld(view.ConnectionString, _timeManager, logger);
            this._playerManager = new PlayerManager(_environment, _timeManager, logger);
            this._network = new ServerApplication(view, _playerManager, _commands, logger, pluginPool);
            this._logic = new GameLogic(_environment, _timeManager, _commands, Network.Transport, logger, _playerManager);

            this._playerManager.Transport = _network.Transport;

            this._chatConnector = new ChatConnector(_playerManager, _environment, _network.Transport);
        }

        public GameLogic Logic
        {
            get { return _logic; }
            set { _logic = value; }
        }

        public ServerApplication Network
        {
            get { return _network; }
            set { _network = value; }
        }

        public IChatConnector Chat
        {
            get { return _chatConnector; }
            set { _chatConnector = value; }
        }
    }
}
