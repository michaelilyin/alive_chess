using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class BuildingInCastleRequest : ICommand
    {
        [ProtoMember(1)]
        private InnerBuildingType number;

        public InnerBuildingType Type
        {
            get { return number; }
            set { number = value; }
        }

        public Command Id
        {
            get { return Command.BuildingInCastleRequest; }
           
        }
    }

}
