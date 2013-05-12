using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class LeaveUnitsRequest : ICommand
    {
        [ProtoMember(1)]
        private Dictionary<UnitType, int> _units;

        public Dictionary<UnitType, int> Units
        {
            get { return _units; }
            set { _units = value; }
        }

        public Command Id
        {
            get { return  Command.LeaveUnitsRequest;}
        }
    }
}
