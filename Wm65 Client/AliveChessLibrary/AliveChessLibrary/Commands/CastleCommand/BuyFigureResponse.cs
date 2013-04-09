using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class BuyFigureResponse : ICommand
    {
        [ProtoMember(1)]
        private List<Unit> _units;

        public Command Id
        {
            get { return Command.BuyFigureResponse; }
        }

        public List<Unit> Units
        {
            get { return _units; }
            set { _units = value; }
        }
    }
}
