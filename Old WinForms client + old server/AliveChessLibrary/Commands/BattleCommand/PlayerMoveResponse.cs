using ProtoBuf;

namespace AliveChessLibrary.Commands.BattleCommand
{
    [ProtoContract]
    public class PlayerMoveResponse : ICommand
    {
        [ProtoMember(1)]
        byte[] move = new byte[3];
        [ProtoMember(2)]
        int countunit;
        [ProtoMember(3)]
        bool step;

        public bool Step
        {
            get { return step; }
            set { step = value; }
        }

        public int Countunit
        {
            get { return countunit; }
            set { countunit = value; }
        }

        public byte[] Move
        {
            get { return move; }
            set { move = value; }
        }

        public Command Id
        {
            get { return Command.PlayerMoveResponse; }

        }
    }
}
