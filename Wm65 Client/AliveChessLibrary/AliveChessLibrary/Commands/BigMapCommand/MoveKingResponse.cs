using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class MoveKingResponse : ICommand
    {
        [ProtoMember(1)]
        private List<Position> steps;

        public MoveKingResponse()
        {
        }

        public MoveKingResponse(List<Position> steps)
        {
            this.steps = steps;
        }

        public Command Id
        {
            get { return Command.MoveKingResponse; }
        }

        public List<Position> Steps
        {
            get { return steps; }
            set { steps = value; }
        }
    }
}
