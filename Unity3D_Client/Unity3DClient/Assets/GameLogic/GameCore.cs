using Assets.GameLogic.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Assets.GameLogic
{
    public class GameCore
    {
        private static GameCore _instance;

        //private readonly Player _player;
        private readonly Assets.GameLogic.Network.Network _network;
        //private readonly WindowContext _windowContext;
        //private readonly GameWorld _world;
        //private BigMapCommandController _bigMapCommandController;
        //private CastleCommandController _castleCommandController;

        public event EventHandler Authorized;

        //public BigMapCommandController BigMapCommandController
        //{
        //    get { return _bigMapCommandController; }
        //}

        //public CastleCommandController CastleCommandController
        //{
        //    get { return _castleCommandController; }
        //}

        private GameCore()
        {
            //_windowContext = new WindowContext();
            //_bigMapCommandController = new BigMapCommandController(this);
            //_castleCommandController = new CastleCommandController(this);
            _network = new Assets.GameLogic.Network.Network();
            ////_executor = new RequestExecutor(_logger, _commands);
            //_network.OnConnect += new NetworkManager.ConnectHandler(OnConnect);
            //_world = new GameWorld();
            //_player = new Player();
        }

        public static GameCore Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new GameCore();
                }
                return _instance; 
            }
        }

        public Assets.GameLogic.Network.Network Network
        {
            get { return _network; }
        }

        //public RequestExecutor Executor
        //{
        //    get { return _executor; }
        //}

        //public WindowContext WindowContext
        //{
        //    get { return _windowContext; }
        //}

        //public Player Player
        //{
        //    get { return _player; }
        //}

        //public GameWorld World
        //{
        //    get { return _world; }
        //}

        public void ConnectToServer()
        {
            _network.Connect(IPAddress.Parse("127.0.0.1"));
        }

        public void OnAuthorize()
        {
            if (Authorized != null)
                Authorized(this, new EventArgs());
        }

        private void OnConnect()
        {
           // _executor.Start();
        }
    }
}
