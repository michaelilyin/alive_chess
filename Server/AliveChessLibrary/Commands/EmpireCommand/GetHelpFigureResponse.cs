using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// попросить фигуры у союзников
    /// </summary>
    [ProtoContract]
    public class GetHelpFigureResponse : ICommand
    {
        [ProtoMember(1)]
        private List<Unit> _units;

        public GetHelpFigureResponse()
        {
        }

        public GetHelpFigureResponse(List<Unit> units)
        {
            this._units = units;
        }

        public Command Id
        {
            get { return Command.GetHelpFigureResponse; }
        }

        public List<Unit> Units
        {
            get { return _units; }
            set { _units = value; }
        }
    }
}
