using AliveChessLibrary.Interaction;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BattleCommand
{
    [ProtoContract]
    public class DownloadBattlefildResponse : ICommand
    {
        //[ProtoMember(1)]
        //private IList<Unit> arm = new List<Unit>();
        //[ProtoMember(2)]
        //private IList<Unit> oppArm = new List<Unit>();
        //[ProtoMember(3)]
        //private bool course;

        //[ProtoMember(4)]
        //private uint idBattle;
        //[ProtoMember(5)]
        //private uint idOpponent;

        [ProtoMember(1)]
        private Battle _battle;

        public Command Id
        {
            get { return Command.DownloadBattlefildResponse; }
        }

        public Battle Battle
        {
            get { return _battle; }
            set { _battle = value; }
        }

        //public uint IdOpponent
        //{
        //    get { return idOpponent; }
        //    set { idOpponent = value; }
        //}

        //public uint IdBattle
        //{
        //    get { return idBattle; }
        //    set { idBattle = value; }
        //}

        //public bool Course
        //{
        //    get { return course; }
        //    set { course = value; }
        //}

        //public IList<Unit> OppArm
        //{
        //    get { return oppArm; }
        //    set { oppArm = value; }
        //}

        //public IList<Unit> Arm
        //{
        //    get { return arm; }
        //    set { arm = value; }
        //}
    }
}
