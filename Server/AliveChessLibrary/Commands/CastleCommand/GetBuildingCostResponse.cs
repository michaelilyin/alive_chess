using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetBuildingCostResponse : ICommand
    {
        //[ProtoMember(1)]
        private int _count;
        //[ProtoMember(2)]
        private ResourceTypes _type;
        [ProtoMember(1)]
        private CreationRequirements _buidningCost;

        public Command Id
        {
            get { return Command.GetBuildingCostResponse; }
        }

        public CreationRequirements BuildingCost
        {
            get { return _buidningCost; }
            set { _buidningCost = value; }
        }

        //public int Count
        //{
        //    get { return _count; }
        //    set { _count = value; }
        //}

        //public ResourceTypes Type
        //{
        //    get { return _type; }
        //    set { _type = value; }
        //}
    }
}
