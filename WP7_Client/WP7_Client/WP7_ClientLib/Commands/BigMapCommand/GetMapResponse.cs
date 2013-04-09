using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Objects;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetMapResponse : ICommand
    {
        public GetMapResponse()
        {
        }

        public GetMapResponse(int mapId, int sizeX, int sizeY,
            List<Castle> castles,
            List<Mine> mines,
            List<BasePoint> basePoints,
            List<Border> borders,
            List<SingleObject> single,
            List<MultyObject> multy)
        {
            MapId = mapId;
            SizeMapX = sizeX;
            SizeMapY = sizeY;
            Castles = castles;
            Mines = mines;
            BasePoints = basePoints;
            Borders = borders;
            SingleObjects = single;
            MultyObjects = multy;
        }

        public virtual Command Id
        {
            get { return Command.GetMapResponse; }
        }

        [ProtoMember(1)]
        public int MapId { get; set; }

        [ProtoMember(2)]
        public int SizeMapX { get; set; }

        [ProtoMember(3)]
        public int SizeMapY { get; set; }

        [ProtoMember(4)]
        public List<Castle> Castles { get; set; }

        [ProtoMember(5)]
        public List<Mine> Mines { get; set; }

        [ProtoMember(6)]
        public List<BasePoint> BasePoints { get; set; }

        [ProtoMember(7)]
        public List<SingleObject> SingleObjects { get; set; }

        [ProtoMember(8)]
        public List<MultyObject> MultyObjects { get; set; }

        [ProtoMember(9)]
        public List<Border> Borders { get; set; }
    }
}
