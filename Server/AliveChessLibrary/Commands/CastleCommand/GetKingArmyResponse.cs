using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetKingArmyResponse : ICommand
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
            get { return Command.GetKingArmyResponse; }
        }
    }
}
