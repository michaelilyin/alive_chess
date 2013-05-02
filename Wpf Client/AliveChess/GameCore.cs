using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using AliveChess.GameLayer.LogicLayer;
using AliveChess.GameLayer.PresentationLayer;
using AliveChess.NetworkLayer;

namespace AliveChess
{
    public class GameCore
    {
        private static GameCore _instance;

        private readonly Player _player;
        private readonly NetworkManager _network;
        private readonly Logger _logger;
        private readonly RequestExecutor _executor;
        private readonly CommandPool _commands;
        private readonly WindowContext _windowContext;
        private readonly GameWorld _world;
        private BigMapRequestSender _bigMapRequestSender;

        public BigMapRequestSender BigMapRequestSender
        {
            get { return _bigMapRequestSender; }
            //set { _bigMapRequestSender = value; }
        }

        private GameCore()
        {
            _logger = new Logger();
            _commands = new CommandPool();
            _windowContext = new WindowContext();
            _bigMapRequestSender = new BigMapRequestSender();
            _network = new NetworkManager(_logger, _commands);
            _executor = new RequestExecutor(_logger, _commands);
            _network.OnConnect += new NetworkManager.ConnectHandler(OnConnect);
            _world = new GameWorld();
            _player = new Player();
        }

        public static GameCore Instance
        {
            get { return _instance ?? (_instance = new GameCore()); }
        }

        public NetworkManager Network
        {
            get { return _network; }
        }

        public RequestExecutor Executor
        {
            get { return _executor; }
        }

        public WindowContext WindowContext
        {
            get { return _windowContext; }
        }

        public Player Player
        {
            get { return _player; }
        }

        public GameWorld World
        {
            get { return _world; }
        }

        public void ConnectToServer()
        {
            _network.Connect(IPAddress.Parse("127.0.0.1"));
        }

        private void OnConnect()
        {
            _executor.Start();
        }
    }
}
