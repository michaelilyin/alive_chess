using ProtoBuf;

namespace AliveChessLibrary.Commands.BattleCommand
{
    [ProtoContract]
    public class MoveUnitRequest : ICommand
    {
        public Command Id
        {
            get { return Command.MoveUnitRequest; }
        }

        [ProtoMember(3)]
        public int BattleId { get; set; }

        [ProtoMember(2)]
        public int OpponentId { get; set; }

        [ProtoMember(1)]
        public byte Position { get; set; }
    }
}

