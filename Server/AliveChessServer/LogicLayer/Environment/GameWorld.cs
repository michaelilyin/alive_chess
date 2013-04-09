using System.Collections.Generic;
using AliveChessServer.DBLayer.Loaders;

namespace AliveChessServer.LogicLayer.Environment
{
    public class GameWorld
    {
        private TimeManager _timeManager;

        private LevelRoutine _levelRoutine;

        private ILevelLoader _levelLoader;
        private List<FastBattle> _fastBattles;

        public GameWorld(string connectionString, TimeManager timeManager, AliveChessLogger logger)
        {
            this._timeManager = timeManager;
            this._fastBattles = new List<FastBattle>();
            this._levelLoader = new XMLLevelLoader(this);
            //this._levelLoader = new SQLLevelLoader(connectionString, logger);
            this._levelRoutine = new LevelRoutine(this, _levelLoader, _timeManager, logger);
            this._levelLoader.LevelRoutine = _levelRoutine;
        }

        public void Initialize()
        {
            this._levelRoutine.Load();
        }

        public void Update()
        {
            UpdateFastBattles();
            this._levelRoutine.Update();
        }

        public LevelRoutine LevelManager
        {
            get { return _levelRoutine; }
            set { _levelRoutine = value; }
        }

        public ILevelLoader LevelLoader
        {
            get { return _levelLoader; }
            set { _levelLoader = value; }
        }

        public void AddFastBattle(FastBattle battle)
        {

        }

        public void RemoveFastBattle(FastBattle battle)
        {

        }

        private void UpdateFastBattles()
        {
            for (int i = 0; i < _fastBattles.Count; i++)
                _fastBattles[i].Update();
        }
    }
}
