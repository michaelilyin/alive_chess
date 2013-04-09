using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetRecBuildingsResponse : ICommand
    {
        public Command Id
        {
            get { return Command.GetRecBuildingsResponse; }
        }

        [ProtoMember(1)]
        public ResBuild ResBuild { get; set; }
    }
}
