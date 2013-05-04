using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.Commands.CastleCommand
{
    public class GetKingArmyResponse : ICommand
    {
        [ProtoMember(1)]
        private List<Unit> _units;

        public List<Unit> Units
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
