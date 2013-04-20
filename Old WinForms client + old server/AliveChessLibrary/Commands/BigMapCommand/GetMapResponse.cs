using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetMapResponse : ICommand
    {
        [ProtoMember(1)]
        private uint mapId;
        [ProtoMember(2)]
        private int sizeMapX;
        [ProtoMember(3)]
        private int sizeMapY;
        [ProtoMember(4)]
        private List<MapPoint> _points;
        [ProtoMember(5)]
        private List<MapSector> _objects;
        [ProtoMember(6)]
        private List<BasePoint> _basePoints;

        public GetMapResponse()
        {
        }

        public GetMapResponse(uint mapId, int sizeX, int sizeY, List<MapPoint> points,
            List<MapSector> sectors, List<BasePoint> basePoints)
        {
            this.mapId = mapId;
            this.sizeMapX = sizeX;
            this.sizeMapY = sizeY;
            this._points = points;
            this._objects = sectors;
            this._basePoints = basePoints;
        }

        public Command Id
        {
            get { return Command.GetMapResponse; }
        }

        public uint MapId
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

        public List<MapPoint> Points
        {
            get { return _points; }
            set { _points = value; }
        }

        public List<MapSector> Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }

        public List<BasePoint> BasePoints
        {
            get { return _basePoints; }
            set { _basePoints = value; }
        }
    }
}
