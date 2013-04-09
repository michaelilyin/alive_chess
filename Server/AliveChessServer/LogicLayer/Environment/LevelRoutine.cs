using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using AliveChessServer.DBLayer.Loaders;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.LogicLayer.Environment
{
    public class LevelRoutine : IRoutine
    {
        private GameWorld _environment;
        private ILevelLoader _levelLoader;
        private PlayerManager _playerManager;
        private TimeManager _timeManager;
        private AliveChessLogger _logger;
       
        private EntitySet<Level> _levels;

        private object _levelSync = new object();

        public LevelRoutine(GameWorld environment, ILevelLoader levelLoader, 
            TimeManager timeManager, AliveChessLogger logger)
        {
            this._logger = logger;
            this._environment = environment;
            this._levelLoader = levelLoader;
            this._timeManager = timeManager;
            this._levels = new EntitySet<Level>();
        }

        public void Load()
        {
            Level level = _levelLoader.LoadLevel(LevelTypes.Easy);
            level.Initialize(_timeManager, _playerManager, _logger);

            lock (_levelSync)
                _levels.Add(level);
        }

        public void Update()
        {
            lock (_levelSync)
                foreach (Level level in _levels)
                    level.Update();
        }

        public Level GetLevelById(int id)
        {
            lock (_levelSync)
                return _levels.Single(x => x.Id == id);
        }

        public Level GetLevelByType(LevelTypes type)
        {
            lock (_levelSync)
                return _levels.FirstOrDefault(x => x.LevelType == type);
        }

        public IEnumerable<Level> NextLevel()
        {
            lock (_levelSync)
                for (int i = 0; i < _levels.Count; i++)
                    yield return _levels[i];
        }

        public void Load(IEnumerable<Level> levels)
        {
            lock (_levelSync)
                _levels.Assign(levels);
        }

        public PlayerManager PlayerManager
        {
            get { return _playerManager; }
            set { _playerManager = value; }
        }

        public Level Find(Func<Level,bool> predicate)
        {
            lock (_levelSync)
                return _levels.FirstOrDefault(predicate);
        }
    }
}
