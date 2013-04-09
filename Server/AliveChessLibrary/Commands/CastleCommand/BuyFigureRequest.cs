using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class BuyFigureRequest : ICommand
    {
        [ProtoMember(1)]
        private int _figureCount;
        [ProtoMember(2)]
        private UnitType _figureType;

        public Command Id
        {
            get { return  Command.BuyFigureRequest; }
            
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
