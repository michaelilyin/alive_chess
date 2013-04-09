using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    [ProtoContract]
    public sealed class BasePoint : ISinglePoint
    {
        public BasePoint()
        {
            
        }

        public BasePoint(Map map, MapPoint point)
        {
            Map = map;
            if (point != null)
                AddView(point);
        }

        public void AddView(MapPoint point)
        {
            ViewOnMap = point;
            ViewOnMap.SetOwner(this);
        }

        public void RemoveView()
        {
            ViewOnMap.SetOwner(null);
        }

        public MapPoint ViewOnMap { get; set; }

        [ProtoMember(2)]
        public int X { get; set; }

        [ProtoMember(3)]
        public int Y { get; set; }

        public PointTypes Type
        {
            get { return PointTypes.Landscape; }
        }

        [ProtoMember(5)]
        public float WayCost { get; set; }

        [ProtoMember(1)]
        public int Id { get; set; }

        public int ImageId { get; set; }

        [ProtoMember(4)]
        public LandscapeTypes LandscapeType { get; set; }

        public Map Map { get; set; }

    }
}
