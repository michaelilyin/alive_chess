using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// попросить фигуры у союзников
    /// </summary>
    [ProtoContract]
    public class GetHelpFigureRequest : ICommand
    {
        [ProtoMember(1)]
        private int _figureCount;
        [ProtoMember(2)]
        private UnitType _figureType;

        public Command Id
        {
            get { return Command.GetHelpFigureRequest; }
        }

        public int FigureCount
        {
            get { return _figureCount; }
            set { _figureCount = value; }
        }

        public UnitType FigureType
        {
            get { return _figureType; }
            set { _figureType = value; }
        }
    }
}
