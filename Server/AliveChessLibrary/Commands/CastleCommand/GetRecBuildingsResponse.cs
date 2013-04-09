using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetRecBuildingsResponse : ICommand
    {
        //[ProtoMember(1)]
        private int _count;
        //[ProtoMember(2)]
        private ResourceTypes _type;
        [ProtoMember(1)]
        private ResBuild resBuild;

        public Command Id
        {
            get { return Command.GetRecBuildingsResponse; }
        }

        public ResBuild ResBuild
        {
            get { return resBuild; }
            set { resBuild = value; }
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
