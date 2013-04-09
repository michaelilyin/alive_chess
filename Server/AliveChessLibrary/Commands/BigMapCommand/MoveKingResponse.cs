using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// перемещение короля
    /// </summary>
    [ProtoContract]
    public class MoveKingResponse : ICommand
    {
        [ProtoMember(1)]
        private List<Position> _steps;

        public MoveKingResponse()
        {
        }

        public MoveKingResponse(List<Position> steps)
        {
            this._steps = steps;
        }

        public Command Id
        {
            get { return Command.MoveKingResponse; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public List<Position> Steps
        {
            get { return _steps; }
            set { _steps = value; }
        }
    }
}
