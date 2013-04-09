using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetListBuildingsInCastleResponse : ICommand
    {
        [ProtoMember(1)]
        private List<InnerBuilding> list;
        
        public Command Id
        {
            get { return Command.GetListBuildingsInCastleResponse; }
        }

        public List<InnerBuilding> List
        {
            get { return list; }
            set { list = value; }
        }
    }
}
