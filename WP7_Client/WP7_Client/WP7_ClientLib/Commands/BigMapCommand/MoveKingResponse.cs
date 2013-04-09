using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class MoveKingResponse : ICommand
    {
        public MoveKingResponse()
        {
        }

        public MoveKingResponse(List<Position> steps)
        {
            Steps = steps;
        }

        public Command Id
        {
            get { return Command.MoveKingResponse; }
        }

        [ProtoMember(1)]
        public List<Position> Steps { get; set; }
    }
}
