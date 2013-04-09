using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Utility;
using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class SendFigureHelpMessage : ICommand
    {
        [ProtoMember(1)]
        private int _receiverId;
        [ProtoMember(2)]
        private List<Unit> _units;
        [ProtoMember(3)]
        private int _fromCastle;

        public SendFigureHelpMessage()
        {
        }

        public SendFigureHelpMessage(List<Unit> units)
        {
            this._units = units;
        }

        public Command Id
        {
            get { return Command.SendFigureHelpMessage; }
        }

        public void AddFigure(Unit u)
        {
            if (_units == null)
                _units = new List<Unit>();

            int index = -1;
            if ((index = _units.FindIndex<Unit>(x => x.UnitType == u.UnitType)) < 0)
                _units.Add(u);
            else _units[index].UnitCount += u.UnitCount;
        }

        public List<Unit> Units
        {
            get { return _units; }
            set { _units = value; }
        }

        public int ReceiverId
        {
            get { return _receiverId; }
            set { _receiverId = value; }
        }

        public int FromCastle
        {
            get { return _fromCastle; }
            set { _fromCastle = value; }
        }
    }
}
