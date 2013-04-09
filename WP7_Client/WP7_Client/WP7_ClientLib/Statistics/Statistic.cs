using System;
using ProtoBuf;

namespace AliveChessLibrary.Statistics
{
    [ProtoContract]
    public class Statistic
    {
        [ProtoMember(1)]
        private int _pawnNumber;
        [ProtoMember(2)]
        private int _bishopNumber;
        [ProtoMember(3)]
        private int _rookNumber;
        [ProtoMember(4)]
        private int _queenNumber;
        [ProtoMember(5)]
        private int _knightNumber;

        public DateTime StartGameDate { get; set; }

        public DateTime LastGameDate { get; set; }

        public int WinNumber { get; set; }

        public int LooseNumber { get; set; }

        public int PawnNumber
        {
            get { return _pawnNumber; }
            set { _pawnNumber = value; }
        }

        public int BishopNumber
        {
            get { return _bishopNumber; }
            set { _bishopNumber = value; }
        }

        public int RookNumber
        {
            get { return _rookNumber; }
            set { _rookNumber = value; }
        }

        public int QueenNumber
        {
            get { return _queenNumber; }
            set { _queenNumber = value; }
        }

        public int KnightNumber
        {
            get { return _knightNumber; }
            set { _knightNumber = value; }
        }

        public int MineNumber { get; set; }

        public int CastleNumber { get; set; }

        public int ResourceNumber { get; set; }

        public int CommonBattlesNumber { get; set; }
    }
}
