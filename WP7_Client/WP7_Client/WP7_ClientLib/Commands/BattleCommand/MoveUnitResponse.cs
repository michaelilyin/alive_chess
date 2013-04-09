using ProtoBuf;

namespace AliveChessLibrary.Commands.BattleCommand
{
    [ProtoContract]
    public class MoveUnitResponse : ICommand
    {
        public Command Id
        {
            get { return Command.MoveUnitResponse; }
        }

        [ProtoMember(1)]
        public bool Succeess { get; set; }
    }
}
