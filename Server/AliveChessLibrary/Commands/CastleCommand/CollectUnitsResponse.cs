using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class CollectUnitsResponse : ICommand
    {
        [ProtoMember(1)]
        private Dictionary<UnitType, int> _castleArmy;
        [ProtoMember(2)]
        private Dictionary<UnitType, int> _kingArmy;

        public Dictionary<UnitType, int> CastleArmy
        {
            get { return _castleArmy; }
            set { _castleArmy = value; }
        }

        public Dictionary<UnitType, int> KingArmy
        {
            get { return _kingArmy; }
            set { _kingArmy = value; }
        }

        public Command Id
        {
            get { return Command.CollectUnitsResponce;}
        }
    }
}
