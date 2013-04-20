using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetObjectsResponse : ICommand
    {
        [ProtoMember(1)]
        private List<MapPoint> _objects;
        [ProtoMember(2)]
        private List<MapSector> _sectors;

        public GetObjectsResponse(){}

        public GetObjectsResponse(List<MapPoint> objects, List<MapSector> sectors)
        {
            this._objects = objects;
            this._sectors = sectors;
        }

        public Command Id
        {
            get { return Command.GetObjectsResponse; }
        }

        public List<MapPoint> Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }

        public List<MapSector> Sectors
        {
            get { return _sectors; }
            set { _sectors = value; }
        }
    }
}
