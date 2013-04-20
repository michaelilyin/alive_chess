using System.Diagnostics;
using System.Threading;
using AliveChessLibrary.GameObjects;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer
{
    public class LogicEntryPoint
    {
        private Thread _executeThread;

        private MainExecutor _executor;
        private GameData _context;
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public LogicEntryPoint(GameWorld environment, CommandPool commands,
            ProtoBufferTransport transport, AliveChessLogger logger, PlayerManager playerManager)
        {
            Debug.Assert(environment != null);
            Debug.Assert(commands != null);
            Debug.Assert(transport != null);
            Debug.Assert(logger != null);
            Debug.Assert(playerManager != null);

            this._environment = environment;
            this._context = environment.Context;
            this._playerManager = playerManager;
            this._executor = new MainExecutor(commands, transport, logger, this);
            this._environment.LevelManager.PlayerManager = playerManager;
            this._environment.EconomyRoutine.PlayerManager = playerManager;
            //this._environment.DisputeRoutine.PlayerManager = playerManager;

            _executeThread = new Thread(new ThreadStart(_executor.Execute));
            _executeThread.IsBackground = true;

            //AlianceRoutine a = new AlianceRoutine();
            //Empire e = new Empire(new Union());
            //e.CurrentTimeWithoutLeader = Empire.TimeWithoutLeader;
            //a.Add(e);
            //a.DoLogic(new GameTime());
            //a.DoLogic(new GameTime());
        }

        public void StartGame()
        {
            // запуск обработки команд
            _executeThread.Start();
            // запуск игрового процесса
            _environment.StartGame();
        }

        public void StopGame()
        {
            _environment.StopGame();
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
