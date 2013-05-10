using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class DestroyBuildingRequest : ICommand
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
            get { return Command.DestroyBuildingRequest; }
           
        }
    }

}
