using System.Diagnostics;
using System.Threading;
using AliveChessLibrary;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer
{
    public class GameLogic
    {
        private Thread _executeThread;
        private Thread _globalEnvironmentThread;
        private Thread _localEnvironmentThread;

        private TimeManager _timeManager;

        private MainExecutor _executor;
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public GameLogic(GameWorld environment, TimeManager timeManager, CommandPool commands,
            ProtoBufferTransport transport, AliveChessLogger logger, PlayerManager playerManager)
        {
#if DEBUG
            DebugConsole.WriteLine(this, "Created");
#endif
            Debug.Assert(environment != null);
            Debug.Assert(commands != null);
            Debug.Assert(transport != null);
            Debug.Assert(logger != null);
            Debug.Assert(playerManager != null);

            this._timeManager = timeManager;
            this._environment = environment;
            this._playerManager = playerManager;
            this._executor = new MainExecutor(commands, transport, logger, this);
            this._environment.LevelManager.PlayerManager = playerManager;

            _executeThread = new Thread(_executor.Execute);
            _executeThread.IsBackground = true;

            this._globalEnvironmentThread = new Thread(UpdateGame);
            this._globalEnvironmentThread.IsBackground = true;

            //this._localEnvironmentThread = new Thread(new ThreadStart(UpdateLocal));
            //this._localEnvironmentThread.IsBackground = true;
        }

        // запуск игрового процесса
        public void StartGame()
        {
            StartGlobalGame();
        }

        // игровой цикл (_globalEnvironmentThread)
        private void UpdateGame()
        {
            while (true)
            {
                // обновляем игровое время
                _timeManager.Update();

                // обновляем состояния игроков
                _playerManager.Update();

                // если появился новый игрок то
                // добавляем его в игру
                if (_playerManager.HasNewPlayers)
                {
                    _playerManager.HandleNewPlayer();
                }
                else
                {
                    // обновляем игровой мир
                    _environment.Update();
                }
            }
        }

        // запуск обработки команд
        public void StartExecuting()
        {
            _executeThread.Start();
        }

        // запуск глобальных карт
        private void StartGlobalGame()
        {
            this._globalEnvironmentThread.Start();
        }

        // запуск локальных карт
        private void StartLocalGame()
        {
            this._localEnvironmentThread.Start();
        }

        public GameWorld Environment
        {
            get { return _environment; }
        }

        public PlayerManager PlayerManager
        {
            get { return _playerManager; }
        }
    }
}
