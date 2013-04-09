using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetArmyCastleToKingResponse : ICommand
    {
        [ProtoMember(1)]
        public List<Unit> Arm { get; set; }

        public Command Id
        {
            get { return Command.GetArmyCastleToKingResponse;}
        }
    }
}
