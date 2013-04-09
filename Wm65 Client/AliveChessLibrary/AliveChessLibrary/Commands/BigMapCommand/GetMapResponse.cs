using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Landscapes;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// Only for 2D
    /// </summary>
    [ProtoContract]
    public class GetMapResponse : ICommand
    {
        [ProtoMember(1)]
        private int mapId;
        [ProtoMember(2)]
        private int sizeMapX;
        [ProtoMember(3)]
        private int sizeMapY;
        [ProtoMember(4)]
        private List<Castle> _castles;
        [ProtoMember(5)]
        private List<Mine> _mines;
        [ProtoMember(6)]
        private List<BasePoint> _basePoints;

        public GetMapResponse()
        {
        }

        public GetMapResponse(int mapId, int sizeX, int sizeY, List<Castle> castles,
            List<Mine> mines, List<BasePoint> basePoints)
        {
            this.mapId = mapId;
            this.sizeMapX = sizeX;
            this.sizeMapY = sizeY;
            this._castles = castles;
            this._mines = mines;
            this._basePoints = basePoints;
        }

        public virtual Command Id
        {
            get { return Command.GetMapResponse; }
        }

        public int MapId
        {
            get { return mapId; }
            set { mapId = value; }
        }

        public int SizeMapX
        {
            get { return sizeMapX; }
            set { sizeMapX = value; }
        }

        public int SizeMapY
        {
            get { return sizeMapY; }
            set { sizeMapY = value; }
        }

        public List<Castle> Castles
        {
            get { return _castles; }
            set { _castles = value; }
        }

        public List<Mine> Mines
        {
            get { return _mines; }
            set { _mines = value; }
        }

        public List<BasePoint> BasePoints
        {
            get { return _basePoints; }
            set { _basePoints = value; }
        }
    }
}
