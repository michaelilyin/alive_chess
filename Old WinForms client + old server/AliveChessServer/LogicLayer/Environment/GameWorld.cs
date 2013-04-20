using System;
using System.Threading;
using AliveChessLibrary.GameObjects;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessServer.DBLayer.Loaders;

namespace AliveChessServer.LogicLayer.Environment
{
    public class GameWorld
    {
        //private Timer _envTimer;
        private Thread _envThread;

        private GameTime _time;
        private TimeManager _timeManager;

        private LevelRoutine _levelRoutine;
        private BigMapRoutine _bigMapRoutine;
        private BattleRoutine _battleRoutine;
        private EconomyRoutine _economyRoutine;
        private DisputeRoutine _disputeRoutine;

        private XMLMapLoader _xmlLoader;
        private DataBaseMapManager _dbLoader;

        private GameData _context;
        private VisibleSpaceManager _vsManager;

        public GameWorld(GameData context)
        {
            this._context = context;
            this._timeManager = new TimeManager();

            //this._battleRoutine = new BattleRoutine(context);
            this._economyRoutine = new EconomyRoutine(context, this);
            //this._disputeRoutine = new DisputeRoutine(context, this);
            //this._bigMapRoutine = new BigMapRoutine(context, this);

            this._vsManager = new VisibleSpaceManager(context);

            this._xmlLoader = new XMLMapLoader(this, context);
            this._dbLoader = new DataBaseMapManager(this);
            this._levelRoutine = new LevelRoutine(this, _xmlLoader, _dbLoader);

            this._envThread = new Thread(new ThreadStart(Update));
            this._envThread.IsBackground = true;
            //this._envThread.SetApartmentState(ApartmentState.STA);
        }

        public void StartGame()
        {
            // инициализируем уровни
            this._levelRoutine.Initialize();
            // запускаем процесс обработки команд
            //this._envTimer = new Timer(new TimerCallback(Update), null, 0, 10);
            this._envThread.Start();
        }

        public void StopGame()
        {
            try
            {
                _envThread.Abort();
            }
            catch(ThreadAbortException ex)
            {
                Console.WriteLine(ex.Message);
            }
            //_envTimer.Change(0, Timeout.Infinite);
        }

        private void Update()
        {
            while (true)
            {
                _time = _timeManager.Time;
                if (_timeManager.IsUpdated)
                {
                    _levelRoutine.DoLogic(_time);
                    //_bigMapRoutine.DoLogic(_time);
                    //_battleRoutine.DoLogic(_time);
                    //_disputeRoutine.DoLogic(_time);
                    //_economyRoutine.DoLogic(_time);
                }

                Thread.Sleep(10);
            }
        }

        public LevelRoutine LevelManager
        {
            get { return _levelRoutine; }
            set { _levelRoutine = value; }
        }

        //public BigMapRoutine BigMapRoutine
        //{
        //    get { return _bigMapRoutine; }
        //    set { _bigMapRoutine = value; }
        //}

        public EconomyRoutine EconomyRoutine
        {
            get { return _economyRoutine; }
            set { _economyRoutine = value; }
        }

        //public BattleRoutine BattleRoutine
        //{
        //    get { return _battleRoutine; }
        //    set { _battleRoutine = value; }
        //}

        //public DisputeRoutine DisputeRoutine
        //{
        //    get { return _disputeRoutine; }
        //    set { _disputeRoutine = value; }
        //}

        public GameData Context
        {
            get { return _context; }
            set { _context = value; }
        }

        public DataBaseMapManager DbLoader
        {
            get { return _dbLoader; }
            set { _dbLoader = value; }
        }

        public XMLMapLoader XmlLoader
        {
            get { return _xmlLoader; }
            set { _xmlLoader = value; }
        }

        public VisibleSpaceManager VisibleSpaceManager
        {
            get { return _vsManager; }
        }
    }
}
