using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Landscapes;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetMapResponse3D : GetMapResponse2D
    {
        [ProtoMember(1)]
        private List<Height3D> _heightMap;

        public override Command Id
        {
            get { return Command.GetMapResponse3D; }
        }

        public List<Height3D> HeightMap
        {
            get { return _heightMap; }
            set { _heightMap = value; }
        }
    }
}
