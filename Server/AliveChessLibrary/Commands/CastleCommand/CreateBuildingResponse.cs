using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class CreateBuildingResponse : ICommand
    {
        [ProtoMember(1)]
        private List<InnerBuilding> _buildings;

        public Command Id
        {
            get { return Command.CreateBuildingResponse; }
        }

        public List<InnerBuilding> Buildings
        {
            get { return _buildings; }
            set { _buildings = value; }
        }
    }
}
