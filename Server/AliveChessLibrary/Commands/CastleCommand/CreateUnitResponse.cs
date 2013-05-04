using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class CreateUnitResponse : ICommand
    {
        [ProtoMember(1)]
        private List<Unit> _units;

        public Command Id
        {
            get { return Command.CreateUnitResponse; }
        }

        public List<Unit> Units
        {
            get { return _units; }
            set { _units = value; }
        }
    }
}
