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

        private int _experience;
        private int _militaryRank;

        private int _mineNumber;
        private int _castleNumber;
        private int _resourceNumber;

        private int _aliancePowerRate;

        private int _winNumber;
        private int _looseNumber;

        private int _commonBattlesNumber;

        private DateTime _startGameDate;
        private DateTime _lastGameDate;

        public DateTime StartGameDate
        {
            get { return _startGameDate; }
            set { _startGameDate = value; }
        }

        public DateTime LastGameDate
        {
            get { return _lastGameDate; }
            set { _lastGameDate = value; }
        }

        public int WinNumber
        {
            get { return _winNumber; }
            set { _winNumber = value; }
        }

        public int LooseNumber
        {
            get { return _looseNumber; }
            set { _looseNumber = value; }
        }

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

        public int MineNumber
        {
            get { return _mineNumber; }
            set { _mineNumber = value; }
        }

        public int CastleNumber
        {
            get { return _castleNumber; }
            set { _castleNumber = value; }
        }

        public int ResourceNumber
        {
            get { return _resourceNumber; }
            set { _resourceNumber = value; }
        }

        public int CommonBattlesNumber
        {
            get { return _commonBattlesNumber; }
            set { _commonBattlesNumber = value; }
        }
    }
}
