using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Utility;
using AliveChessServer.DBLayer;
using ModuleBatleAliveChess;

namespace AliveChessServer.LogicLayer.Environment
{
    public class BattleRoutine : IRoutine
    {
        private LevelRoutine lManager;
        private GlobalC War;
        byte[,] myB;
        int[,] Arm;
        Engine e;
        private Dictionary<int, Battle> _battles;
        private TimeManager _timeManager;

        public BattleRoutine(TimeManager timeManager)
        {
            War = new GlobalC();
            myB = new byte[War.Sides, 64];
            Arm = new int[War.Sides, 64];
            e = new Engine(null, myB, Arm);
            _battles = new Dictionary<int, Battle>();
            _timeManager = timeManager;
        }

        public void Update()
        {
            if (e.MyForce) return;

            //if (!myPondering && !myInMove) return;
            if (!e.MyInMove) return;

            if (e.MyThinking) return;

            e.MyThink = true;
            //e.MyThinkStart = _timeManager.BigMapTime.Now.Ticks;// DateTime.Now.Ticks;


        }

        public Battle CreateBattle(King first, King second, bool step)
        {
            War = new GlobalC();
            myB = new byte[War.Sides, 64];
            Arm = new int[War.Sides, 64];
            e = new Engine(null, myB, Arm);

            Battle b = new Battle();
            GuidIDPair p = GuidGenerator.Instance.GeneratePair();
            b.Id = p.Id;
            b.Organizator = first;
            b.Respondent = second;
            return b;
        }

        public void Add(Battle battle)
        {
            _battles.Add(battle.Id, battle);
        }

        public void Remove(Battle battle)
        {
            _battles.Remove(battle.Id);
        }

        public Battle GetBattleById(int id)
        {
            return _battles.ContainsKey(id) ? _battles[id] : null;
        }

        public King GetOpponent(Battle dispute, King player)
        {
            return player.Id == dispute.Organizator.Id ? dispute.Respondent : dispute.Organizator;
        }

        public Engine E
        {
            get { return e; }
        }

        public GlobalC War1
        {
            get { return War; }
        }

        public BattleRoutine(LevelRoutine lManager)
        {
            this.lManager = lManager;
        }

        public byte[] UserMov(byte[] t)
        {

            return e.UserMove(t);
        }
    }
}
