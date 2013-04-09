using ProtoBuf;

namespace AliveChessLibrary.Commands.BattleCommand
{
    [ProtoContract]
    public class MoveUnitResponse : ICommand
    {
        [ProtoMember(1)]
        bool _succeess;

        public Command Id
        {
            get { return Command.MoveUnitResponse; }
        }

        public bool Succeess
        {
            get { return _succeess; }
            set { _succeess = value; }
        }
    }
}
