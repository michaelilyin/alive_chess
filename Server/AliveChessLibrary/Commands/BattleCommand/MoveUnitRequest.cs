using ProtoBuf;

namespace AliveChessLibrary.Commands.BattleCommand
{
    [ProtoContract]
    public class MoveUnitRequest : ICommand
    {
        [ProtoMember(1)]
        byte _position;
        [ProtoMember(2)]
        int _enemyId;
        [ProtoMember(3)]
        private int _battleId;

        public Command Id
        {
            get { return Command.MoveUnitRequest; }
        }

        public int BattleId
        {
            get { return _battleId; }
            set { _battleId = value; }
        }

        public int OpponentId
        {
            get { return _enemyId; }
            set { _enemyId = value; }
        }

        public byte Position
        {
            get { return _position; }
            set { _position = value; }
        }
    }
}

