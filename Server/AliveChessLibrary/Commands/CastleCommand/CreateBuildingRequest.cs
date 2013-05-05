using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class CreateBuildingRequest : ICommand
    {
        [ProtoMember(1)]
        private InnerBuildingType type;

        public InnerBuildingType InnerBuildingType
        {
            get { return type; }
            set { type = value; }
        }

        public Command Id
        {
            get { return Command.CreateBuildingRequest; }
           
        }
    }

}
