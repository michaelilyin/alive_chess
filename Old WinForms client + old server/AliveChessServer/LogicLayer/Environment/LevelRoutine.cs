using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using AliveChessLibrary.GameObjects;
using AliveChessServer.DBLayer.Loaders;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.LogicLayer.Environment
{
    public class LevelRoutine : IRoutine
    {
        private GameWorld _environment;
        private XMLMapLoader _xmlLoader;
        private DataBaseMapManager _dbLoader;
        private PlayerManager _playerManager;

        private EntitySet<Level> _levels;

        private object _levelSync = new object();

        public LevelRoutine(GameWorld environment, XMLMapLoader xmlLoader,
            DataBaseMapManager dbLoader)
        {
            this._environment = environment;
            this._xmlLoader = xmlLoader;
            this._dbLoader = dbLoader;
            this._levels = new EntitySet<Level>();
        }

        public void Initialize()
        {
            //_dbLoader.Load();

            Level level = new Level(_xmlLoader.Load(), LevelTypes.Easy);
            lock (_levelSync)
                _levels.Add(level);
            //_environment.BigMapRoutine.AddMap(level.Map);

           // Debug.Assert(_environment.BigMapRoutine.PlayerManager != null);
            level.PlayerManager = this._playerManager;
        }

        public void DoLogic(GameTime time)
        {
            lock (_levelSync)
                foreach (Level level in _levels)
                    level.DoLogic(time);
        }

        public Level GetLevelById(int guid)
        {
            return _levels.Single(x => x.Id == guid);
        }

        public Level EasyLevel
        {
            get { return _levels.Single(x => x.LevelType == LevelTypes.Easy); }
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

        public GameData GameData
        {
            get { return _xmlLoader.Context; }
        }

        public PlayerManager PlayerManager
        {
            get { return _playerManager; }
            set { _playerManager = value; }
        }
    }
}
