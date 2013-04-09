using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class BuildingInCastleRequest : ICommand
    {
        [ProtoMember(1)]
        public InnerBuildingType Type { get; set; }

        public Command Id
        {
            get { return Command.BuildingInCastleRequest; }
           
        }
    }

}
