using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetListBuildingsInCastleResponse : ICommand
    {
        [ProtoMember(1)]
        private IList<IInnerBuilding> list;
        
        public Command Id
        {
            get { return Command.GetListBuildingsInCastleResponse; }
        }

        public IList<IInnerBuilding> List
        {
            get { return list; }
            set { list = value; }
        }
    }
}
