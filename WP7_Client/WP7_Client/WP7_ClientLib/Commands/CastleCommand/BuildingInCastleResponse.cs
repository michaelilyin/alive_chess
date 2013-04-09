using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class BuildingInCastleResponse : ICommand
    {
        public Command Id
        {
            get { return Command.BuildingInCastleResponse; }
        }

        [ProtoMember(1)]
        public List<InnerBuilding> Buildings_list { get; set; }
    }
}
