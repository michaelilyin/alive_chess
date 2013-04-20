using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetArmyCastleToKingResponse : ICommand
    {
        [ProtoMember(1)]
        private List<Unit> arm;

        public List<Unit> Arm
        {
            get { return arm; }
            set { arm = value; }
        }
        public Command Id
        {
            get { return Command.GetArmyCastleToKingResponse;}
        }
    }
}
