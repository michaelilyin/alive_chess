using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetListBuildingsInCastleResponse : ICommand
    {
        public Command Id
        {
            get { return Command.GetListBuildingsInCastleResponse; }
        }

        [ProtoMember(1)]
        public IList<IInnerBuilding> List { get; set; }
    }
}
