using ProtoBuf;

namespace AliveChessLibrary.Commands.BattleCommand
{
    [ProtoContract]
    public class PlayerMoveRequest : ICommand
    {
        [ProtoMember(1)]
        byte[] move = new byte[2];
        [ProtoMember(2)]
        int opponentID;
        [ProtoMember(3)]
        private bool ok;
        [ProtoMember(4)]
        private int battleID;

        public int BattleID
        {
            get { return battleID; }
            set { battleID = value; }
        }

        public bool Ok
        {
            get { return ok; }
            set { ok = value; }
        }

        public int OpponentID
        {
            get { return opponentID; }
            set { opponentID = value; }
        }

        public byte[] Move
        {
            get { return move; }
            set { move = value; }
        }

        public Command Id
        {
            get { return Command.PlayerMoveRequest; }
        }
    }
}

