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

        private readonly Player _player;
        private readonly Assets.GameLogic.Network.Network _network;
        private readonly World _world;
        private BigMapCommandController _bigMapCommandController;

        public event EventHandler Authorized;

        public BigMapCommandController BigMapCommandController
        {
            get { return _bigMapCommandController; }
        }

        private GameCore()
        {
            _bigMapCommandController = new BigMapCommandController();
            _network = new Assets.GameLogic.Network.Network();
            _world = new World();
            _player = new Player();
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

        public Player Player
        {
            get { return _player; }
        }

        public World World
        {
            get { return _world; }
        }

        public void ConnectToServer()
        {
            _network.Connect(IPAddress.Parse("127.0.0.1"));
        }

        public void OnAuthorize()
        {
            if (Authorized != null)
                Authorized(this, new EventArgs());
        }
    }
}
