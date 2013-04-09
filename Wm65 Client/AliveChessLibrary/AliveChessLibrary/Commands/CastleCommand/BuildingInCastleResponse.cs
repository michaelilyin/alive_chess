using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class BuildingInCastleResponse : ICommand
    {
        [ProtoMember(1)]
        private List<InnerBuilding> buildings_list;

        public Command Id
        {
            get { return Command.BuildingInCastleResponse; }
        }

        public List<InnerBuilding> Buildings_list
        {
            get { return buildings_list; }
            set { buildings_list = value; }
        }
    }
}
